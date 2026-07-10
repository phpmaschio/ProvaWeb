using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProcessosAPI.Data;
using ProcessosAPI.DTOs;
using ProcessosAPI.Exceptions;
using ProcessosAPI.Models;

namespace ProcessosAPI.Services;

public class ProcessoService
{
    private readonly ProcessoApiContext _apiContext;
    private readonly IMapper _mapper;
    private readonly StatusProcessoService _statusProcessoService;
    private readonly ParteProcessoService _parteProcessoService;
    private readonly ParteService _parteService;
    private readonly AndamentoService _andamentoService;
    
    public ProcessoService(
        StatusProcessoService statusProcessoService, 
        ParteProcessoService parteProcessoService ,
        ParteService parteService,
        AndamentoService andamentoService,
        ProcessoApiContext apiContext,  
        IMapper mapper)
    {
        _apiContext = apiContext;
        _mapper = mapper;
        _statusProcessoService = statusProcessoService;
        _parteProcessoService = parteProcessoService;
        _parteService = parteService;
        _andamentoService = andamentoService;
    }

    public List<ReadProcessoDto> ListarProcessos(int skip, int take)
    {
        var processosDb = _apiContext.Processos
            .Include(p => p.Status)
            .Include(p => p.PartesProcessos)
            .ThenInclude(pp => pp.Parte)
            .Skip(Math.Max(skip, 0))
            .Take(Math.Clamp(take, 1, 100))
            .ToList();

        return processosDb.Select(MontarProcessoDto).ToList();
    }

    public ReadProcessoDto BuscarProcessoPorIdDto(long id)
    {
        var processo = _apiContext.Processos
            .Include(p => p.Status)
            .Include(p => p.PartesProcessos)
            .ThenInclude(pp => pp.Parte)
            .FirstOrDefault(x => x.Id == id);

        return processo == null ? throw new NotFoundException("Processo não encontrado") : MontarProcessoDto(processo);
    }

    // Usa a expressão 'with' do C# (já que seu DTO é um record) para preencher as coleções e relacionamentos que faltam
    private ReadProcessoDto MontarProcessoDto(Processo p)
    {
        var andamento = _andamentoService.BuscarAndamentoAtualDoProcesso(p.Id);

        var processoDto = _mapper.Map<ReadProcessoDto>(p);

        return processoDto with
        {
            // Navegamos por PartesProcessos para extrair as Partes e mapeá-las para ReadParteDto
            Partes = p.PartesProcessos
                .Select(pp => _mapper.Map<ReadParteDto>(pp.Parte))
                .ToList(),

            Andamento = andamento == null
                ? null
                : _mapper.Map<ReadAndamentoAtualDto>(andamento)
        };
    }
    
    private Processo? BuscarProcessoPorId(long id)
    {
        var processo = _apiContext.Processos.FirstOrDefault(x => x.Id == id);
        return processo ?? throw new NotFoundException("Processo não encontrado");
    }
    
    

