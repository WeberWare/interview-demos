using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Tests
{
    public class GivenEmptyRepositories_WhenGetCountryPopulations_ThenEmptyList
    {
        private class EmptyRepository : ICountryRepository
        {
            public async Task<ICollection<Country>> GetCountryPopulationsAsync()
            {
                var countries = new List<Country>();
                await Task.Run(() => {});
                return countries;
            }
        }
        public void Execute()
        {
            var dbRepository = new EmptyRepository();
            var statRepository = new EmptyRepository();
            var countryPopulations = Program.GetCountryPopulations(dbRepository, statRepository).Result;
            Validate(countryPopulations);
        }

        private void Validate(ICollection<Country> countries)
        {
            if (countries.Count != 0)
            {
                throw new Exception("GivenEmptyRepositories_WhenGetCountryPopulations_ThenEmptyList Failed != 0");
            }
        }
    }
}
