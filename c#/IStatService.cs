using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public interface IStatService
    {
        List<Tuple<string, int>> GetCountryPopulations();
        Task<List<Tuple<string, int>>> GetCountryPopulationsAsync();
    }
}
