using AutoMapper;
using ProcessosAPI.Data;
using ProcessosAPI.DTOs;
using ProcessosAPI.Exceptions;

namespace ProcessosAPI.Services;

public class StatusProcessoService
{
    private readonly ProcessoApiContext _apiContext; 
    private readonly IMapper _mapper;
    
    public StatusProcessoService(ProcessoApiContext apiContext , IMapper mapper)
    {
        _apiContext = apiContext;
        _mapper = mapper;
    }

    public ReadStatusProcessoDto CadastrarStatusProcesso(CreateStatusProcessoDto createStatusProcessoDto)
    {
        var statusProcesso = _mapper.Map<StatusProcesso>(createStatusProcessoDto);
        
        statusProcesso.Descricao = createStatusProcessoDto.Descricao;
        statusProcesso.CriadoEm = DateTime.UtcNow;
        
        _apiContext.StatusProcessos.Add(statusProcesso);
        _apiContext.SaveChanges();
        return _mapper.Map<ReadStatusProcessoDto>(statusProcesso);
    }

    public void AtualizarStatusProcesso(long id, UpdateStatusProcessoDto updateStatusProcessoDto)
    {
        var statusProcessoSalvo = _apiContext.StatusProcessos.FirstOrDefault(x => x.Id == id);
        if (statusProcessoSalvo == null) throw new NotFoundException("Status não encontrado");
        var statusProcesso = _mapper.Map<StatusProcesso>(updateStatusProcessoDto);
        statusProcessoSalvo.Descricao = statusProcesso.Descricao;
        statusProcessoSalvo.UltimaAlteracao = DateTime.UtcNow;
        _apiContext.SaveChanges();
    }
    
    public IEnumerable<ReadStatusProcessoDto> ListarStatus(int skip, int take)
    {
        return _mapper.Map<List<ReadStatusProcessoDto>>(_apiContext.StatusProcessos.Skip(Math.Max(skip, 0)).Take(Math.Clamp(take, 1, 100)).ToList());
    }
    
    public ReadStatusProcessoDto RetornaStatusPorIdDto(long id)
    {
        var statusProcesso = _apiContext.StatusProcessos.FirstOrDefault(x => x.Id == id);
        return statusProcesso == null ? throw new NotFoundException("Status não encontrado") : _mapper.Map<ReadStatusProcessoDto>(statusProcesso);
    }
    
    public StatusProcesso RetornaStatusPorId(long id)
    {
        var statusProcesso = _apiContext.StatusProcessos.FirstOrDefault(x => x.Id == id);
        return statusProcesso ?? throw new NotFoundException("Status não encontrado");
        
    }

    public void ExcluirStatusProcesso(long id)
    {
        var statusProcesso = this.RetornaStatusPorId(id);
        if(statusProcesso == null) throw new NotFoundException("Status não encontrado");
        _apiContext.Remove(statusProcesso);
        _apiContext.SaveChanges();
    }
}