using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FBGroupingApp.Server.Model;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using Plugin.FilePicker;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

namespace FBGroupingApp.Server.Views
{

    public partial class AdminNewsFeedPage : ContentPage
    {
        byte[] imagebytes;
        public AdminNewsFeedPage()
        {
            InitializeComponent();
            GetPhotoPermissionAsync();
        }

        public async void Button_Clicked(object sender, EventArgs args)
        {
            MyActivity.IsVisible = true;
            MyActivity.IsRunning = true;
            if (string.IsNullOrEmpty(AdminText.Text))
            {
                MyActivity.IsVisible = false;
                MyActivity.IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Alert", "Please add your news information", "Ok");
                return;
            }
            if (string.IsNullOrEmpty(filepath))
            {
                MyActivity.IsVisible = false;
                MyActivity.IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Alert", "Please add image for the news", "Ok");
                return;
            }
            var current = Connectivity.NetworkAccess;
            try
            {
                if (current == NetworkAccess.Internet)
                {
                    Guid guid = Guid.NewGuid();
                    FirebaseClient fc = new FirebaseClient("https://allabeed-default-rtdb.firebaseio.com/");
                    var result = await fc
                     .Child("NewsTable")
                     .PostAsync(new NewsTable() { NewsDateTime = DateTime.UtcNow.ToLocalTime().ToString("dd MMM yyyy"), NewsText = AdminText.Text, IDentifier = guid.ToString(),NewsDetailTime = DateTime.UtcNow.ToLocalTime()});
                    PerformStorage(guid);
                }
                else
                {
                    MyActivity.IsVisible = false;
                    MyActivity.IsRunning = false;
                    await App.Current.MainPage.DisplayAlert("Alert", "No Internet Connection", "Ok");
                }

            }
            catch (Exception ex)
            {
                MyActivity.IsVisible = false;
                MyActivity.IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
            }
        }

        private async void PerformStorage(Guid guid)
        {
            try
            {
                Stream stream = new MemoryStream(imagebytes);
                var task = await new FirebaseStorage("allabeed.appspot.com")
                    .Child("Interplanetary")
                    .Child(guid + ".png")
                    .PutAsync(stream);
                AdminText.Text = "";
                BackText.IsVisible = true;
                MyImage.Source = null;
                filepath = "";
                await App.Current.MainPage.DisplayAlert("Alert", "Sucessfully added the news", "Ok");
                MyActivity.IsVisible = false;
                MyActivity.IsRunning = false;
            }
            catch (Exception ex)
            {
                MyActivity.IsVisible = false;
                MyActivity.IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
            }
        }
        string filepath;

        async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            string[] fileTypes = null;

            if (Device.RuntimePlatform == Device.Android)
            {
                fileTypes = new string[] { "image/png", "image/jpeg" };
            }

            if (Device.RuntimePlatform == Device.iOS)
            {
                fileTypes = new string[] { "public.image" }; // same as iOS constant UTType.Image
            }

            if (Device.RuntimePlatform == Device.UWP)
            {
                fileTypes = new string[] { ".jpg", ".png" };
            }

            if (Device.RuntimePlatform == Device.WPF)
            {
                fileTypes = new string[] { "JPEG files (*.jpg)|*.jpg", "PNG files (*.png)|*.png" };
            }

            await PickAndShowFile(fileTypes);
        }

        private async Task PickAndShowFile(string[] fileTypes)
        {
            try
            {
                var pickedFile = await CrossFilePicker.Current.PickFile(fileTypes);

                if (pickedFile != null)
                {
                    if (pickedFile.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase)
                        || pickedFile.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                    {
                        MyImage.Source = ImageSource.FromStream(() => pickedFile.GetStream());

                        filepath = pickedFile.FilePath;
                        imagebytes = pickedFile.DataArray;
                        BackText.IsVisible = false;
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {

                BackText.IsVisible = true;
            }


        }

        public async Task GetPhotoPermissionAsync()
        {
            var status = await CheckAndRequestPermissionAsync(new Permissions.Photos());
            if (status != PermissionStatus.Granted)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "You need to provide permission to access photos.", "Ok");
                return;
            }
        }

        public async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission)
                    where T : BasePermission
        {
            var status = await permission.CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                status = await permission.RequestAsync();
            }

            return status;
        }
       
    }
}