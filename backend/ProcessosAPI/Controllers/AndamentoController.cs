using Microsoft.AspNetCore.Mvc;
using ProcessosAPI.Services;

namespace ProcessosAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AndamentoController: ControllerBase
{

    private readonly AndamentoService _andamentoService;

    public AndamentoController(AndamentoService andamentoService)
    {
        _andamentoService = andamentoService;
    }

    [HttpGet]
    public IActionResult ListarAndamentos()
    {
        return Ok(_andamentoService.ListarAndamentos());
    }

    [HttpGet("Processo/{id}")]
    public IActionResult ListarAndamentos(long id)
    {
        return Ok(_andamentoService.BuscarAndamentoPorProcesso(id));
    }
}
