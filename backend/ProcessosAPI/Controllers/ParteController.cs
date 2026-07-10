using Microsoft.AspNetCore.Mvc;
using ProcessosAPI.DTOs;
using ProcessosAPI.Services;

namespace ProcessosAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParteController : ControllerBase
{
    private readonly ParteService _parteService;

    public ParteController(ParteService parteService)
    {
        _parteService = parteService;
    }

    [HttpGet]
    public IActionResult ListarPartes([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        return Ok(_parteService.ListarPartes(skip, take));
    }

    [HttpGet("{id}")]
    public IActionResult BuscarPartePorId(long id)
    {
        return Ok(_parteService.BuscarPartePorIdDto(id));
    }

    [HttpPost]
    public IActionResult CadastrarParte([FromBody] CreateParteDto createParteDto)
    {
        return Created("", _parteService.CadastrarParte(createParteDto));
    }

    [HttpPut("{id}")]
    public IActionResult AtualizarParte(long id, [FromBody] UpdateParteDto updateParteDto)
    {
        _parteService.AtualizarParte(id, updateParteDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletarParte(long id)
    {
        _parteService.DeletarParte(id);
        return NoContent();
    }
}
