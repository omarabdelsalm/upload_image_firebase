using System;
using FBGroupingApp.Client.Views;
using FBGroupingApp.Server.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FBGroupingApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
           MainPage = new NewsPage();
          //MainPage = new AdminNewsFeedPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
