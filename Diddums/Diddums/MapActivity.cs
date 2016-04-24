using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Android.Gms.Maps;
using Android.Locations;
using Android.Gms.Maps.Model;
using Android.Support.V4.App;
using Diddums.Data;
using Newtonsoft.Json;
using static Android.Gms.Maps.GoogleMap;
using System.Security;
using System.Security.Cryptography;

namespace Diddums
{
    [Activity(ScreenOrientation = ScreenOrientation.Landscape)]
    public class MapActivity : FragmentActivity, IOnMapReadyCallback
    { 
        private GoogleMap googleMap;
        private Coordinates coordinates;
        private Airspace airspace;

        private string[] hashes;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Map);

            string coordinatesJson = Intent.GetStringExtra("coordinates");
            coordinates = JsonConvert.DeserializeObject<Coordinates>(coordinatesJson);

            SupportMapFragment mapFragment = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);
        }

        public async void OnMapReady(GoogleMap googleMap)
        {
            float lat = (float)coordinates.Latitude;
            float lng = (float)coordinates.Longitude;

            LatLng coords = new LatLng(lat, lng);

            CameraPosition cameraPosition;

            using (CameraPosition.Builder builder = new CameraPosition.Builder())
            {
                builder.Zoom(12);
                builder.Target(coords);
                cameraPosition = builder.Build();
            }

            this.googleMap = googleMap;

            this.googleMap.MoveCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));
            this.googleMap.MapType = 3;

            this.googleMap.InfoWindowLongClick += GoogleMap_InfoWindowLongClick;

            MarkerOptions myLocationMarker = new MarkerOptions();
            myLocationMarker.SetPosition(new LatLng(lat, lng));
            myLocationMarker.SetTitle("My location");
            this.googleMap.AddMarker(myLocationMarker);

            var airScraper = new AirspaceScraper(lng, lat, 120);

            airspace = await airScraper.ScrapeAsync();

            hashes = new string[airspace.NOTAMZones.Length];

            for (int x = 0; x < airspace.NOTAMZones.Length; x++)
            {
                PolygonOptions polygonOptions = new PolygonOptions();
                for (int i = 0; i < airspace.NOTAMZones[x].Polypoints.Length; i++)
                {
                    polygonOptions.Add(new LatLng(airspace.NOTAMZones[x].Polypoints[i].Latitude, airspace.NOTAMZones[x].Polypoints[i].Longitude));
                }

                hashes[x] = $"{airspace.NOTAMZones[x].Latitude}{airspace.NOTAMZones[x].Longitude}";

                MarkerOptions notamMarker = new MarkerOptions();
                notamMarker.SetPosition(new LatLng(airspace.NOTAMZones[x].Latitude, airspace.NOTAMZones[x].Longitude));
                notamMarker.SetTitle($"{airspace.NOTAMZones[x].Reference} - {airspace.NOTAMZones[x].Meaning}");
                this.googleMap.AddMarker(notamMarker);

                this.googleMap.AddPolygon(polygonOptions);
            }
        }

        private void GoogleMap_InfoWindowLongClick(object sender, InfoWindowLongClickEventArgs e)
        {
            NOTAMZone notamZone = new NOTAMZone();
            string hash = $"{e.Marker.Position.Latitude}{e.Marker.Position.Longitude}";
            for (int i = 0; i < hashes.Length; i++)
            {
                if (hash == hashes[i])
                {
                    notamZone = airspace.NOTAMZones[i];
                    break;
                }
            }

            Intent intent = new Intent(this.ApplicationContext, typeof(InfoActivity));

            string notamZoneJson = JsonConvert.SerializeObject(notamZone);
            intent.PutExtra("airspaceInfo", notamZoneJson);
            intent.PutExtra("airspaceType", typeof(NOTAMZone).ToString());

            StartActivity(intent);
        }
    }
}