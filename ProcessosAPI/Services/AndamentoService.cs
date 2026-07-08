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

    public void AtribuirAndamentoProcesso(Processo processo, ReadAndamentoAtualDto andamentoDto)
    {
        Andamento andamento = _mapper.Map<Andamento>(andamentoDto);
        andamento.Processo = processo;
        andamento.AtribuidoEm = DateTime.UtcNow;
        _context.Andamentos.Add(andamento);
        _context.SaveChanges();
    }
}