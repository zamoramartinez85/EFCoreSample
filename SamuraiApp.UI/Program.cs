using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SamuraiApp.UI
{
    // CONSIDERACIONES:
    // En código para producción hay que controlar las Excepciones.
    // Para este proyecto de prueba de EF Core no lo voy a tener en cuenta.
    class Program
    {
        private static SamuraiContext _samuraiContext = new();
        static void Main(string[] args)
        {
            _samuraiContext.Database.EnsureCreated();

            //List<string> samuraiNames = CreateSamuraisList();

            //AddSamurai();
            //AddSamuraisByName(samuraiNames);
            //GetSamurais(string.Empty);
            //AddVariousTypes();
            //QueryFilters();
            //QueryAggregates();
            //RetrieveAndUpdateSamurai();
            //RetrieveAndUpdateMultipleSamurais();
            //QueryAndUpdateBattles_Disconnected();

            //Objetos relacionados
            //InsertNewSamuraiWithQuote();
            //InsertNewSamuraiWithMultipleQuotes();
            //AddQuoteToExistingSamuraiWhileTracking();
            //AddQuoteToExistingSamuraiNotTracked(4);
            //Simpler_AddQuoteToExistingSamuraiNotTracked(4);
            //EagerLoadSamuraiWithQuotes();
            //ProjectSomeProperties();
            //FilteringWithRelatedData();
            //ModifyingRelatedDataWhenNotTracked();

            //Trabajando con relaciones n-m
            //AddingNewSamuraiToAnExistingBattle();
            //ReturnBattleWithSamurais();
            //ReturnAllBattlesWithSamurais();
            //AddAllSamuraisToAllBattles();
            //RemoveSamuraiFromBattle();
            //WillNotRemoveSamuraiBattleRelation();

            //Deleting M2M relationship using explicit M2M Mapping
            //RemoveSamuraiFromBattleExplicit();

            //Trabajando con relaciones 1-1
            //AddNewSamuraiWithHorse();
            //AddNewHorseToDisconnectedSamuraiObject();
            //ReplaceAHorse();
            //GetSamuraiWithHorse();
            GetHorsesWithSamurai();

            Console.WriteLine("Press any key....");
            Console.ReadKey();
        }

        private static void GetHorsesWithSamurai()
        {
            Horse horseOnly = _samuraiContext.Set<Horse>().Find(1);

            Samurai samuraiWithHorse = _samuraiContext.Samurais.Include(s => s.Horse)
                                                            .FirstOrDefault(s => s.Horse.HorseId == 1);

            var horseSamuraiPairs = _samuraiContext.Samurais
                .Where(s => s.Horse != null)
                .Select(s => new { Horse = s.Horse, Samurai = s })
                .ToList();
        }

        private static void GetSamuraiWithHorse()
        {
            List<Samurai> samurais = _samuraiContext.Samurais.Include(s => s.Horse).ToList();
        }

        private static void ReplaceAHorse()
        {
            Samurai samurai = _samuraiContext.Samurais.Include(s => s.Horse).FirstOrDefault(s => s.SamuraiId == 5);
            samurai.Horse = new Horse() { Name = "Nuevo Caballo" };
            _samuraiContext.SaveChanges();
        }

        private static void AddNewHorseToDisconnectedSamuraiObject()
        {
            Samurai samurai = _samuraiContext.Samurais.FirstOrDefault(s => s.SamuraiId == 5);
            samurai.Horse = new Horse() { Name = "Rocinante" };

            using (SamuraiContext samuraiContext = new SamuraiContext())
            {
                samuraiContext.Samurais.Attach(samurai);
                samuraiContext.SaveChanges();
            }
        }

        private static void AddNewSamuraiWithHorse()
        {
            Samurai samurai = new Samurai() 
            { 
                Name = "Jina Ujichika" ,
                Horse = new Horse()
                {
                    Name = "El cabajo de Jina"
                }
            };

            _samuraiContext.Samurais.Add(samurai);
            _samuraiContext.SaveChanges();
            
        }

        private static void RemoveSamuraiFromBattleExplicit()
        {
            BattleSamurai battleSamurai = _samuraiContext.Set<BattleSamurai>()
                .SingleOrDefault(bs => bs.BattleId == 1 && bs.SamuraiId == 10);

            if (battleSamurai != null)
            {
                _samuraiContext.Set<BattleSamurai>().Remove(battleSamurai);
                //_samuraiContext.Remove(battleSamurai);
                _samuraiContext.SaveChanges();
            }
        }

        private static void WillNotRemoveSamuraiBattleRelation()
        {
            Battle battle = _samuraiContext.Battles.FirstOrDefault(b => b.BattleId == 1);
            Samurai samurai = _samuraiContext.Samurais.FirstOrDefault();
            battle.Samurais.Remove(samurai);
            
            _samuraiContext.SaveChanges();  //En este caso, la relación no está recogida por EF
        }

        private static void RemoveSamuraiFromBattle()
        {
            Battle battleWithSamurai = _samuraiContext.Battles.Where(b => b.BattleId == 1)
                                                                .Include(b => b.Samurais).FirstOrDefault();
            Samurai samurai = battleWithSamurai.Samurais[0];
            battleWithSamurai.Samurais.Remove(samurai);
            _samuraiContext.SaveChanges();
        }

        private static void AddAllSamuraisToAllBattles()
        {
            List<Samurai> samurais = _samuraiContext.Samurais.ToList();
            List<Battle> battles = _samuraiContext.Battles.Include(b => b.Samurais).ToList();

            battles.ForEach(b =>
            {
                b.Samurais.AddRange(samurais);
            });

            _samuraiContext.SaveChanges();
        }

        private static void ReturnAllBattlesWithSamurais()
        {
            List<Battle> battles = _samuraiContext.Battles.Include(b => b.Samurais).ToList();
        }

        private static void ReturnBattleWithSamurais()
        {
            Battle battle = _samuraiContext.Battles.Include(b => b.Samurais).First();
        }

        private static void AddingNewSamuraiToAnExistingBattle()
        {
            Battle battle = _samuraiContext.Battles.First();
            battle.Samurais.Add(new Samurai() { Name = "Nuevo Samurai" });
            _samuraiContext.SaveChanges();
        }

        private static void ModifyingRelatedDataWhenNotTracked()
        {
            Samurai samurai = _samuraiContext.Samurais.Include(s => s.Quotes).FirstOrDefault(s => s.SamuraiId == 3);
            Quote quote = samurai.Quotes[0];
            quote.Text += "Texto agregado";

            using (SamuraiContext newContext = new SamuraiContext())
            {
                newContext.Entry(quote).State = EntityState.Modified;
                newContext.SaveChanges();
            }
        }

        private static void FilteringWithRelatedData()
        {
            List<Samurai> samurai = _samuraiContext.Samurais
                                        .Where(s => s.Quotes.Any(q => q.Text.Contains("Hola"))).ToList();

        }

        private static void ProjectSomeProperties()
        {
            var someProperties = _samuraiContext.Samurais.Select(s => new { s.SamuraiId, s.Name }).ToList();
            var someProperties2 = _samuraiContext.Samurais.Select(s => new { s.SamuraiId, s.Name, s.Quotes }).ToList();
            var someProperties3 = _samuraiContext.Samurais.Select(s => new { s.SamuraiId, s.Name, NumberOfQuotes = s.Quotes.Count }).ToList();
            var someProperties4 = _samuraiContext.Samurais.Select(s => new { s.SamuraiId, s.Name, HappyQuotes = s.Quotes.Where(q=>q.Text.Contains("happy")) }).ToList();
        }

        /// <summary>
        /// Métodos que recargan los objetos junto a sus hijos
        /// Podemos traer tus nietos usando ThenInclude
        /// </summary>
        private static void EagerLoadSamuraiWithQuotes()
        {
            Samurai samurai = _samuraiContext.Samurais.Where(s => s.SamuraiId == 3).Include(s => s.Quotes).First();

            Samurai samurai2 = _samuraiContext.Samurais.Where(s => s.SamuraiId == 3).AsSplitQuery().Include(s => s.Quotes).First();
        }

        /// <summary>
        /// En este método se realiza el tracking a través de la especificación del samurai
        /// mediante su id
        /// </summary>
        /// <param name="samuraiId">Id del samurai</param>
        private static void Simpler_AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            Quote quote = new Quote() { Text = "Nueva cita", SamuraiId = samuraiId };
            using (var newContext = new SamuraiContext())
            {
                newContext.Quotes.Add(quote);
                newContext.SaveChanges();
            }
        }

        /// <summary>
        /// Método donde agregamos una entidad al contexto para guardar los cambios
        /// </summary>
        /// <param name="samuraiId">Id del samurai</param>
        private static void AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            Samurai samurai = _samuraiContext.Samurais.Find(samuraiId);
            samurai.Quotes.Add(new Quote() { Text = "Ahora somos amigos" });

            using (var newContext = new SamuraiContext())
            {
                newContext.Samurais.Attach(samurai);
                newContext.SaveChanges();
            }
        }

        private static void AddQuoteToExistingSamuraiWhileTracking()
        {
            Samurai samurai = _samuraiContext.Samurais.First();
            samurai.Quotes.Add(new Quote() { Text = "Soy un buen samurai" });

            _samuraiContext.SaveChanges();
        }

        private static void InsertNewSamuraiWithMultipleQuotes()
        {
            Samurai samurai = new Samurai()
            {
                Name = "El samurai",
                Quotes = new List<Quote>()
                {
                    new Quote() {Text = "Hola, qué pasa?"},
                    new Quote() {Text = "Hoy hace calor"}
                }
            };
            _samuraiContext.Samurais.Add(samurai);
            _samuraiContext.SaveChanges();
        }

        private static void InsertNewSamuraiWithQuote()
        {
            Samurai samurai = new Samurai()
            {
                Name = "Kobei Shimada",
                Quotes = new List<Quote>()
                {
                    new Quote() {Text = "Hola, qué tal?"}
                }
            };
            _samuraiContext.Samurais.Add(samurai);
            _samuraiContext.SaveChanges();
        }

        /// <summary>
        /// En este método veremos como trazar elementos en distintos contextos
        /// </summary>
        private static void QueryAndUpdateBattles_Disconnected()
        {
            List<Battle> disconnectedBattles;
            
            using (var context1 = new SamuraiContext())
            {
                disconnectedBattles = _samuraiContext.Battles.ToList();
            }
            //El primer contexto se libera

            disconnectedBattles.ForEach(b =>
            {
                b.StartDate = new DateTime(1570, 1, 1);
                b.EndDate = new DateTime(1570, 12, 1);
            });

            using (var context2 = new SamuraiContext())
            {
                //Este contexto no tiene información de traza
                context2.UpdateRange(disconnectedBattles);
                context2.SaveChanges();
            }
        }

        private static void RetrieveAndUpdateMultipleSamurais()
        {
            List<Samurai> samurais = _samuraiContext.Samurais.Skip(1).Take(4).ToList();
            samurais.ForEach(s => s.Name += "San");
            _samuraiContext.SaveChanges();
        }

        private static void RetrieveAndUpdateSamurai()
        {
            Samurai samurai = _samuraiContext.Samurais.FirstOrDefault();
            samurai.Name += "San";
            _samuraiContext.SaveChanges();
        }

        private static void QueryAggregates()
        {
            string name = "Samurai 1";
            Samurai samurai = _samuraiContext.Samurais.FirstOrDefault(s => s.Name == name);

            //Es un método directo de DbSet. Si está cargado, no realiza peticiones
            Samurai samurai1 = _samuraiContext.Samurais.Find(2);
        }

        private static void QueryFilters()
        {
            List<Samurai> samurais = _samuraiContext.Samurais.Where(s => s.Name == "Samurai 1").ToList();
        }

        private static void AddVariousTypes()
        {
            //Podemos añadir varias entidades de ambas formas:
            _samuraiContext.AddRange(
                new Samurai() { Name = "Samurai 1" },
                new Samurai() { Name = "Samurai 2" },
                new Battle() { Name = "Batalla de Anegawa" },
                new Battle() { Name = "Batalla de Nagashino" });

            //_samuraiContext.Samurais.AddRange(
            //    new Samurai() { Name = "Samurai 1" },
            //    new Samurai() { Name = "Samurai 2" });

            //_samuraiContext.Battles.AddRange(
            //    new Battle() { Name = "Batalla de Anegawa" },
            //    new Battle() { Name = "Batalla de Nagashino" });

            _samuraiContext.SaveChanges();
        }

        private static void AddSamuraisByName(List<string> samuraiNames)
        {
            samuraiNames.ForEach(s => _samuraiContext.Add(new Samurai() { Name = s }));

            _samuraiContext.SaveChanges();
        }

        private static List<string> CreateSamuraisList()
        {
            return new List<string>() { "Samurai Manué", "Samurai Pepe", "Samurai Paco" };
        }

        private static void AddSamurai()
        {
            Samurai samurai = new() { Name = "Prueba" };
            _samuraiContext.Samurais.Add(samurai);
            _samuraiContext.SaveChanges();
        }

        private static void GetSamurais(string text)
        {
            List<Samurai> samurais = _samuraiContext.Samurais
                .TagWith("Añadimos comentarios para profiler")
                .ToList();
            Console.WriteLine($"{text} Samurai count in {samurais.Count}");

            foreach (Samurai samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }
    }
}
