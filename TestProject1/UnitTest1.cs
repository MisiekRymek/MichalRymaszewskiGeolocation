using MichalRymaszewskiGeolocation.Models;
using MichalRymaszewskiGeolocation.Presenters;
using MichalRymaszewskiGeolocation.Services;
using MichalRymaszewskiGeolocation.Utils;
using MichalRymaszewskiGeolocation.Views;
using Microsoft.VisualBasic.ApplicationServices;
using System.Reflection.Metadata.Ecma335;
using System.Windows.Forms;

namespace TestProject1
{
    public class UnitTest1
    {
        private DummyUser _user;
        private IGeoService _geoService;
        private IDatabaseService _dbService;
        private GeoPresenter _presenter;
        private IGeoServiceInput _view;

        [SetUp]
        public void Setup()
        {
            _user = new DummyUser();
            _dbService = new FakeDatabaseService();
            _presenter = new GeoPresenter(_user, _dbService, _geoService);
        }

        [Test]
        public void IPInputTest()
        {
            var user = new DummyUser();
            IDatabaseService dbService = new MySqlDatabaseService(Config.dbAddress, Config.dbName, Config.dbUser, Config.dbPassword);
            IGeoService geoService = new IpStackGeoService(Config.IPstackApiKey);
            var presenter = new GeoPresenter(user, dbService, geoService);
            // empty input
            user.Submit();
            Assert.AreEqual(expected: null, actual: user.IPaddress, message: "IPaddress field should be initialized as null");
            Assert.IsTrue(user.ShowError, "Null ip should trigger error");
            Assert.AreEqual(expected: "IP address or URL cannot be empty", actual: user.ErrorMessage, "Error message should indicate empty IP");

            // incorrect input ip
            user.IPaddress = "192.aaa.1.1";
            user.Submit();
            Assert.IsTrue(user.ShowError, "Incorrect IP format should trigger error");
            Assert.AreEqual(expected: "Incorrect IP or URL format.", actual: user.ErrorMessage, "Error message for incorrect IP format should be displayed");

            // incorrect input url
            user.IPaddress = "htp://www.example.com";
            user.Submit();
            Assert.IsTrue(user.ShowError, "Incorrect IP format should trigger error");
            Assert.AreEqual(expected: "Incorrect IP or URL format.", actual: user.ErrorMessage, "Error message for incorrect IP format should be displayed");

            // incorrect input url v2
            user.IPaddress = "http//example";
            user.Submit();
            Assert.IsTrue(user.ShowError, "Incorrect IP format should trigger error");
            Assert.AreEqual(expected: "Incorrect IP or URL format.", actual: user.ErrorMessage, "Error message for incorrect IP format should be displayed");
        }

        [Test]
        public async Task ReloadSaveRemoveButtonTest()
        {
            var user = new DummyUser();
            IDatabaseService dbService = new MySqlDatabaseService("localhost", "geodb", "root", "");
            IGeoService geoService = new IpStackGeoService(Config.IPstackApiKey);
            var presenter = new GeoPresenter(user, dbService, geoService);

            // correct input ip - download from db or from api
            user.IPaddress = "86.49.201.207";
            user.Submit();
            await Task.Delay(5000);
            Assert.AreEqual(expected: "86.49.201.207", actual: user.IPaddress, message: "IPaddress field should be settable");
            Assert.IsFalse(user.ShowError, "Valid ip should not trigger error");
            Assert.AreEqual(expected: "", actual: user.ErrorMessage, "Error message should be empty for valid IP");
            Assert.AreEqual(expected: "86.49.201.207", actual: user.GetListGeoLocation().IpOrUrl, "Result should be displayed in the list");
            // get the time for comparison after reload
            DateTime NotReloadedTime = user.GetListGeoLocation().RetrievedAt;

            user.Reload();
            await Task.Delay(5000);
            DateTime ReloadedTime = user.GetListGeoLocation().RetrievedAt;
            // after reload, time should be different, as it is reloaded from api
            Assert.That(actual: ReloadedTime, Is.GreaterThan(expected: NotReloadedTime), "Time after reload should be different");

            // save to db
            user.Save();
            await Task.Delay(5000);
            // submit different ip and go back to the previous one from db - if saved correctly, time should be the same as before (ReloadedTime)
            user.IPaddress = "86.49.201.208";
            user.Submit();
            await Task.Delay(5000);
            user.IPaddress = "86.49.201.207";
            user.Submit();
            await Task.Delay(5000);
            Assert.That(actual: user.GetListGeoLocation().RetrievedAt, Is.EqualTo(expected: ReloadedTime), "Time after reloading from db should be the same as before");

            // remove from db
            user.Remove();
            await Task.Delay(5000);
            user.IPaddress = "86.49.201.208";
            user.Submit();
            await Task.Delay(5000);
            // submit the same ip again - if removed correctly, time should be different, as it is reloaded from api
            user.IPaddress = "86.49.201.207";
            user.Submit();
            await Task.Delay(5000);
            Assert.That(actual: user.GetListGeoLocation().RetrievedAt, Is.GreaterThan(expected: ReloadedTime), "Time after removing from db and reloading from api should be different");
        }

