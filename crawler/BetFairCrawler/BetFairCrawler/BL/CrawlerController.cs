using BetFairCrawler.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetFairCrawler.BL
{
    class CrawlerController
    {
        //get all matches from url
        public static List<MatchInfo> GetAllMatchInfo(string html)
        {
            //return null if the html string is null or empty
            if (string.IsNullOrEmpty(html))
                return null;

            List<MatchInfo> lstMatch = new List<MatchInfo>();

            //using HtmlAgilityPack to get document content of the url
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            if (doc != null)
            {
                //need a root node of the html document
                var node = doc.DocumentNode;

                //the bet-mate.co contains the data-odds div that holds all matches info, so we need to have this node, 
                //then, traversal all its child nodes to get all matches info
                //we use xpath to get all the odds
                var dataOdds = node.SelectNodes("//div[@id='data-odds']");
                //all odd matches info are in the childs of the data-odds nodes
                if (dataOdds != null)
                {
                    var lstDataOdds = dataOdds.ToList();
                    foreach (HtmlAgilityPack.HtmlNode odd in lstDataOdds[0].ChildNodes)
                    {
                        //create each MatchInfo object that corresponding with each child nodes
                        MatchInfo match = new MatchInfo();

                        //get match type first.
                        //div[@class='left col-xs-4']//div[@class='col-md-2 col-xs-4 col-sm-4']//span[@class='f24 flag Scotland']
                        var nodeType = odd.SelectNodes("div[@class='left col-xs-4']//div[@class='col-md-2 col-xs-4 col-sm-4']//span");
                        if (nodeType != null)
                        {
                            string type = nodeType[0].Attributes[0].Value;
                            match.type = type;
                        }

                        //get match event and time,
                        //match time: col-md-3 hidden-xs hidden-sm
                        //div[@class='left col-xs-4']//div[@class='col-md-3 hidden-xs hidden-sm']
                        var nodeTime = odd.SelectNodes("div[@class='left col-xs-4']//div[@class='col-md-3 hidden-xs hidden-sm']");
                        if (nodeTime != null)
                        {
                            string time = nodeTime[0].ChildNodes[0].InnerText;
                            match.time = time;
                        }

                        //match event
                        //div[@class='left col-xs-4']//div[@class='col-md-7 col-xs-8 col-sm-8']//span
                        var nodeEvent = odd.SelectNodes("div[@class='left col-xs-4']//div[@class='col-md-7 col-xs-8 col-sm-8']");
                        if (nodeEvent != null)
                        {
                            string eventText = nodeEvent[0].InnerText;
                            if (eventText.Contains(match.time))
                                eventText = eventText.Substring(match.time.Length);

                            match.name = eventText;
                        }

                        //match odds
                        //div[@class='right col-xs-8 text-center']//div[@class='col-xs-5 p0']
                        var nodeOddInfo = odd.SelectNodes("div[@class='right col-xs-8 text-center']//div[@class='col-xs-5 p0']");
                        if (nodeOddInfo != null)
                        {
                            //there are 2 'col-xs-5 p0', first will be used for odd, the second will be used for money and pick
                            HtmlAgilityPack.HtmlNode oddDetail = nodeOddInfo[0];
                            if (oddDetail != null)
                            {
                                //get odd for home team.
                                HtmlAgilityPack.HtmlNode oddHome = oddDetail.ChildNodes[0];
                                string strOddHome = oddHome.InnerHtml;
                                //oddHome is in "1.234<br>1.345" format
                                string[] oddNumbers = strOddHome.Split(new string[]{"<br>"}, StringSplitOptions.None);
                                double startOdd = 0.0;
                                double currentOdd = 0.0;
                                GetOddFromString(oddNumbers, ref startOdd, ref currentOdd);

                                match.startHomeOdd = startOdd;
                                match.currentHomeOdd = currentOdd;

                                //need to get attribute of this odd home to explore it is hot drop or not
                                string dropType = oddHome.Attributes != null ? oddHome.Attributes[0].Value : string.Empty;
                                match.oddHomeType = GetOddTypeFromString(dropType);


                                //get odd for draw case.
                                HtmlAgilityPack.HtmlNode oddDraw = oddDetail.ChildNodes[1];
                                string strOddDraw = oddDraw.InnerHtml;
                                //oddHome is in "1.234<br>1.345" format
                                oddNumbers = strOddDraw.Split(new string[] { "<br>" }, StringSplitOptions.None);
                                GetOddFromString(oddNumbers, ref startOdd, ref currentOdd);

                                match.startDrawOdd = startOdd;
                                match.currentDrawOdd = currentOdd;

                                //need to get attribute of this odd home to explore it is hot drop or not
                                dropType = oddDraw.Attributes != null ? oddDraw.Attributes[0].Value : string.Empty;
                                match.oddDrawType = GetOddTypeFromString(dropType);

                                //get odd for away team.
                                HtmlAgilityPack.HtmlNode oddAway = oddDetail.ChildNodes[2];
                                string strOddAway = oddAway.InnerHtml;
                                //oddAway is in "1.234<br>1.345" format
                                oddNumbers = strOddAway.Split(new string[] { "<br>" }, StringSplitOptions.None);
                                GetOddFromString(oddNumbers, ref startOdd, ref currentOdd);

                                match.startAwayOdd = startOdd;
                                match.currentAwayOdd = currentOdd;

                                //need to get attribute of this odd home to explore it is hot drop or not
                                dropType = oddAway.Attributes != null ? oddAway.Attributes[0].Value : string.Empty;
                                match.oddAwayType = GetOddTypeFromString(dropType);
                            }

                            //get money, vIndex, and suggestion.
                            HtmlAgilityPack.HtmlNode pickDetail = nodeOddInfo[1];
                            if (pickDetail != null)
                            {
                                //get total money
                                HtmlAgilityPack.HtmlNode moneyNode = pickDetail.ChildNodes[0];
                                if (moneyNode != null)
                                    match.totalMoney = Convert.ToDecimal(moneyNode.InnerText.Remove(0, 1)); //remove $ character

                                //get vIndex
                                HtmlAgilityPack.HtmlNode vIndexNode = pickDetail.ChildNodes[1];
                                if (vIndexNode != null)
                                    match.vindex = vIndexNode.InnerText;

                                //get prediction
                                HtmlAgilityPack.HtmlNode predictionNode = pickDetail.ChildNodes[2];
                                if (predictionNode != null)
                                    match.currentPick = predictionNode.InnerText;

                            }
                            
                        }
                       
                        lstMatch.Add(match);
                    }
                }
            }

            //return the list 
            return lstMatch;
        }

        private static int GetOddTypeFromString(string dropType)
        {
            if (dropType.Contains("cellbg_red1"))
                return 1;
            else if (dropType.Contains("cellbg_red2"))
                return 2;
            else if (dropType.Contains("cellbg_red3"))
                return 3;
            else
                return 0;
        }

        private static void GetOddFromString(string[] oddNumbers, ref double startOdd, ref double currentOdd)
        {
            if (oddNumbers != null)
            {
                if (oddNumbers[0] != "-")
                {
                    startOdd = Convert.ToDouble(oddNumbers[0]);
                    currentOdd = Convert.ToDouble(oddNumbers[1]);
                }
                else
                {
                    startOdd = Convert.ToDouble(oddNumbers[1]);
                    currentOdd = Convert.ToDouble(oddNumbers[1]);
                }
            }
        }
    }
}
