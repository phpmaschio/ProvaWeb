using System.ComponentModel.DataAnnotations;

namespace ProcessosAPI.DTOs;

public record CreateParteDto(
        [Required(ErrorMessage = "Nome da parte é obrigatório")]
        [StringLength(100,ErrorMessage = "Nome deve conter no máximo 100 caracteres")]
        string Nome,
        [Required(ErrorMessage = "Tipo da parte é obrigatório")]
        [MaxLength(50,ErrorMessage = "Tipo deve conter no máximo 50 caracteres")]
         string Tipo 
    );