using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }

        public DbSet<Quote> Quotes { get; set; }

        public DbSet<Battle> Battles { get; set; }

        /// <summary>
        /// Constructor para WebAPI
        /// </summary>
        /// <param name="options"></param>
        public SamuraiContext(DbContextOptions<SamuraiContext> options)
            :base(options)
        {

        }

        //Comentamos el siguiente método para su uso a través de API
        //En el caso de tener que ser ejecutado en local, descomentaríamos para completar la configuración
        //En el caso de ASP.Net WebAPI, el servicio se encarga de configurar el DbContext
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=DESKTOP-GN8HL8E;Initial Catalog=SamuraiAppData;Integrated Security=True");

        //    //base.OnConfiguring(optionsBuilder); 
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Samurai>()
                .HasMany(s => s.Battles)
                .WithMany(b => b.Samurais)
                .UsingEntity<BattleSamurai>
                (bs => bs.HasOne<Battle>().WithMany(),
                 bs => bs.HasOne<Samurai>().WithMany())
                .Property(bs => bs.DateJoined)
                .HasDefaultValueSql("getdate()");
        }
    }
}
