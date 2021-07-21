using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }

        public DbSet<Quote> Quotes { get; set; }

        public DbSet<Battle> Battles { get; set; }

        private StreamWriter streamWriter;        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-GN8HL8E;Initial Catalog=SamuraiAppData;Integrated Security=True")
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name },
                LogLevel.Information);

            //streamWriter = new StreamWriter("EFCoreLog.txt", append: true);
            //optionsBuilder.UseSqlServer("Data Source=DESKTOP-GN8HL8E;Initial Catalog=SamuraiAppData;Integrated Security=True")
            //    .LogTo(streamWriter.WriteLine);

            //optionsBuilder.UseSqlServer("Data Source=DESKTOP-GN8HL8E;Initial Catalog=SamuraiAppData;Integrated Security=True")
            //    .LogTo(log => Debug.WriteLine(log));


        }

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
