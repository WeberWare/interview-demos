using Backend.Models;
using Backend.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

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
                var countryPopulations = await GetCountryPopulations(_dbRepository, _statRepository);
                OutputCountryPopulations(countryPopulations);
            }

            // Uncomment to run tests.
            // new GivenEmptyRepositories_WhenGetCountryPopulations_ThenEmptyList().Execute();
            // new GivenActualData_WhenGetCountryPopulations_ThenValidateList().Execute();

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

        public async static Task<ICollection<Country>> GetCountryPopulations(ICountryRepository dbRepository, ICountryRepository statRepository)
        {
            Task<ICollection<Country>> dbCountriesTask = dbRepository.GetCountryPopulationsAsync();
            Task<ICollection<Country>> statCountriesTask = statRepository.GetCountryPopulationsAsync();
            await Task.WhenAll(dbCountriesTask, statCountriesTask);
            return CombineLists(dbCountriesTask.Result, statCountriesTask.Result);
            //var countryPopulations = CombineLists(dbCountriesTask.Result, statCountriesTask.Result);
            //return countryPopulations;
        }

        private static ICollection<Country> CombineLists(ICollection<Country> dbList, ICollection<Country> statList)
        {
            var combinedLists = (from stc in statList
                                      where !(from dbc in dbList select dbc.CountryName).Contains(stc.CountryName)              
                                      select stc)
                                      .Concat(dbList).OrderBy(c => c.CountryName).ToList();
            return combinedLists;
        }
    }
}
