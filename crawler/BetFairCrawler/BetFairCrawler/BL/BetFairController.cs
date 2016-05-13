using BetFairCrawler.DL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetFairCrawler.BL
{
    //this class will responsible for all tasks such as getting matches from bet-mate.co, then put these matches to Firebase cloud 
    public class BetFairController
    {
        private static BetFairController _theIntance = null;
        private Dictionary<string, MatchInfo> _lstAllMatches = null;
        //this list contains all keys are going to play from the bet-mate website
        //so, others match keys not in this list need to be update their status to 4: already played
        private List<string> _lstKeyMatchesGoingToPlay = new List<string>(); 
        private BetFairController()
        {
            _lstAllMatches = new Dictionary<string, MatchInfo>();
        }

        public static BetFairController TheIntance
        {
            get
            {
                if (_theIntance == null)
                    _theIntance = new BetFairController();

                return _theIntance; 
            }
        }

        //Craw Bet-mate.co website then push, update those info to FireBase cloud
        public void CrawBetFair()
        {
            //get all list matches from website
            List<MatchInfo> lstMatch = GetHtmlDocument();

            //reset the going to play list matches, so we will update it every time when fetch new data
            _lstKeyMatchesGoingToPlay.Clear();

            //after having a list of all Matches from Bet-mate, we need to check these matches are new or not
            //if new, add them to FireBase, 
            //if old, need to update it to Firebase also
            foreach (MatchInfo m in lstMatch)
            {
                //check this match is already in dictionary or not.
                string key = GetMatchKey(m);
                if (String.IsNullOrEmpty(key))
                {
                    //do not existed yet, need add it to Firebase.
                    string name = FireBaseController.TheInstance().PushMatch(m);
                    //put this object to dictionary
                    _lstAllMatches.Add(name, m);

                    //add to going to play list
                    _lstKeyMatchesGoingToPlay.Add(name);
                }
                else
                {
                    //already existed, need to update its value to Firebase and in the dictionary also
                    _lstAllMatches[key] = m;
                    FireBaseController.TheInstance().UpdateMatch(key, m);
                    _lstKeyMatchesGoingToPlay.Add(key);
                }
            }

            //finaly, need to udpate the all matches list, since there are some matches that had been played, we need to update 
            //the status of these matches to 4: already played, so every clients device will auto removed these matches.
            List<string> lstRemovedMatches = new List<string>();
            foreach (string key in _lstAllMatches.Keys)
            {
                if (!_lstKeyMatchesGoingToPlay.Contains(key))
                {
                    //this match had beed played, so need to update it status to firebase then remove it.
                    lstRemovedMatches.Add(key);
                    //get this match.
                    MatchInfo happenedMatch = _lstAllMatches[key];
                    happenedMatch.result = 4; //already happened. 
                    //update it to firebase.
                    FireBaseController.TheInstance().UpdateMatch(key, happenedMatch);
                }
            }

            //done, remove already played matches to memory, so we do not face over memory issue.
            foreach (string key in lstRemovedMatches)
                _lstAllMatches.Remove(key);
        }

        //get a key for a match if it is existed in the Dictionary
        private string GetMatchKey(MatchInfo inputMatch)
        {
           foreach(KeyValuePair<string, MatchInfo> pair in _lstAllMatches)
           {
               MatchInfo match = pair.Value;
               if (match.Equals(inputMatch))
                   return pair.Key;
           }
           return string.Empty;
        }

        /// <summary>
        /// This function is using Html Com to get the html document of a website that is running in IE only,
        /// since the betmate.co website takes some times to download all their odds, so we can not use webclient to request their 
        /// odds info. As a result, we have to use html document to get the content of this website that has been loaded in IE.
        /// </summary>
        private List<MatchInfo> GetHtmlDocument()
        {
            SHDocVw.ShellWindows shellWindows = new SHDocVw.ShellWindowsClass();

            string filename;

            foreach (SHDocVw.InternetExplorer ie in shellWindows)
            {
                filename = Path.GetFileNameWithoutExtension(ie.FullName).ToLower();

                if (filename.Equals("iexplore"))
                {
                    Console.WriteLine("Web Site   : {0}", ie.LocationURL);

                    if (ie.LocationURL == "http://www.website.com”)
                    {

                    //uses IHTMLDocument2 to get IE document info
                    mshtml.IHTMLDocument2 htmlDoc = ie.Document as mshtml.IHTMLDocument2;

                    Console.WriteLine("   Document Snippet: {0}",
                         ((htmlDoc != null) ? htmlDoc.body.outerHTML.Substring(0, 40)
                                               : "***Failed***"));
                    Console.WriteLine("{0}{0}", Environment.NewLine);

                    //pass it to HTMLAgilityPack to get all matches infos
                    return CrawlerController.GetAllMatchInfo(htmlDoc.body.innerHTML);                  
                     }   
                }
            }
            return null;
        }

    }
}
