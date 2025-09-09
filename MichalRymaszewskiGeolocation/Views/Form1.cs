using MichalRymaszewskiGeolocation.Models;
using MichalRymaszewskiGeolocation.Presenters;
//using MichalRymaszewskiGeolocation.Repositories;
using MichalRymaszewskiGeolocation.Services;
using MichalRymaszewskiGeolocation.Views;
using MichalRymaszewskiGeolocation.Utils;

namespace MichalRymaszewskiGeolocation
{
    public partial class Form1 : Form, IGeoServiceInput
    {
        private GeoPresenter _presenter;
        IDatabaseService dbService = new MySqlDatabaseService("localhost", "geodb", "root", "");
        IGeoService geoService = new IpStackGeoService(Config.IPstackApiKey);

        string IGeoServiceInput.IPaddress { get => this.txtb_sumbitIP.Text; set => this.txtb_sumbitIP.Text = value; }
        string IGeoServiceInput.ErrorMessage { get => this.ErrorMessage.Text; set => this.ErrorMessage.Text = value; }
        bool IGeoServiceInput.ShowError { get => this.ErrorMessage.Visible; set => this.ErrorMessage.Visible = value; }

        public event EventHandler SubmitAttempted;
        public event EventHandler SaveAttempted;
        public event EventHandler RemoveAttempted;
        public event EventHandler ReloadAttempted;

        public bool chk_AutoSaveToDB => chk_autosave.Checked;

        public Form1()
        {
            InitializeComponent();
            this.Text = "IP/URL Geolocation";
            ErrorMessage.Text = "";
            this._presenter = new GeoPresenter(this, dbService, geoService);


            lv_geo_result.View = View.Details;
            lv_geo_result.FullRowSelect = true;
            lv_geo_result.Columns.Add("IP / URL", 120);
            lv_geo_result.Columns.Add("Country", 100);
            lv_geo_result.Columns.Add("City", 100);
            lv_geo_result.Columns.Add("Latitude", 80);
            lv_geo_result.Columns.Add("Longitude", 80);
            lv_geo_result.Columns.Add("Retrieved At", 150);
        }

        private void btn_submitIP_Click(object sender, EventArgs e)
        {
            SubmitAttempted?.Invoke(this, EventArgs.Empty);
        }

        private void btn_saveDB_Click(object sender, EventArgs e)
        {
            SaveAttempted?.Invoke(this, EventArgs.Empty);
        }
        private void btn_removeDB_Click(object sender, EventArgs e)
        {
            RemoveAttempted?.Invoke(this, EventArgs.Empty);
        }
        private void btn_refreshIP_Click(object sender, EventArgs e)
        {
            ReloadAttempted?.Invoke(this, EventArgs.Empty);
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

        public void ToggleRemoveButton(bool enable)
        {
            btn_removeDB.Enabled = enable;
        }

        public void ToggleSaveButton(bool enable)
        {
            btn_saveDB.Enabled = enable;
        }
    }
}
