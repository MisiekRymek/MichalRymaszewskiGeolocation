using MichalRymaszewskiGeolocation.Models;
using System.Collections.Generic;

namespace MichalRymaszewskiGeolocation.Services
{
    public interface IDatabaseService
    {
        bool Insert(GeoLocation geo);
        bool Delete(string ipOrUrl);
        //IEnumerable<GeoLocation> GetAll();
        GeoLocation? Find(string ipOrUrl);
    }
}

