using Backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Repositories
{
    public class StatRepository : ICountryRepository
    {
        private readonly IStatService _statService;

        public StatRepository(IStatService statService)
        {
            _statService = statService;
        }

        public async Task<ICollection<Country>> GetCountryPopulationsAsync()
        {
            var countries = new List<Country>();
            List<Tuple<string, int>> statCountries = await _statService.GetCountryPopulationsAsync();
            statCountries.ForEach(c => countries.Add(new Country
            {
                CountryName = c.Item1,
                Population = c.Item2
            }));
            return countries;    
        }
    }
}
