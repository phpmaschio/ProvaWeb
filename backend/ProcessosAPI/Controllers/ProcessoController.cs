using Microsoft.AspNetCore.Mvc;
using ProcessosAPI.DTOs;
using ProcessosAPI.Services;

namespace ProcessosAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProcessoController : ControllerBase
{
    private readonly ProcessoService _processoService;

    public ProcessoController(ProcessoService processoService)
    {
        _processoService = processoService;
    }

    [HttpGet]
    public IActionResult ListarProcesso([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        return Ok(_processoService.ListarProcessos(skip, take));
    }

    [HttpGet("{id:long}")]
    public IActionResult BuscarProcessoPorId(long id)
    {
        return Ok(_processoService.BuscarProcessoPorIdDto(id));
    }

    [HttpPost]
    public IActionResult CadastrarProcesso([FromBody] CreateProcessoDto createProcessoDto)
    {
        return Ok(_processoService.CadastrarProcesso(createProcessoDto));
    }

    [HttpPut("{id:long}")]
    public IActionResult AtualizarProcesso(long id, [FromBody] UpdateProcessoDto updateProcessoDto)
    {
        _processoService.AtualizarProcesso(id, updateProcessoDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletarProcesso(long id)
    {
        _processoService.DeletarProcesso(id);
        return NoContent();
    }
}
