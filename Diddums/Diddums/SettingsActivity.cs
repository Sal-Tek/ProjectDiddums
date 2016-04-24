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

namespace Diddums
{
    [Activity(Label = "Settings")]
    public class SettingsActivity : Activity
    {
        private Button btnAbout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Settings);

            btnAbout = FindViewById<Button>(Resource.Id.btnAbout);
            btnAbout.Click += BtnAbout_Click;
        }

        private void BtnAbout_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder message = new AlertDialog.Builder(this);
            message.SetTitle("About Project Diddums");
            message.SetMessage("Test");
            message.Show();
        }
    }
}