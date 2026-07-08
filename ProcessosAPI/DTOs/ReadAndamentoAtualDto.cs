using System.ComponentModel.DataAnnotations;

namespace ProcessosAPI.DTOs;

public record ReadAndamentoAtualDto(
        [Required(ErrorMessage = "Id do andamento é obrigatorio")] 
        long Id,
        [Required(ErrorMessage = "Descrição do andamento é obrigatoria")]  
        [StringLength(100,ErrorMessage = "A descrição deve conter no máximo 100 caracteres")] 
        string Descricao
    );