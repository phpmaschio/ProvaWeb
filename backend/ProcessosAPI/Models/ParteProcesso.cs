using System.ComponentModel.DataAnnotations;

namespace ProcessosAPI.Models;

public class ParteProcesso
{
    [Key]
    [Required]
    public long Id { get; set; }
    [Required]
    public Parte Parte { get; set; }
    [Required]
    public Processo Processo { get; set; }

    public ParteProcesso() { }
    public ParteProcesso(Parte parte, Processo processo)
    {
        Parte = parte;
        Processo = processo;
    }
}