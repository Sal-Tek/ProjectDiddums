
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Diddums.Data;
using Newtonsoft.Json;

namespace Diddums
{
    [Activity(Label = "Manual Coordinate Input")]
    public class CoordsActivity : Activity
    {
        private EditText txtLatitude;
        private EditText txtLongitude;
        private Button btnSubmit;
        private Coordinates coordinates;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Coords);

            txtLatitude = FindViewById<EditText>(Resource.Id.txtLatitude);
            txtLongitude = FindViewById<EditText>(Resource.Id.txtLongitude);

            btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);
            btnSubmit.Click += BtnSubmit_Click;

            string coordinatesJson = Intent.GetStringExtra("coordinates");
            if (coordinatesJson != null)
            {
                coordinates = JsonConvert.DeserializeObject<Coordinates>(coordinatesJson);

                txtLatitude.Text = coordinates.Latitude == 0 ? string.Empty : coordinates.Latitude.ToString();
                txtLongitude.Text = coordinates.Longitude == 0 ? string.Empty : coordinates.Longitude.ToString();
            }
        }

        private void BtnSubmit_Click(object sender, System.EventArgs e)
        {
            coordinates.Longitude = float.Parse(txtLongitude.Text);
            coordinates.Latitude = float.Parse(txtLatitude.Text);

            string coordinatesJson = JsonConvert.SerializeObject(coordinates);

            Intent intent = new Intent(this.ApplicationContext, typeof(MapActivity));
            intent.PutExtra("coordinates", coordinatesJson);

            StartActivity(intent);
            Finish();
        }
    }
}