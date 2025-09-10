using MichalRymaszewskiGeolocation.Models;

namespace MichalRymaszewskiGeolocation.Views
{
    public interface IGeoServiceInput
    {
        public string IPaddress { get; set; }
        public string ErrorMessage { get; set; }
        public bool ShowError { get; set; }
        public event EventHandler SubmitAttempted;
        public event EventHandler SaveAttempted;
        public event EventHandler RemoveAttempted;
        public event EventHandler ReloadAttempted;

        public bool chk_AutoSaveToDB { get; }
        public void ShowGeoLocation(GeoLocation location);
        public void ToggleRemoveButton(bool enable);
        public void ToggleSaveButton(bool enable);
        public GeoLocation? GetListGeoLocation();
    }
}
