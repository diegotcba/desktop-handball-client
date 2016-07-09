using HandballCliente.Models;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HandballCliente.Controllers
{
    public static class TwitterController
    {
        private static Playlist playlist = new Playlist();
        private static List<Tweets> tweets = new List<Tweets>();

        public static void newPlaylist()
        {
            playlist = new Playlist();
            playlist.tweets = new List<Tweets>();
        }

        public static void setPlaylist(String title, DateTime date, String descrip)
        {
            playlist.title = title;
            playlist.dateTime = date.ToShortDateString();
            playlist.description = descrip;
        }

        public static void addToPlaylist(Tweets tweet)
        {
            if (playlist == null || playlist.tweets == null)
            {
                newPlaylist();
            }
            playlist.tweets.Add(tweet);
        }

        public static void removeFromPlaylist(long id)
        {
            playlist.tweets.RemoveAll(t => t.id == id);
        }

        public static Playlist getPlaylist()
        {
            return playlist;
        }

        public static void mockingTwitterList()
        {
            List<Tweets> list = new List<Tweets>();
            Tweets t = new Tweets();

            t.id = 1;
            t.hashtag = "#LoQueSea";
            t.fullName = "Juan Perez";
            t.userName = "JPerez87";
            t.message = "Gracias por lo que nos dan todos los dias!!! Vamoooooooosss";

            list.Add(t);

            t = new Tweets();
            t.id = 2;
            t.hashtag = "#LoQueSea";
            t.fullName = "Maria Lopez";
            t.userName = "marylop006";
            t.message = "Son lo mas. Desde Jujuy damos nuestro apoyo y fuerzas.";

            list.Add(t);

            HandballMatch.getInstance().tweets = list;
        }

        public static ListViewItem[] fillTwitterSearchList()
        {
            ListViewItem[] results = new ListViewItem[HandballMatch.getInstance().tweets.Count];
            int i = 0;
            string[] arr;
            foreach (Tweets item in HandballMatch.getInstance().tweets)
            {
                arr = new string[5];
                arr[0] = item.id.ToString();
                arr[1] = item.message;
                arr[2] = item.userName;
                arr[3] = item.fullName;
                arr[4] = item.reTweet.ToString();
                results[i] = new ListViewItem(arr);
                i++;
            }
            return results;
        }

        public static ListViewItem[] fillTwitterList()
        {
            ListViewItem[] results = new ListViewItem[HandballMatch.getInstance().tweets.Count];
            int i = 0;
            string[] arr;
            foreach (Tweets item in HandballMatch.getInstance().tweets)
            {
                arr = new string[3];
                arr[0] = item.id.ToString();
                arr[1] = item.hashtag;
                arr[2] = item.userName;
                results[i] = new ListViewItem(arr);
                i++;
            }
            return results;
        }

        public static void getTwitterQuery(String endpoint, String hashtag)
        {
            var client = new RestClient(endpoint);
            var request = new RestRequest("/tweets/", Method.POST);

            TwitterQuery body = new TwitterQuery();
            body.hashtag = hashtag;
            string json = request.JsonSerializer.Serialize(body);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            //            request.AddHeader("Content-Type", "application/json"); 
            //            request.AddBody(body);

            var response = client.Execute(request);

            List<Tweets> twitterQueryResult = new List<Tweets>();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();
                twitterQueryResult = deserial.Deserialize<List<Tweets>>(response);
            }

            HandballMatch.getInstance().tweets = twitterQueryResult;
        }

        public static ListViewItem[] fillTwitterSearchPlaylist()
        {
            ListViewItem[] result = new ListViewItem[playlist.tweets.Count];
            int i = 0;
            string[] arr;
            foreach (Tweets item in playlist.tweets)
            {
                arr = new string[3];
                arr[0] = item.id.ToString();
                arr[1] = item.message;
                arr[2] = item.userName;
                result[i] = new ListViewItem(arr);
                i++;
            }
            return result;
        }

        public static void mockSavePlaylist()
        {
            int id = 1;
            if (HandballMatch.getInstance().playlists.Count != 0)
            {
                List<Playlist> aux = HandballMatch.getInstance().playlists;

                id = aux.Max(p => p.id) + 1;
            }
            playlist.id = id;

            HandballMatch.getInstance().playlists.Add(playlist);
        }

        public static void callSaveTwitterPlaylist(String endpoint)
        {
            var client = new RestClient(endpoint);
            var request = new RestRequest("/playlists/", Method.POST);

            Playlist body = new Playlist();
            //body.hashtag = txtTwitterSearchHashtag.Text;
            string json = request.JsonSerializer.Serialize(body);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            //            request.AddHeader("Content-Type", "application/json"); 
            //            request.AddBody(body);

            var response = client.Execute(request);

            Playlist twittePlaylist = new Playlist();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();
                twittePlaylist = deserial.Deserialize<Playlist>(response);
            }

            HandballMatch.getInstance().playlists.Add(twittePlaylist);
        }

        public static ListViewItem[] fillPlaylistList()
        {
            ListViewItem[] result = new ListViewItem[HandballMatch.getInstance().playlists.Count];

            int i = 0;
            string[] arr;
            foreach (Playlist item in HandballMatch.getInstance().playlists)
            {
                arr = new string[4];
                arr[0] = item.id.ToString();
                arr[1] = item.title;
                arr[2] = item.description;
                arr[3] = item.tweets.Count.ToString();
                result[i] = new ListViewItem(arr);
                i++;
            }
            return result;
        }

        public static void mockingTwitterPlaylist()
        {
            List<Playlist> playlists = new List<Playlist>();

            Playlist play = new Playlist();
            play.id = int.Parse("1");
            play.title = "Primero";
            play.description = "";
            play.tweets = HandballMatch.getInstance().tweets;

            playlists.Add(play);

            play = new Playlist();
            play.id = int.Parse("2");
            play.title = "Segundo";
            play.description = "";
            play.tweets = HandballMatch.getInstance().tweets;

            playlists.Add(play);

            if (HandballMatch.getInstance().playlists.Count == 0)
            {
                HandballMatch.getInstance().playlists = playlists;
            }
        }

        public static void getTwitterPlaylistList(String endpoint)
        {
            var client = new RestClient(endpoint);
            var request = new RestRequest("/tweets/playlist/", Method.GET);

            //            request.AddHeader("Content-Type", "application/json"); 

            var response = client.Execute(request);

            List<Playlist> twitterQueryResult = new List<Playlist>();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();
                twitterQueryResult = deserial.Deserialize<List<Playlist>>(response);
            }

            HandballMatch.getInstance().playlists = twitterQueryResult;
        }

        public static Playlist selectPlaylist(long id)
        {
            Playlist aux = HandballMatch.getInstance().playlists.Find(element => element.id == id);
            return aux;
        }

        public static ListViewItem[] fillSelectedPlaylistTweets(long id)
        {
            ListViewItem[] result = new ListViewItem[selectPlaylist(id).tweets.Count];
            int i = 0;
            string[] arr;
            foreach (Tweets item in selectPlaylist(id).tweets)
            {
                arr = new string[3];
                arr[0] = item.id.ToString();
                arr[1] = item.hashtag;
                arr[2] = item.userName;
                result[i] = new ListViewItem(arr);
                i++;
            }

            return result;
        }

    }
}