    public ReadProcessoDto CadastrarProcesso(CreateProcessoDto createProcessoDto)
    {
        using var transaction = _apiContext.Database.BeginTransaction();

        //Busca statusProcesso
        var statusProcesso = _statusProcessoService.RetornaStatusPorId(createProcessoDto.StatusProcessoId);
        if (statusProcesso == null) throw new NotFoundException("Status não encontrado");
        var processo = _mapper.Map<Processo>(createProcessoDto);
        processo.Status = statusProcesso;
        processo.CriadoEm = DateTime.UtcNow;
        _apiContext.Processos.Add(processo);
        _apiContext.SaveChanges();

        //Atriuir partes ao processo
        List<Parte> partes = createProcessoDto.Partes
            .Select(parteDto => _parteService.BuscarPartePorId(parteDto.Id)
                                ?? throw new NotFoundException("Erro ao atribuir Partes, parte não encontrada"))
            .ToList();
        _parteProcessoService.AtribuirPartesAoProcesso(processo, partes );

        //Atribuir andamento
        ReadAndamentoAtualDto andamentoAtualDto = _andamentoService.AtribuirAndamentoProcesso(processo,createProcessoDto.Andamento);

        transaction.Commit();

        return new ReadProcessoDto(
            processo.Id,
            processo.Descricao,
            _mapper.Map<ReadStatusProcessoDto>(processo.Status),
            createProcessoDto.Partes,
            andamentoAtualDto,
            processo.CriadoEm);
    }
public void AtualizarProcesso(long id, UpdateProcessoDto updateProcessoDto)
{
    using var transaction = _apiContext.Database.BeginTransaction();

    var processo = _mapper.Map<Processo>(updateProcessoDto);
    var processoSalvo = this.BuscarProcessoPorId(id);
    
    if (processoSalvo == null) throw new NotFoundException("Processo não encontrado");
    
    var statusProcesso = _statusProcessoService.RetornaStatusPorId(updateProcessoDto.StatusProcessoId);
    if (statusProcesso == null) throw new NotFoundException("Status não encontrado");
    
    // Atualiza dados básicos
    processoSalvo.Descricao = processo.Descricao;
    processoSalvo.Status = statusProcesso;
    processoSalvo.UltimaAlteracao = DateTime.UtcNow;
    
    _apiContext.Processos.Update(processoSalvo);

    // Alterar andamento (só cria um novo registro se a descrição realmente mudou,
    // evitando duplicar o histórico a cada edição do processo)
    var andamentoAtual = _andamentoService.BuscarAndamentoAtualDoProcesso(processoSalvo.Id);
    var descricaoMudou = andamentoAtual == null
        || !string.Equals(andamentoAtual.Descricao, updateProcessoDto.Andamento.Descricao, StringComparison.OrdinalIgnoreCase);

    if (descricaoMudou)
    {
        this._andamentoService.AtribuirAndamentoProcesso(processoSalvo, updateProcessoDto.Andamento);
    }
    
    // ==========================================
    // ALTERAR PARTES (Usando seu modelo com Navegação)
    // ==========================================
    
    // 1. Extraímos os IDs que vieram do Frontend
    var idsPartesRecebidas = updateProcessoDto.Partes.Select(p => p.Id).ToList();

    // 2. Buscamos os vínculos atuais (com os Includes necessários para acessar as propriedades de navegação)
    var vinculosAtuais = _apiContext.PartesProcessos
        .Include(pp => pp.Parte)    // <-- Precisa incluir para podermos ler o pp.Parte.Id
        .Include(pp => pp.Processo) // <-- Precisa incluir para comparar o Processo
        .Where(pp => pp.Processo.Id == processoSalvo.Id)
        .ToList();
        
    var idsPartesAtuais = vinculosAtuais.Select(pp => pp.Parte.Id).ToList();

    // 3. REMOVER: Vínculos que estão no banco, mas NÃO vieram na atualização
    var vinculosParaRemover = vinculosAtuais
        .Where(pp => !idsPartesRecebidas.Contains(pp.Parte.Id))
        .ToList();

    _apiContext.PartesProcessos.RemoveRange(vinculosParaRemover);

    // 4. ADICIONAR: IDs que vieram na atualização, mas NÃO estão no banco
    var idsParaAdicionar = idsPartesRecebidas
        .Where(id => !idsPartesAtuais.Contains(id))
        .ToList();

    foreach (var idAdicionar in idsParaAdicionar)
    {
        var parteExiste = _parteService.BuscarPartePorId(idAdicionar);
        if (parteExiste == null) 
            throw new NotFoundException($"Erro ao atribuir Partes, parte de ID {idAdicionar} não encontrada");

        // Utilizando o construtor customizado que você criou no modelo ParteProcesso!
        var novoVinculo = new ParteProcesso(parteExiste, processoSalvo);
        
        _apiContext.PartesProcessos.Add(novoVinculo);
    }

    // Salva tudo na mesma transação
    _apiContext.SaveChanges();
    transaction.Commit();
}
    public void DeletarProcesso(long id)
    {
        var processoSalvo = this.BuscarProcessoPorId(id);
        if (processoSalvo == null) throw new NotFoundException("Processo não encontrado");
        _apiContext.Processos.Remove(processoSalvo);
        _apiContext.SaveChanges();
    }
}