using System;
using System.Collections.Generic;
using FBGroupingApp.Client.ViewModel;
using Xamarin.Forms;

namespace FBGroupingApp.Client.Views
{
    public partial class NewsPage : ContentPage
    {
        public NewsPage()
        {
            InitializeComponent();
            BindingContext = new NewsViewModel();
        }

        //private async void Button_Clicked(object sender, EventArgs e)
        //{
        //    await NewsViewModel.DeleteFile(labFil.Text);
        //    labFil.Text = string.Empty;
        //    await DisplayAlert("Success", "Deleted", "OK");
        //}
    }
}
