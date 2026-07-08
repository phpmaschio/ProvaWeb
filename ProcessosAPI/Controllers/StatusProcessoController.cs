using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProcessosAPI.Data;
using ProcessosAPI.DTOs;
using ProcessosAPI.Exceptions;
using ProcessosAPI.Services;

namespace ProcessosAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class StatusProcessoController : ControllerBase
{
    private readonly StatusProcessoService _statusProcessoService;

    public StatusProcessoController(ProcessoApiContext apiContext, IMapper mapper)
    {
        _statusProcessoService = new StatusProcessoService(apiContext, mapper);
    }
    
    [HttpGet]
    public IActionResult RetornaTodosStatus([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        try
        {
            return Ok(_statusProcessoService.ListarStatus(skip, take));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(); 
        }
    }
    
    [HttpGet("{id:long}")]
    public IActionResult RetornaStatusPorId(long id)
    {
        try
        {
            return Ok(_statusProcessoService.RetornaStatusPorIdDto(id));
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    public CreatedAtActionResult CadastraStatusProcesso([FromBody] CreateStatusProcessoDto createStatusProcessoDto)
    {
        try
        {
            var statusProcesso = _statusProcessoService.CadastrarStatusProcesso(createStatusProcessoDto);
        
            return CreatedAtAction(
                nameof(RetornaStatusPorId),
                new { id = statusProcesso.Id },
                statusProcesso);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
       
    }

    [HttpPut("{id:long}")]
    public IActionResult AtualizaStatusProcesso([FromBody] UpdateStatusProcessoDto updateStatusProcessoDto, long id)
    {
        try
        {
            _statusProcessoService.AtualizarStatusProcesso(id, updateStatusProcessoDto);
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
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete("{id}")]
    public IActionResult ExcluirStatusProcesso(long id)
    {
        try
        {
            _statusProcessoService.ExcluirStatusProcesso(id);
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
            return BadRequest(e.Message);
        }
    }
}