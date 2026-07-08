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
        return _mapper.Map<List<ReadProcessoDto>>(
            _apiContext.Processos
                .Include(p => p.Status)
                .Skip(skip)
                .Take(take)
                .ToList());
    }

    public ReadProcessoDto BuscarProcessoPorIdDto(long id)
    {
        var processo = _apiContext.Processos.Include(p => p.Status).FirstOrDefault(x => x.Id == id);
        return processo == null ? throw new NotFoundException("Processo não econtrado") : _mapper.Map<ReadProcessoDto>(processo);
    }
    
    private Processo? BuscarProcessoPorId(long id)
    {
        var processo = _apiContext.Processos.FirstOrDefault(x => x.Id == id);
        return processo ?? throw new NotFoundException("Processo não econtrado");
    }
    
    

    public ReadProcessoDto CadastrarProcesso(CreateProcessoDto createProcessoDto)
    {
        //Busca statusProcesso
        var statusProcesso = _statusProcessoService.RetornaStatusPorId(createProcessoDto.StatusProcessoId);
        if (statusProcesso == null) throw new NotFoundException("Status não econtrado");
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
        _andamentoService.AtribuirAndamentoProcesso(processo,createProcessoDto.Andamento);
        return new ReadProcessoDto(
            processo.Id,
            processo.Descricao,
            _mapper.Map<ReadStatusProcessoDto>(processo.Status),
            createProcessoDto.Partes,
            createProcessoDto.Andamento);
    }
    
    public void AtualizarProcesso(long id, UpdateProcessoDto updateProcessoDto)
    {
        var processo = _mapper.Map<Processo>(updateProcessoDto);
        var processoSalvo = this.BuscarProcessoPorId(id);
        if (processoSalvo == null) throw new NotFoundException("Processo não encontrado");
        var statusProcesso = _statusProcessoService.RetornaStatusPorId(updateProcessoDto.StatusProcessoId);
        if (statusProcesso == null) throw new NotFoundException("Status não econtrado");
        processoSalvo.Descricao = processo.Descricao;
        processoSalvo.Status = statusProcesso;
        processoSalvo.UltimaAlteracao = DateTime.UtcNow;
        _apiContext.Processos.Update(processoSalvo);
        _apiContext.SaveChanges();
    }

    public void DeletarProcesso(long id)
    {
        var processoSalvo = this.BuscarProcessoPorId(id);
        if (processoSalvo == null) throw new NotFoundException("Processo não encontrado");
        _apiContext.Processos.Remove(processoSalvo);
        _apiContext.SaveChanges();
    }
}