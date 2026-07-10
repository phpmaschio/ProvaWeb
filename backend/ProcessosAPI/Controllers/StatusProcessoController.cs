using Microsoft.AspNetCore.Mvc;
using ProcessosAPI.DTOs;
using ProcessosAPI.Services;

namespace ProcessosAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusProcessoController : ControllerBase
{
    private readonly StatusProcessoService _statusProcessoService;

    public StatusProcessoController(StatusProcessoService statusProcessoService)
    {
        _statusProcessoService = statusProcessoService;
    }

    [HttpGet]
    public IActionResult RetornaTodosStatus([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        return Ok(_statusProcessoService.ListarStatus(skip, take));
    }

    [HttpGet("{id:long}")]
    public IActionResult RetornaStatusPorId(long id)
    {
        return Ok(_statusProcessoService.RetornaStatusPorIdDto(id));
    }

    [HttpPost]
    public CreatedAtActionResult CadastraStatusProcesso([FromBody] CreateStatusProcessoDto createStatusProcessoDto)
    {
        var statusProcesso = _statusProcessoService.CadastrarStatusProcesso(createStatusProcessoDto);

        return CreatedAtAction(
            nameof(RetornaStatusPorId),
            new { id = statusProcesso.Id },
            statusProcesso);
    }

    [HttpPut("{id:long}")]
    public IActionResult AtualizaStatusProcesso([FromBody] UpdateStatusProcessoDto updateStatusProcessoDto, long id)
    {
        _statusProcessoService.AtualizarStatusProcesso(id, updateStatusProcessoDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult ExcluirStatusProcesso(long id)
    {
        _statusProcessoService.ExcluirStatusProcesso(id);
        return NoContent();
    }
}
