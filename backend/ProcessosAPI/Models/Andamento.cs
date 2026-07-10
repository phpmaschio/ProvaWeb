using System.ComponentModel.DataAnnotations;

namespace ProcessosAPI.Models;

public class Andamento
{
    [Key]
    [Required]
    public long Id { get; set; }

    [Required]
    [MaxLength(100)]   
    public string Descricao { get; set; }
    
    [Required]
    public Processo Processo { get; set; }
    
    [Required]
    public DateTime AtribuidoEm { get; set; }
    
}