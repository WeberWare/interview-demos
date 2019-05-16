using System;
using System.Collections.Generic;
using System.Data.Common;
using Backend.Domain;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Tests
{
    public class GivenActualData_WhenGetCountryPopulations_ThenValidList
    {
        public void Execute()
        {
            IDbManager db = new SqliteDbManager();
            DbConnection conn = db.getConnection();
            var dbRepository = new DbRepository(conn);
            var statRepository = new StatRepository(new ConcreteStatService());
            var manager = new CountryPopulationsManager(dbRepository, statRepository);
            var countryPopulations = manager.GetCountryPopulations().Result;
            Validate(countryPopulations);
        }

        private void Validate(ICollection<Country> countries)
        {
            if(countries.Count != 39)
            {
                throw new Exception("GivenActualData_WhenGetCountryPopulations_ThenValidList Failed != 39");
            }
            // ETC... Validate
        }
    }
}
