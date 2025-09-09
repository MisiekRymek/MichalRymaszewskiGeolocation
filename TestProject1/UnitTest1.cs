using MichalRymaszewskiGeolocation.Models;
using MichalRymaszewskiGeolocation.Presenters;
using MichalRymaszewskiGeolocation.Services;
using MichalRymaszewskiGeolocation.Utils;
using MichalRymaszewskiGeolocation.Views;
using Microsoft.VisualBasic.ApplicationServices;
using System.Reflection.Metadata.Ecma335;

namespace TestProject1
{
    public class UnitTest1
    {
        private DummyUser _user;
        private IGeoService _geoService;
        private IDatabaseService _dbService;
        private GeoPresenter _presenter;

        [SetUp]
        public void Setup()
        {
            _user = new DummyUser();
            _geoService = new FakeGeoService();
            _dbService = new FakeDatabaseService();
            _presenter = new GeoPresenter(_user, _dbService, _geoService);
        }

        [Test]
        public void IPInputTest()
        {
            var user = new DummyUser();
            IDatabaseService dbService = new MySqlDatabaseService("localhost", "geodb", "root", "");
            IGeoService geoService = new IpStackGeoService(Config.IPstackApiKey);
            var presenter = new GeoPresenter(user, dbService, geoService);
            user.Save();
            Assert.AreEqual(expected: null, actual: user.IPaddress, message: "IPaddress field should be initialized as null");
            Assert.IsTrue(user.ShowError, "Null ip should trigger error");
            Assert.AreEqual(expected: "IP address or URL cannot be empty", actual: user.ErrorMessage, "Error message should indicate empty IP");

            //user.IPaddress = "192.168.1.1";
            //user.Save();
            //Assert.AreEqual(expected: "192.168.1.1", actual: user.IPaddress, message: "IPaddress field should be settable");
            //Assert.IsFalse(user.ShowError, "Valid ip should not trigger error");
            //Assert.AreEqual(expected:"", actual: user.ErrorMessage, "Error message should be empty for valid IP");

            user.IPaddress = "192.aaa.1.1";
            user.Save();
            Assert.IsTrue(user.ShowError, "Incorrect IP format should trigger error");
            Assert.AreEqual(expected: "Incorrect IP or URL format.", actual: user.ErrorMessage, "Error message for incorrect IP format should be displayed");
        }
    }

    class DummyUser : IGeoServiceInput
    {
        public string IPaddress { get; set; }
        public string ErrorMessage { get; set; }
        public bool chk_AutoSaveToDB { get; }
        public bool ShowError { get; set; }
        public GeoLocation LastLocation { get; private set; }

        public event EventHandler SubmitAttempted;
        public event EventHandler SaveAttempted;
        public event EventHandler ReloadAttempted;
        public event EventHandler RemoveAttempted;


        public void ShowGeoLocation(GeoLocation location)
        {
            LastLocation = location;
        }

        public void Save() => SubmitAttempted?.Invoke(this, EventArgs.Empty);

        public void ToggleRemoveButton(bool enable)
        {
            // not for testing
        }

        public void ToggleSaveButton(bool enable)
        {
            // not for testing
        }
    }

    class FakeDatabaseService : IDatabaseService
    {
        private readonly Dictionary<string, GeoLocation> _storage = new();
        public bool CheckDbStatus() => true;
        public bool Delete(string ipOrUrl) => _storage.Remove(ipOrUrl);
        public GeoLocation? Find(string ipOrUrl) => _storage.TryGetValue(ipOrUrl, out var geo) ? geo : null;
        public bool Insert(GeoLocation geo)
        {
            _storage[geo.IpOrUrl] = geo;
            return true;
        }
    }
    class FakeGeoService : IGeoService
    {
        public async Task<GeoLocation?> GetGeoLocationAsync(string ipOrUrl)
        {
            await Task.Delay(100); // Simulate async operation
            if (ipOrUrl == "192.168.1.1")
            {
                return new GeoLocation
                {
                    IpOrUrl = ipOrUrl,
                    Country = "TestCountry",
                    City = "TestCity",
                    Latitude = 12.34,
                    Longitude = 56.78,
                    RetrievedAt = DateTime.Now
                };
            }
            else
            {
                return null; // Simulate not found
            }
        }

        Task<Result<GeoLocation>> IGeoService.GetGeoLocationAsync(string ipOrUrl)
        {
            throw new NotImplementedException();
        }
    }
}