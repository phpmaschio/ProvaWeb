using System.ComponentModel.DataAnnotations;

namespace ProcessosAPI.DTOs;

public record UpdateProcessoDto(
        [Required(ErrorMessage = "A descrição do processo é obrigatória")]
        [MaxLength(100,ErrorMessage = "A descrição do processo deve conter no máximo 100 caracteres")]
        string Descricao,
        [Required(ErrorMessage = "Código do status é obrigatório")]
        long StatusProcessoId
    );