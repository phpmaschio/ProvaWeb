using System.ComponentModel.DataAnnotations;

namespace ProcessosAPI.DTOs;

public record UpdateParteDto(
        [Required(ErrorMessage = "Descrição da parte é obrigatória")]
        [StringLength(100,ErrorMessage = "Descrição deve conter no máximo 100 caracteres")]
        string Nome,
        [Required(ErrorMessage = "Tipo da parte é obrigatório")]
        [MaxLength(50,ErrorMessage = "Tipo deve conter no máximo 50 caracteres")]
         string Tipo 
    );