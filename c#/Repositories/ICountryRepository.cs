using Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Repositories
{
    public interface ICountryRepository
    {
        Task<ICollection<Country>> GetCountryPopulationsAsync();
    }
}
