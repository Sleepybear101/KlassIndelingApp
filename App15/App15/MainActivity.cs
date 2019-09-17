using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Xamarin.Essentials;
using System;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.Net;
using System.IO;
using System.Collections.Generic;


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
            var ButtonOne = FindViewById<Android.Widget.Button>(Resource.Id.button1);

            ButtonOne.Click += (sender, e) =>
            {
                string number1 = Convert.ToString(edittext.Text);
                int number2 = Convert.ToInt32(number1);
                string responseString = GetStudentGroups(number2);
             

                
                textview.Text = responseString;

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

        private static string GetStudentGroups(int size)
        {
            var request = WebRequest.Create("https://classdivider.azurewebsites.net/api/GetStudentGroups?code=8QhjS0G4vDQIM2IatwNjZjWxfLOXWei0XcwyO/2htLhC20qQD15nDg==");
            request.ContentType = "application/json";
            request.Method = "POST";
            var groupSize = size;
            var data = "{\"GroupSize\":" + groupSize + "}";
            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.WriteLine(data);
            }
            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }

        public class Student
        {
            public string StudentNumber { get; set; }
            public string Name { get; set; }
            public string SurName { get; set; }
            public string StudentClass { get; set; }
        }

        public class Group
        {
            public int GroupNumber { get; set; }
            public List<Student> Students { get; set; }
        }







    }
}