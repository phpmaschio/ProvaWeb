using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcessosAPI.Models;

public class Processo
{
    [Key]
    [Required]
    public long Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Descricao { get; set; }
    
    [Required]
    public StatusProcesso Status { get; set; }
    
    [Required]
    [Column(TypeName = "timestamptz")]
    public DateTime CriadoEm { get; set; }
    
    public DateTime? UltimaAlteracao { get; set; }
    
}