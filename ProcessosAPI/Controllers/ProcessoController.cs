

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProcessosAPI.Data;
using ProcessosAPI.DTOs;
using ProcessosAPI.Exceptions;
using ProcessosAPI.Services;

namespace ProcessosAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class ProcessoController : ControllerBase
{
    private readonly ProcessoService _processoService;

    public ProcessoController(ProcessoApiContext context,IMapper mapper)
    {
        _processoService = new ProcessoService(
            new StatusProcessoService(context,mapper), 
            new ParteProcessoService(context,mapper),
            new ParteService(context, mapper),
            new AndamentoService(context,mapper),
            context,
            mapper);
    }

    [HttpGet]
    public IActionResult ListarProcesso([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        try
        {
            return Ok(_processoService.ListarProcessos(skip, take));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(); 
        }
    }
    
    [HttpGet("{id:long}")]
    public IActionResult BuscarProcessoPorId(long id)
    {
        try
        {
            return Ok(_processoService.BuscarProcessoPorIdDto(id));
        }
        catch (NotFoundException e) {
            Console.WriteLine(e);
            return NotFound(e.Message); 
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(); 
        }
    }

    [HttpPost]
    public IActionResult CadastrarProcesso([FromBody] CreateProcessoDto createProcessoDto)
    {
        try
        {
            return Ok(_processoService.CadastrarProcesso(createProcessoDto));
        }
        catch (NotFoundException e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest();
        }
    }
    
    
    [HttpPut("{id:long}")]
    public IActionResult AtualizarProcesso(long id, [FromBody] UpdateProcessoDto updateProcessoDto)
    {
        try
        {
            _processoService.AtualizarProcesso(id, updateProcessoDto);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest();
        }
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeletarProcesso(long id)
    {
        try
        {
            _processoService.DeletarProcesso(id);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest();
        }
    }
}