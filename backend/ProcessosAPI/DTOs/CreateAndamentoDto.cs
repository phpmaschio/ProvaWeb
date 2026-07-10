using System.ComponentModel.DataAnnotations;

namespace ProcessosAPI.DTOs;

public record CreateAndamentoDto(
        [Required(ErrorMessage = "Descrição do andamento é obrigatoria")]  
        [StringLength(100,ErrorMessage = "A descrição deve conter no máximo 100 caracteres")] 
        string Descricao
    );