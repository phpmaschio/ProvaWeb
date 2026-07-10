using AutoMapper;
using ProcessosAPI.Data;
using ProcessosAPI.Models;

namespace ProcessosAPI.Services;

public class ParteProcessoService
{
 
    private readonly IMapper _mapper;
    private readonly ProcessoApiContext _context;

    public ParteProcessoService(ProcessoApiContext context,IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    public void AtribuirPartesAoProcesso(Processo processo, List<Parte> partes)
    {
        foreach (var parte in partes)
        {
            ParteProcesso parteProcesso = new ParteProcesso(parte,processo);
            _context.PartesProcessos.Add(parteProcesso);
            _context.SaveChanges();
        }
    }
}