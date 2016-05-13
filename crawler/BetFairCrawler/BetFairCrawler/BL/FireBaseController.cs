using BetFairCrawler.DL;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetFairCrawler.BL
{
    //communicate with Firebase wrapper
    public class FireBaseController
    {
        static FireBaseController _theInstance = null;
        public static FireBaseController TheInstance()
        {
            if (_theInstance == null)
                _theInstance = new FireBaseController();

            return _theInstance;
        }
        IFirebaseConfig _config;
        IFirebaseClient _client;
        private FireBaseController()
        {
            _config = new FirebaseConfig
            {
                AuthSecret = “secret key”,
                BasePath = “secret url fire base”

            };
            _client = new FirebaseClient(_config);
        }

        public async void SetMatch(MatchInfo m)
        {
            //var match = new Todo
            //{
            //    name = "Execute SET",
            //    priority = 2
            //};
            SetResponse response = await _client.SetAsync("Matches/set", m);
            MatchInfo result = response.ResultAs<MatchInfo>(); //The response will contain the data written
            //return result;
        }

        public async void PushAsyncMatch(MatchInfo m)
        {
            //var todo = new Todo {
            //    name = "Execute PUSH",
            //    priority = 2
            //};
            PushResponse response = await  _client.PushAsync("Matches", m);
            string name = response.Result.Name; //The result will contain the child name of the new data that was added
        }

        public string PushMatch(MatchInfo m)
        {
            //var todo = new Todo {
            //    name = "Execute PUSH",
            //    priority = 2
            //};
            PushResponse response = _client.Push("Matches", m);
            string name = response.Result.Name; //The result will contain the child name of the new data that was added
            return name;
        }

        public async Task<MatchInfo> GetAsyncMatches()
        {
            FirebaseResponse response = await _client.GetAsync("Matches");
            var match = response.ResultAs<MatchInfo>(); //The response will contain the data being retreived
            return match;
        }

        internal void UpdateMatch(string key, MatchInfo m)
        {
            FirebaseResponse response = _client.Update("Matches/" + key, m);
            var match = response.ResultAs<MatchInfo>(); //The response will contain the data written
        }
    }
}
