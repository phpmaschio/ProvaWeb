using System.ComponentModel.DataAnnotations;
using ProcessosAPI.Models;

namespace ProcessosAPI.DTOs;

public record CreateProcessoDto(
        [Required(ErrorMessage = "A descrição do processo é obrigatória")]
        [MaxLength(100,ErrorMessage = "A descrição do processo deve conter no máximo 100 caracteres")]
        string Descricao,
        [Required(ErrorMessage = "Código do status é obrigatório")]
        long StatusProcessoId,
        [Required(ErrorMessage = "Partes atribuidas ao processo são obrigatórias")]
        List<ReadParteDto> Partes,
        [Required(ErrorMessage = "Andamento atribuido ao processo é obrigatório")]
        CreateAndamentoDto Andamento
    );      