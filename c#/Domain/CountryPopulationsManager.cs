using Backend.Models;
using Backend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain
{
    public class CountryPopulationsManager
    {
        private readonly ICountryRepository _dbRepository;
        private readonly ICountryRepository _statRepository;
        // NOTE: In real world, a service or database would provide the aliases.
        private readonly Dictionary<string, string> _aliases = new Dictionary<string, string>
            {
                { "U.S.A.", "United States of America" }
            };

        public CountryPopulationsManager(ICountryRepository dbRepository, ICountryRepository statRepository)
        {
            _dbRepository = dbRepository;
            _statRepository = statRepository;
        }

        public async Task<ICollection<Country>> GetCountryPopulations()
        {
            Task<ICollection<Country>> dbCountriesTask = _dbRepository.GetCountryPopulationsAsync();
            Task<ICollection<Country>> statCountriesTask = _statRepository.GetCountryPopulationsAsync();
            await Task.WhenAll(dbCountriesTask, statCountriesTask);
            return CombineLists(dbCountriesTask.Result, statCountriesTask.Result);
        }

        private ICollection<Country> CombineLists(ICollection<Country> dbList, ICollection<Country> statList)
        {
            var combinedLists = (from stc in statList
                                 where !(from dbc in dbList select GetAlias(dbc.CountryName)).Contains(stc.CountryName)
                                 select stc)
                                 .Concat(dbList).OrderBy(c => c.CountryName).ToList();
            return combinedLists;
        }

        private string GetAlias(string source)
        {
            if(_aliases.Keys.Contains(source))
            {
                return _aliases[source];
            }
            return source;
        }
    }
}
