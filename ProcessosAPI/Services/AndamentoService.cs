using AutoMapper;
using ProcessosAPI.Data;
using ProcessosAPI.DTOs;
using ProcessosAPI.Models;

namespace ProcessosAPI.Services;

public class AndamentoService
{
    private readonly IMapper _mapper;
    private readonly ProcessoApiContext _context;

    public AndamentoService(ProcessoApiContext context,IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    public ReadAndamentoAtualDto AtribuirAndamentoProcesso(Processo processo, CreateAndamentoDto andamentoDto)
    {
        Andamento andamento = _mapper.Map<Andamento>(andamentoDto);
        andamento.Processo = processo;
        andamento.AtribuidoEm = DateTime.UtcNow;
        _context.Andamentos.Add(andamento);
        _context.SaveChanges();
        return _mapper.Map<ReadAndamentoAtualDto>(andamento);
    }

    public Andamento? BuscarAndamentoAtualDoProcesso(long processoId)
    {
        return _context.Andamentos
            .Where(a => a.Processo.Id == processoId)
            .OrderByDescending(a => a.AtribuidoEm)
            .FirstOrDefault();
    }

    public List<ReadAndamentoAtualDto> ListarAndamentos()
    {
        return _mapper.Map<List<ReadAndamentoAtualDto>>(_context.Andamentos);
    }
}