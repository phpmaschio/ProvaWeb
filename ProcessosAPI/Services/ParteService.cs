using AutoMapper;
using ProcessosAPI.Data;
using ProcessosAPI.DTOs;
using ProcessosAPI.Exceptions;
using ProcessosAPI.Models;

namespace ProcessosAPI.Services;

public class ParteService
{
    private readonly ProcessoApiContext _context;
    private readonly IMapper _mapper;

    public ParteService(ProcessoApiContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Parte? BuscarPartePorId(long id)
    {
        return _context.Partes.FirstOrDefault(x => x.Id == id);
    }
    
    public List<ReadParteDto> ListarPartes(int skip, int take)
    {
        return _mapper.Map<List<ReadParteDto>>(_context.Partes.Skip(skip).Take(take).ToList());
    }

    public ReadParteDto? BuscarPartePorIdDto(long id)
    {
        var parte = _context.Partes.FirstOrDefault(x => x.Id == id);
        return parte == null ? throw new NotFoundException("Parte não encontrada") : _mapper.Map<ReadParteDto>(parte);
    }
    
    public CreateParteDto CadastrarParte(CreateParteDto cadastrarParteDto)
    {
        var parte = _mapper.Map<Parte>(cadastrarParteDto);
        parte.CriadoEm = DateTime.UtcNow;
        _context.Partes.Add(parte);
        _context.SaveChanges();
        return _mapper.Map<CreateParteDto>(parte);
    }

    public void AtualizarParte(long id, UpdateParteDto updateParteDto)
    {
        var novaParte =  _mapper.Map<Parte>(updateParteDto);
        var parteSalva = this.BuscarPartePorId(id);
        if(parteSalva==null) throw new NotFoundException("Parte não encontrada");
        parteSalva.Nome = novaParte.Nome;
        parteSalva.Tipo = novaParte.Tipo;
        parteSalva.UltimaAltercao = DateTime.UtcNow;
        _context.Partes.Update(parteSalva);
        _context.SaveChanges();
    }

    public void DeletarParte(long id)
    {
        var parteSalva = this.BuscarPartePorId(id);
        if(parteSalva==null) throw new NotFoundException("Parte não encontrada");
        _context.Partes.Remove(parteSalva);
        _context.SaveChanges();
    }
}