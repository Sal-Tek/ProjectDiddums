using System;

using Android.App;
using Android.Content;
using Android.OS;
using Diddums.Data;
using Newtonsoft.Json;
using Android.Widget;

namespace Diddums
{
    [Activity(Label = "More Information")]
    public class InfoActivity : Activity
    {
        private NOTAMZone notamZone;
        private NoFlyZone noFlyZone;
        //private Climate climate;

        private TextView lblId;
        private TextView lblReference;
        private TextView lblMeaning;
        private TextView lblSuffix;

        private TextView lblCategory;
        private TextView lblName;
        private TextView lblType;
        private TextView lblFlightLower;
        private TextView lblFlightUpper;

        private LinearLayout noFlyZoneLayout;
        private LinearLayout notamZoneLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Info);

            noFlyZoneLayout = FindViewById<LinearLayout>(Resource.Id.noFlyZoneLayout);
            notamZoneLayout = FindViewById<LinearLayout>(Resource.Id.notamLayout);

            lblId = FindViewById<TextView>(Resource.Id.lblId);
            lblReference = FindViewById<TextView>(Resource.Id.lblReference);
            lblMeaning = FindViewById<TextView>(Resource.Id.lblMeaning);
            lblSuffix = FindViewById<TextView>(Resource.Id.lblSuffix);

            lblCategory = FindViewById<TextView>(Resource.Id.lblCategory);
            lblName = FindViewById<TextView>(Resource.Id.lblName);
            lblType = FindViewById<TextView>(Resource.Id.lblType);
            lblFlightLower = FindViewById<TextView>(Resource.Id.lblFlightLower);
            lblFlightUpper = FindViewById<TextView>(Resource.Id.lblFlightUpper);

            string rawAirspaceType = Intent.GetStringExtra("airspaceType");
            Type airspaceType = Type.GetType(rawAirspaceType);

            if (airspaceType == typeof(NOTAMZone))
            {
                string notamInfoJSON = Intent.GetStringExtra("airspaceInfo");
                if (!string.IsNullOrEmpty(notamInfoJSON))
                {
                    notamZone = JsonConvert.DeserializeObject<NOTAMZone>(notamInfoJSON);

                    notamZoneLayout.Visibility = Android.Views.ViewStates.Visible;

                    lblId.Text = notamZone.Id.ToString();
                    lblMeaning.Text = notamZone.Meaning;
                    lblReference.Text = notamZone.Reference;
                    lblSuffix.Text = notamZone.Suffix;
                }
            }
            else
            {
                string noFlyZoneJSON = Intent.GetStringExtra("airspaceInfo");
                if (!string.IsNullOrEmpty(noFlyZoneJSON))
                {
                    noFlyZone = JsonConvert.DeserializeObject<NoFlyZone>(noFlyZoneJSON);

                    noFlyZoneLayout.Visibility = Android.Views.ViewStates.Visible;

                    lblCategory.Text = noFlyZone.Category.ToString();
                    lblName.Text = noFlyZone.Name;
                    lblType.Text = noFlyZone.Name;
                    lblFlightLower.Text = noFlyZone.FlightLower.ToString();
                    lblFlightUpper.Text = noFlyZone.FlightUpper.ToString();
                }
            }

            //string weatherInfoJSON = Intent.GetStringExtra("weatherInfo");
            
            //if (!string.IsNullOrEmpty(weatherInfoJSON))
            //    climate = JsonConvert.DeserializeObject<Climate>(weatherInfoJSON);
        }
    }
}