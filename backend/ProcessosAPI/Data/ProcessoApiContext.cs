using Microsoft.EntityFrameworkCore;
using ProcessosAPI.Models;

namespace ProcessosAPI.Data;

public class ProcessoApiContext : DbContext
{
    public DbSet<StatusProcesso> StatusProcessos { get; set; }
    public DbSet<Processo> Processos { get; set; }
    public DbSet<Parte> Partes { get; set; }
    public DbSet<ParteProcesso> PartesProcessos { get; set; }
    public DbSet<Andamento> Andamentos { get; set; }
    
    public ProcessoApiContext(DbContextOptions<ProcessoApiContext> options) : base(options)
    {
        
    }
}