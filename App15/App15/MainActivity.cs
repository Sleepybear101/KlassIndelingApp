﻿using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Xamarin.Essentials;
using System;
using System.Drawing;
using Android.Content;
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

            if ("A".Equals(Intent.GetStringExtra("Theme")))
            {
                SetTheme(Resource.Style.AppThemeA);
            }
            else if ("B".Equals(Intent.GetStringExtra("Theme")))
            {
                SetTheme(Resource.Style.AppTheme);
            }

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource     
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var edittext = FindViewById<EditText>(Resource.Id.edittext);

            Button Buttontwo = FindViewById<Android.Widget.Button>(Resource.Id.button2);
            Button ButtonOne = FindViewById<Android.Widget.Button>(Resource.Id.button1);

            if ("lightmode".Equals(Intent.GetStringExtra("Mode")))
            {
                Buttontwo.Text = Intent.GetStringExtra("Mode");
                edittext.SetTextColor(Android.Graphics.Color.White);
                Buttontwo.SetTextColor(Android.Graphics.Color.Black);
                ButtonOne.SetTextColor(Android.Graphics.Color.Black);
            }

            ButtonOne.Click += (sender, e) =>
            {
                string number1 = Convert.ToString(edittext.Text);
                if (number1.Length <= 0)
                {
                    Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                    builder.SetTitle("ERROR");
                    builder.SetMessage("Please enter a value");
                    builder.SetNeutralButton("OK", delegate
                    {
                        builder.Dispose();
                    });
                    builder.Show();
                }
                else
                {
                    int number2 = Convert.ToInt32(number1);


                    if (number2 < 1)
                    {
                        Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                        builder.SetTitle("ERROR");
                        builder.SetMessage("You cannot enter a value below 1");
                        builder.SetNeutralButton("OK", delegate
                        {
                            builder.Dispose();
                        });
                        builder.Show();
                    }
                    else
                    {
                        //var lst_groups = FindViewById<ListView>(Resource.Id.listview_groups);
                        List<string> student_name = new List<string>();
                     
                        string responseString = GetStudentGroups(number2);
                        var groups = JsonConvert.DeserializeObject<List<Group>>(responseString);
                        var lst_groups = FindViewById<ListView>(Resource.Id.listview_groups);
                        // go through all the groups and seperate them
                        foreach (var group in groups)
                        {
                            // go through all the students in the different groups and seperate them
                            foreach (var student in group.Students)
                            {
                                // Set groupname + student name + student surname in list
                                student_name.Add(group.GroupNumber + " " + student.Name + " " + student.SurName);
                                //adding the list into seperate listitems for the listview
                                
                                var adapter = new ArrayAdapter<string>(this,
                                Android.Resource.Layout.SimpleListItem1, student_name);
                                //viewing the seperate list-items
                                lst_groups.Adapter = adapter;
                            }
                        }
                    }
                }
            };

            Buttontwo.Click += (sender, e) =>
            {
                if (Buttontwo.Text == "Darkmode")
                {
                    Intent intent = new Intent(this, typeof(MainActivity));
                    intent.PutExtra("Theme", "A");
                    intent.PutExtra("Mode", "lightmode");
                    StartActivity(intent);
                    Finish();
                }
                else if (Buttontwo.Text == "lightmode")
                {
                    Intent intent = new Intent(this, typeof(MainActivity));
                    intent.PutExtra("Theme", "B");
                    StartActivity(intent);
                    Finish();
                }
            };
        }
        private static string GetStudentGroups(int size)
        {
            //creating an webrequest to an azure function
            var request = WebRequest.Create("https://classdivider.azurewebsites.net/api/GetStudentGroups?code=8QhjS0G4vDQIM2IatwNjZjWxfLOXWei0XcwyO/2htLhC20qQD15nDg==");
            request.ContentType = "application/json";
            request.Method = "POST";
            var groupSize = size;
            //getting the input value and give it with the input variable
            var data = "{\"GroupSize\":" + groupSize + "}";
            //getting the json data from the azure function
            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.WriteLine(data);
            }
            var response = (HttpWebResponse)request.GetResponse();
            //putting the data from azure into an variable and returning it
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

