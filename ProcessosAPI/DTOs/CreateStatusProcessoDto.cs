using System.ComponentModel.DataAnnotations;

namespace ProcessosAPI.DTOs;

public record CreateStatusProcessoDto (
        [Required(ErrorMessage = "A descrição do status é obrigatória")]
        [StringLength(50, ErrorMessage = "A descrição do status deve conter até 50 caracteres")]
         string Descricao
    ); 