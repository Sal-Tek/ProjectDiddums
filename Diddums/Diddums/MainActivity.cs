using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Diddums.Data;
using Android.Content.PM;
using Android.Net;
using Android.Locations;
using Newtonsoft.Json;

namespace Diddums
{
    [Activity(MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity, ILocationListener
    {
        private Button btnStart;

        private LocationManager locManager;
        private Coordinates coordinates;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Main);

            btnStart = FindViewById<Button>(Resource.Id.btnStart);
            btnStart.Click += BtnStart_Click;

            locManager = (LocationManager)GetSystemService(LocationService);
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            // Check whether internet connection is available.
            if (!IsConnected())
            {
                AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                dialog.SetMessage(Resource.String.NoInternetConnectionMessage);
                dialog.SetTitle(Resource.String.NoInternetConnectionTitle);
                dialog.Show();

                return;
            }

            // Attempt to retrieve coordinates.

            Criteria locationCriteria = new Criteria();

            locationCriteria.Accuracy = Accuracy.Fine;
            locationCriteria.PowerRequirement = Power.High;

            bool gpsProvider = locManager.IsProviderEnabled(LocationManager.GpsProvider);
            bool networkProvider = locManager.IsProviderEnabled(LocationManager.NetworkProvider);

            if (gpsProvider || networkProvider)
            {
                btnStart.Enabled = false;
                btnStart.Text = Resources.GetString(Resource.String.StartButtonLoading);

                if (gpsProvider)
                {
                    locManager.RequestSingleUpdate("gps", this, null);
                }
                else if (networkProvider)
                {
                    locManager.RequestSingleUpdate("network", this, null);
                }
            }
            else
            {
                AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                dialog.SetMessage(Resource.String.GpsNotAvailableMessage);
                dialog.SetTitle(Resource.String.GpsNotAvailableTitle);
                dialog.SetPositiveButton(Resource.String.GpsNotAvailablePosButton, delegate
                {
                    Intent intent = new Intent(this.ApplicationContext, typeof(CoordsActivity));

                    string coordinatesJson = JsonConvert.SerializeObject(coordinates);
                    intent.PutExtra("coordinates", coordinatesJson);

                    StartActivity(intent);
                });
                dialog.SetNegativeButton(Resource.String.GpsNotAvailableNegButton, delegate { });
                dialog.Show();
            }
        }
        
        private bool IsConnected()
        {
            ConnectivityManager conManager = (ConnectivityManager)GetSystemService(ConnectivityService);
            NetworkInfo networkInfo = conManager.ActiveNetworkInfo;
            return networkInfo != null && networkInfo.IsConnected;
        }

        /* LOCATION HANDLING
         * ----------------------------------------------------------------*/

        public void OnLocationChanged(Location location)
        {
            coordinates = new Coordinates
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };

            string coordinatesJson = JsonConvert.SerializeObject(coordinates);

            Intent intent = new Intent(this.ApplicationContext, typeof(SettingsActivity));
            intent.PutExtra("coordinates", coordinatesJson);

            StartActivity(intent);
        }

        public void OnProviderDisabled(string provider)
        {
            btnStart.Enabled = true;
            btnStart.Text = Resources.GetString(Resource.String.StartButton);
        }

        public void OnProviderEnabled(string provider)
        {
            
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            
        }
        
        private void About_Click(object sender, EventArgs e)
        {
            StartActivity(new Intent(this.ApplicationContext, typeof(SettingsActivity)));
        }
    }
}