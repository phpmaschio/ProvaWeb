using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcessosAPI;

public class StatusProcesso
{
    [Key]
    [Required]
    public long Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Descricao { get; set; }
    
    [Required]
    [Column(TypeName = "timestamptz")]
    public DateTime CriadoEm { get; set; }
    
    [Column(TypeName = "timestamptz")]
    public DateTime? UltimaAlteracao { get; set; }
    
}