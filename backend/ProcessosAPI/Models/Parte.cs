using System.ComponentModel.DataAnnotations;

namespace ProcessosAPI.Models;

public class Parte
{
    [Key]
    [Required]
    public long Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Nome { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Tipo { get; set; }
    
    [Required]
    public DateTime CriadoEm { get; set; }
    
    public DateTime? UltimaAltercao { get; set; }
    
}