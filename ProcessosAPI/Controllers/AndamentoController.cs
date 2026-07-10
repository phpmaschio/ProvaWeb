using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProcessosAPI.Data;
using ProcessosAPI.Exceptions;
using ProcessosAPI.Services;

namespace ProcessosAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AndamentoController: ControllerBase
{

    private readonly AndamentoService _andamentoService;
    
    public AndamentoController(ProcessoApiContext context, IMapper mapper)
    {
        _andamentoService = new AndamentoService(context, mapper);
    }
    
    [HttpGet]
    public IActionResult ListarAndamentos()
    {
        try
        {
            return Ok(_andamentoService.ListarAndamentos());
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
    [HttpGet("Processo/{id}")]
    public IActionResult ListarAndamentos(long id)
    {
        try
        {
            return Ok(_andamentoService.BuscarAndamentoPorProcesso(id));
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