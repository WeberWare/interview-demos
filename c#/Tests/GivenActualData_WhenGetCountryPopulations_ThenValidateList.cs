using System;
using System.Collections.Generic;
using System.Data.Common;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Tests
{
    public class GivenActualData_WhenGetCountryPopulations_ThenValidateList
    {
        public void Execute()
        {
            IDbManager db = new SqliteDbManager();
            DbConnection conn = db.getConnection();
            var dbRepository = new DbRepository(conn);
            var statRepository = new StatRepository(new ConcreteStatService());
            var countryPopulations =  Program.GetCountryPopulations(dbRepository, statRepository).Result;
            Validate(countryPopulations);
        }

        private void Validate(ICollection<Country> countries)
        {
            // NOTE: There is one issue with U.S.A and United States of America.  So test fails.
            if(countries.Count != 39)
            {
                throw new Exception("GivenActualData_WhenGetCountryPopulations_ThenValidateList Failed != 39");
            }
            // ETC... Validate
        }
    }
}
