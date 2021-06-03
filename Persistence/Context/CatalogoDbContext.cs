using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context
{
    public class CatalogoDbContext : DbContext
    {
        public CatalogoDbContext(DbContextOptions<CatalogoDbContext> options) : base(options) { }

        public DbSet<Acomodacao> Acomodacoes { get; set; }

    }
}
