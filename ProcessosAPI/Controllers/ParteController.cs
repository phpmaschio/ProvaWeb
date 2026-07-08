using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProcessosAPI.Data;
using ProcessosAPI.DTOs;
using ProcessosAPI.Exceptions;
using ProcessosAPI.Services;

namespace ProcessosAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ParteController : ControllerBase
{
    private readonly ParteService _parteService;

    public ParteController(ProcessoApiContext context, IMapper mapper)
    {
        _parteService = new ParteService(context, mapper);
    }
    
    [HttpGet]
    public IActionResult ListarPartes([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        try
        {
            return Ok(_parteService.ListarPartes(skip, take));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(); 
        }
    }
    
    [HttpGet("{id}")]
    public IActionResult BuscarPartePorId(long id)
    {
        try
        {
            return Ok(_parteService.BuscarPartePorIdDto(id));
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
    public IActionResult CadastrarParte([FromBody] CreateParteDto createParteDto)
    {
        try
        {
            return Created("",_parteService.CadastrarParte(createParteDto));
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
    
    
    [HttpPut("{id}")]
    public IActionResult AtualizarParte(long id, [FromBody] UpdateParteDto updateParteDto)
    {
        try
        {
            _parteService.AtualizarParte(id, updateParteDto);
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
    public IActionResult DeletarParte(long id)
    {
        try
        {
            _parteService.DeletarParte(id);
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