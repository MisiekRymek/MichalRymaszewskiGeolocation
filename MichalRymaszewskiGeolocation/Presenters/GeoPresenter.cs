//using MichalRymaszewskiGeolocation.Repositories;
using MichalRymaszewskiGeolocation.Models;
using MichalRymaszewskiGeolocation.Services;
using MichalRymaszewskiGeolocation.Views;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MichalRymaszewskiGeolocation.Presenters
{
    public class GeoPresenter
    {
        private readonly IGeoServiceInput _view;
        private readonly IDatabaseService _dbService;
        private readonly IGeoService _geoService;
        private GeoLocation GeoFoundData;
        bool possible_db_save = false;
        public GeoPresenter(IGeoServiceInput view, IDatabaseService dbService, IGeoService geoService)
        {
            _view = view;
            _dbService = dbService;
            _geoService = geoService;

            _view.SubmitAttempted += OnSubmitAttempted;
            _view.SaveAttempted += OnSaveAttempted;
            _view.RemoveAttempted += OnRemoveAttempted;
            _view.ReloadAttempted += OnReloadAttempted;
            _view.ToggleRemoveButton(false);
            _view.ToggleSaveButton(false);

            if (!_dbService.CheckDbStatus())
            {
                _view.ShowError = true;
                _view.ErrorMessage = "Database not avaliable";
            }
        }
        private async void OnSubmitAttempted(object? sender, EventArgs e)
        {
            _view.ShowError = false;
            _view.ErrorMessage = "";
            await HandleGeoSubmissionAsync(_view.IPaddress, useDatabase: true);
        }

        private async void OnSaveAttempted(object? sender, EventArgs e)
        {
            if (_dbService.Insert(GeoFoundData))
            {
                _view.ToggleSaveButton(false);
                _view.ToggleRemoveButton(true);
            }
            else
            {
                _view.ShowError = true;
                _view.ErrorMessage = "problem saving to database";
            }
        }

        private async void OnRemoveAttempted(object? sender, EventArgs e)
        {
            Debug.WriteLine("REMOVE");
            if (_dbService.Delete(_view.IPaddress))
            {
                Debug.WriteLine("REMOVE finished");
                _view.ToggleSaveButton(true);
                _view.ToggleRemoveButton(false);
            }
            else
            {
                _view.ShowError = true;
                _view.ErrorMessage = "problem removing from database";
            }
        }

        private async void OnReloadAttempted(object? sender, EventArgs e)
        {
            _view.ShowError = false;
            _view.ErrorMessage = "";
            await HandleGeoSubmissionAsync(_view.IPaddress, useDatabase: false);
        }

        private async Task HandleGeoSubmissionAsync(string ipAddress, bool useDatabase)
        {
            string ip_regex = @"^(\b25[0-5]|\b2[0-4][0-9]|\b[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}$";
            string url_regex_www = @"^www\.([a-zA-Z0-9-]+)(\.[a-zA-Z]{2,3})+$";
            string url_regex = @"^([a-zA-Z0-9-]+)(\.[a-zA-Z]{2,3})+$";
            Uri uri;
            bool correct_ip_url = false;
            if (string.IsNullOrEmpty(ipAddress))
            {
                _view.ShowError = true;
                _view.ErrorMessage = "IP address or URL cannot be empty";
                return;
            }
            else if ((Regex.IsMatch(ipAddress, ip_regex)) || (Regex.IsMatch(ipAddress, url_regex_www)) || (Regex.IsMatch(ipAddress, url_regex)))
            {
                if(Regex.IsMatch(ipAddress, ip_regex))
                    Debug.WriteLine($"ip regex");
                else if(Regex.IsMatch(ipAddress, url_regex_www))
                    Debug.WriteLine($"url www regex");
                else if(Regex.IsMatch(ipAddress, url_regex))
                    Debug.WriteLine($"url regex");

                correct_ip_url = true;
            }
            else if (Uri.TryCreate(ipAddress, UriKind.Absolute, out uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            {
                Debug.WriteLine($"URI");
                if (Uri.TryCreate(ipAddress, UriKind.Absolute, out var uri_host))
                {
                    ipAddress = uri_host.Host;
                }

                correct_ip_url = true;
            }

            if (!correct_ip_url)
            {
                _view.ShowError = true;
                _view.ErrorMessage = "Incorrect IP or URL format.";
                return;
            }

            Debug.WriteLine($"IP Address submitted: {ipAddress}");

            if (useDatabase)
            {
                var db_result = _dbService.Find(ipAddress);
                if (db_result != null)
                {
                    GeoFoundData = db_result;
                    _view.ShowGeoLocation(db_result);
                    _view.ToggleSaveButton(false);
                    _view.ToggleRemoveButton(true);
                    return;
                }
            }

            var ipstack_result = await _geoService.GetGeoLocationAsync(ipAddress);
            if (ipstack_result.Success)
            {
                GeoFoundData = ipstack_result.Data;
                _view.ShowGeoLocation(GeoFoundData);
                Debug.WriteLine("IP FOUND");

                if (_view.chk_AutoSaveToDB)
                {
                    _dbService.Insert(GeoFoundData);
                    Debug.WriteLine("auto db insert");
                    _view.ToggleSaveButton(false);
                    _view.ToggleRemoveButton(true);
                }
                else
                {
                    _view.ToggleSaveButton(true);
                    _view.ToggleRemoveButton(false);
                }
            }
            else
            {
                _view.ShowError = true;
                //_view.ErrorMessage = "problem connecting to IPStack";
                _view.ErrorMessage = ipstack_result.ErrorMessage;
                Debug.WriteLine("PROBLEM");
            }
        }
    }
}
