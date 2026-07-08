using System.ComponentModel.DataAnnotations;

namespace ProcessosAPI.DTOs;

public record ReadStatusProcessoDto(

    [Required(ErrorMessage = "O Id do status é obrigatória")]
    long Id,
    [Required(ErrorMessage = "A descrição do status é obrigatória")]
    [StringLength(50, ErrorMessage = "A descrição do status deve conter até 50 caracteres")]
    string Descricao
);
