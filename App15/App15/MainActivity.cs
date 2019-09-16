using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Xamarin.Essentials;
using System;

namespace App15
{

    [Activity(Label = "GroepIndeling", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            var edittext = FindViewById<EditText>(Resource.Id.edittext);
            var textview = FindViewById<TextView>(Resource.Id.Textview);
            var ButtonOne = FindViewById<Button>(Resource.Id.button1);

            ButtonOne.Click += (sender, e) =>
            {

                textview.Text = edittext.Text;

            };


            Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;

            void Accelerometer_ShakeDetected(object sender, EventArgs e)
            {
                textview.Text = string.Empty;
                Accelerometer.Stop();
            }

            
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }



    }
}