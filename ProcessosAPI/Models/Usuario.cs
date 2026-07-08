using System.ComponentModel.DataAnnotations;

namespace ProcessosAPI;

public class Usuario
{  
    [Key]
    [Required]
    public long Id { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string Login { get; set; }
}