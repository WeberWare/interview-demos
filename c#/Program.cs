using Backend.Models;
using Backend.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Backend.Domain;
using Backend.Tests;

namespace Backend
{
    public class Program
    {
        private static ICountryRepository _dbRepository;
        private static ICountryRepository _statRepository;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Started");
            Console.WriteLine("Getting DB Connection...");
            IDbManager db = new SqliteDbManager();
            DbConnection conn = db.getConnection();
            if (conn == null)
            {
                Console.WriteLine("Failed to get connection");
            }
            else
            {
                InitRepositories(conn);
                var manager = new CountryPopulationsManager(_dbRepository, _statRepository);
                var countryPopulations = await manager.GetCountryPopulations();
                OutputCountryPopulations(countryPopulations);
            }

            // Uncomment to run tests.
            //new GivenEmptyRepositories_WhenGetCountryPopulations_ThenEmptyList().Execute();
            //new GivenActualData_WhenGetCountryPopulations_ThenValidList().Execute();

            Console.WriteLine("Completed");
        }

        private static void InitRepositories(DbConnection conn)
        {
            _dbRepository = new DbRepository(conn);
            _statRepository = new StatRepository(new ConcreteStatService());
        }

        private static void OutputCountryPopulations(ICollection<Country> countryPopulations)
        {
            Console.WriteLine($"Number of Countries: {countryPopulations.Count}");
            foreach (var country in countryPopulations)
            {
                Console.WriteLine($"{country.CountryName} : {country.Population}");
            }
        }
    }
}
