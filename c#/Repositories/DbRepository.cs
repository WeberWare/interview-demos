using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Common;
using Backend.Models;

namespace Backend.Repositories
{
    public class DbRepository : ICountryRepository
    {
        private readonly DbConnection _dbConnection;

        public DbRepository(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<ICollection<Country>> GetCountryPopulationsAsync()
        {
            if(_dbConnection == null)
            {
                Console.WriteLine("Invalid DbConnection in DbRepository.");
                throw new Exception("Invalid DbConnection in DbRepository.");
            }
            DbDataReader reader = await GetPopulationsFromDatabaseAsync();
            return await GetCountriesFromDataReader(reader);
        }

        private async Task<DbDataReader> GetPopulationsFromDatabaseAsync()
        {
            DbCommand command = GetCommand();
            return await command.ExecuteReaderAsync();
        }

        private async Task<ICollection<Country>> GetCountriesFromDataReader(DbDataReader reader)
        {
            List<Country> countries = new List<Country>();
            while (reader.Read())
            {
                countries.Add(await GetCountryFromDataReader(reader));
            }
            return countries;
        }

        private async Task<Country> GetCountryFromDataReader(DbDataReader reader)
        {
            Task<string> countryTask = reader.GetFieldValueAsync<string>(0);
            Task<double> populationTask = reader.GetFieldValueAsync<double>(1);
            await Task.WhenAll(countryTask, populationTask);
            return new Country
            {
                CountryName = countryTask.Result,
                Population = populationTask.Result
            };
        }

        private DbCommand GetCommand()
        {
            DbCommand command = _dbConnection.CreateCommand();
            if(command == null)
            {
                Console.WriteLine("Could not Create Command.");
                throw new Exception("Could not Create Command.");
            }
            command.CommandText = @"select country.CountryName, cast(ifnull(sum(city.Population),0) as real)
                                    from Country country 
                                    left join State state on country.CountryId=state.CountryId
                                    left join City city on state.StateId=city.StateId
                                    group by country.CountryId
                                    order by country.CountryName";
            return command;
        }
    }
}
