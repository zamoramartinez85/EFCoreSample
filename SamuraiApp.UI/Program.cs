using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContext _samuraiContext = new();
        static void Main(string[] args)
        {
            _samuraiContext.Database.EnsureCreated();
            GetSamurais("Before add:");
            AddSamurai();
            GetSamurais("After Add:");

            Console.WriteLine("Press any key....");
            Console.ReadKey();
        }

        private static void AddSamurai()
        {
            Samurai samurai = new() { Name = "Prueba" };
            _samuraiContext.Samurais.Add(samurai);
            _samuraiContext.SaveChanges();
        }

        private static void GetSamurais(string text)
        {
            List<Samurai> samurais = _samuraiContext.Samurais.ToList();
            Console.WriteLine($"{text} Samurai count in {samurais.Count}");

            foreach(Samurai samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }
    }
}
