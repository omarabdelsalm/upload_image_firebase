using System;
using System.Collections.ObjectModel;
using FBGroupingApp.Client.ViewModel;
using Xamarin.Forms;

namespace FBGroupingApp.Client.Model
{
	public class ClientNewsModel
	{
     public string News { get; set; }
     public  ImageSource NewsImagePath { get; set; }

	 public ClientNewsModel()
    {

    }
}

    public class GroupedClientNewsModel : ObservableCollection<ClientNewsModel>
	{
		public string NewsDateTime { get; set; }
	}
}
