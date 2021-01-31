using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FBGroupingApp.Client.Model;
using FBGroupingApp.Server.Model;
using Firebase.Database;
using Firebase.Storage;
using Xamarin.Forms;

namespace FBGroupingApp.Client.ViewModel
{
    public class NewsViewModel : BindableObject
    {
        private ObservableCollection<GroupedClientNewsModel> newsItem;
        public ObservableCollection<GroupedClientNewsModel> NewsItem
        {
            get { return newsItem; }
            set
            {
                newsItem = value;
                OnPropertyChanged();
            }
        }

        public NewsViewModel()
        {
            NewsItem = new ObservableCollection<GroupedClientNewsModel>();
            GetNewsInformation();
        }

        private async void GetNewsInformation()
        {
            FirebaseClient fc = new FirebaseClient("https://allabeed-default-rtdb.firebaseio.com/");
            FirebaseStorage firebaseStorage = new FirebaseStorage("allabeed.appspot.com");
            var GetNews = (await fc
              .Child("NewsTable")
              .OnceAsync<NewsTable>()).Select(item => new NewsTable
              {
                  NewsDateTime = item.Object.NewsDateTime,
                  NewsText = item.Object.NewsText,
                  IDentifier = item.Object.IDentifier
              }).ToList().OrderBy(i=>i.NewsDateTime);

            var headergroup = GetNews.Select(x => x.NewsDateTime).Distinct().ToList();

            foreach(var item in headergroup)
            {
                var newsGroup = new GroupedClientNewsModel() { NewsDateTime = item};

                var contents = GetNews.Where(i => i.NewsDateTime == item).ToList();

                if(contents.Count!= 0)
                {
                   foreach(var groupitems in contents)
                    {
                        var filepath = await firebaseStorage
                            .Child("Interplanetary")
                            .Child(groupitems.IDentifier +".png")
                            .GetDownloadUrlAsync();
                        newsGroup.Add(new ClientNewsModel() { News = groupitems.NewsText ,NewsImagePath = filepath});
                    }
                    NewsItem.Add(newsGroup);
                }


            }
        }
        
    }
}
