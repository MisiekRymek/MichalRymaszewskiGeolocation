using MichalRymaszewskiGeolocation.Models;
using MichalRymaszewskiGeolocation.Presenters;
using MichalRymaszewskiGeolocation.Services;
using MichalRymaszewskiGeolocation.Views;

namespace TestProject1
{
    public class UnitTest1
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IPInputTest()
        {
            var user = new DummyUser();
            IDatabaseService dbService = new MySqlDatabaseService("localhost", "geodb", "root", "");
            IGeoService geoService = new IpStackGeoService("328cb5d926addf33d32aa8d6847ac352");
            var presenter = new GeoPresenter(user, dbService, geoService);
            user.Save();
            Assert.AreEqual(expected: null, actual:user.IPaddress, message: "IPaddress field should be initialized as null");
            Assert.IsTrue(user.ShowError, "Null ip should trigger error");
            Assert.AreEqual(expected:"IP address cannot be empty", actual:user.ErrorMessage, "Error message should indicate empty IP");

            //user.IPaddress = "192.168.1.1";
            //user.Save();
            //Assert.AreEqual(expected: "192.168.1.1", actual: user.IPaddress, message: "IPaddress field should be settable");
            //Assert.IsFalse(user.ShowError, "Valid ip should not trigger error");
            //Assert.AreEqual(expected:"", actual: user.ErrorMessage, "Error message should be empty for valid IP");

            user.IPaddress = "192.aaa.1.1";
            user.Save();
            Assert.IsTrue(user.ShowError, "Incorrect IP format should trigger error");
            Assert.AreEqual(expected: "Incorrect IP address format", actual: user.ErrorMessage, "Error message for incorrect IP format should be displayed");
        }
    }

    class DummyUser : IGeoServiceInput
    {
        public string IPaddress { get; set; }
        public string ErrorMessage { get; set; }
        public bool ShowError { get; set; }

        public event EventHandler SubmitAttempted;
        public event EventHandler SaveAttempted;
        public event EventHandler ReloadAttempted;

        public bool chk_AutoSaveToDB { get; }

        public GeoLocation LastLocation { get; private set; }

        public void ShowGeoLocation(GeoLocation location)
        {
            // For a dummy user, just print to console or store it in a property
            Console.WriteLine($"IP: {location.IpOrUrl}, Country: {location.Country}, City: {location.City}, Lat: {location.Latitude}, Lon: {location.Longitude}, RetrievedAt: {location.RetrievedAt}");

            // Or store in a property for later testing
            LastLocation = location;
        }

        public void Save() => SubmitAttempted?.Invoke(this, EventArgs.Empty);

        public void ToggleRemoveButton(bool enable)
        {
            throw new NotImplementedException();
        }

        public void ToggleSaveButton(bool enable)
        {
            throw new NotImplementedException();
        }
    }
}