        [Test]
        public async Task DisconnectApiTest()
        {
            var user = new DummyUser();
            IDatabaseService dbService = new MySqlDatabaseService("localhost", "geodb", "root", "");
            //wrong api key to simulate disconnection
            string IPstackApiKey = "d9cad46e8a4726f6e4f0e4e2d7fc10588";
            IGeoService geoService = new IpStackGeoService(IPstackApiKey);
            var presenter = new GeoPresenter(user, dbService, geoService);
            // correct input ip which is not in db - should try to connect to api and fail
            user.IPaddress = "86.49.203.222";
            user.Submit();
            await Task.Delay(5000);
            Assert.IsTrue(user.ShowError, "Incorrect API key format should trigger error");
            Assert.AreEqual(expected: "Problem getting geolocation from IPStack", actual: user.ErrorMessage, "Error message should indicate problem with API connection");
        }

        [Test]
        public async Task DisconnectDbTest()
        {
            var user = new DummyUser();
            //wrong db password to simulate disconnection
            IDatabaseService dbService = new MySqlDatabaseService("localhost", "geodb", "root", "1234");
            IGeoService geoService = new IpStackGeoService(Config.IPstackApiKey);
            var presenter = new GeoPresenter(user, dbService, geoService);
            Assert.IsTrue(user.ShowError, "Incorrect DB connection should trigger error");
            Assert.AreEqual(expected: "Database not avaliable", actual: user.ErrorMessage, "Error message should indicate problem with DB connection");
            user.IPaddress = "86.49.203.222";
            user.Submit();
            await Task.Delay(5000);
            user.Save();
            await Task.Delay(5000);
            Assert.IsTrue(user.ShowError, "Incorrect DB connection should trigger error");
            Assert.AreEqual(expected: "problem saving to database", actual: user.ErrorMessage, "Error message should indicate problem with DB connection");
            user.Remove();
            await Task.Delay(5000);
            Assert.IsTrue(user.ShowError, "Incorrect DB connection should trigger error");
            Assert.AreEqual(expected: "problem removing from database", actual: user.ErrorMessage, "Error message should indicate problem with DB connection");
        }

        class DummyUser : IGeoServiceInput
        {
            public string IPaddress { get; set; }
            public string ErrorMessage { get; set; }
            public bool chk_AutoSaveToDB { get; }
            public bool ShowError { get; set; }
            public GeoLocation LastLocation { get; private set; }



            //public ListView lv_geo_result { get; }
            public ListView lv_geo_result { get; private set; }

            public event EventHandler SubmitAttempted;
            public event EventHandler SaveAttempted;
            public event EventHandler ReloadAttempted;
            public event EventHandler RemoveAttempted;

            public DummyUser()
            {
                lv_geo_result = new ListView();
                lv_geo_result.View = View.Details;
                lv_geo_result.FullRowSelect = true;

                lv_geo_result.Columns.Add("IP / URL", 120);
                lv_geo_result.Columns.Add("Country", 100);
                lv_geo_result.Columns.Add("City", 100);
                lv_geo_result.Columns.Add("Latitude", 80);
                lv_geo_result.Columns.Add("Longitude", 80);
                lv_geo_result.Columns.Add("Retrieved At", 150);
            }
            public void ShowGeoLocation(GeoLocation location)
            {
                lv_geo_result.Items.Clear();
                var item = new ListViewItem(location.IpOrUrl);
                item.SubItems.Add(location.Country ?? "");
                item.SubItems.Add(location.City ?? "");
                item.SubItems.Add(location.Latitude.ToString());
                item.SubItems.Add(location.Longitude.ToString());
                item.SubItems.Add(location.RetrievedAt.ToString("yyyy-MM-dd HH:mm:ss"));

                lv_geo_result.Items.Add(item);
            }

            public GeoLocation? GetListGeoLocation()
            {
                if (lv_geo_result.Items.Count == 0) return null;

                var item = lv_geo_result.Items[0];

                return new GeoLocation
                {
                    IpOrUrl = item.SubItems[0].Text,
                    Country = item.SubItems[1].Text,
                    City = item.SubItems[2].Text,
                    Latitude = double.TryParse(item.SubItems[3].Text, out var lat) ? lat : 0,
                    Longitude = double.TryParse(item.SubItems[4].Text, out var lon) ? lon : 0,
                    RetrievedAt = DateTime.TryParse(item.SubItems[5].Text, out var date) ? date : DateTime.MinValue
                };
            }

            public void Submit() => SubmitAttempted?.Invoke(this, EventArgs.Empty);
            public void Save() => SaveAttempted?.Invoke(this, EventArgs.Empty);
            public void Reload() => ReloadAttempted?.Invoke(this, EventArgs.Empty);
            public void Remove() => RemoveAttempted?.Invoke(this, EventArgs.Empty);

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
    }
}