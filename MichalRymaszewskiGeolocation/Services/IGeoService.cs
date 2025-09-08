using System.Threading.Tasks;
using MichalRymaszewskiGeolocation.Models;

namespace MichalRymaszewskiGeolocation.Services
{
    public interface IGeoService
    {
        Task<Result<GeoLocation>> GetGeoLocationAsync(string ipOrUrl);
    }
}
