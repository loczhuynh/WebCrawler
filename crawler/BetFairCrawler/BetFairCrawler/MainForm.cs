using BetFairCrawler.BL;
using BetFairCrawler.DL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetFairCrawler
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            this.crawlerTimer.Start();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            BetFairController.TheIntance.CrawBetFair();
        }

        private void crawlerTimer_Tick(object sender, EventArgs e)
        {
            //auto call CrawBerFair every 1000ms
            //MessageBox.Show("Start Crawling Bet-Mate.co");
            BetFairController.TheIntance.CrawBetFair();
        }

        //private static void OpenWebsite(string url)
        //{
        //    WebBrowser wb = new WebBrowser();
        //    wb.Navigate(url);
        //    string pageHTML = "";
        //    wb.DocumentCompleted += (sender, e) => pageHTML = wb.DocumentText;

        //}

        //public void DownloadFile(Uri uri, string destination)
        //{
        //    using (var wc = new WebClient())
        //    {
        //        wc.DownloadFile(uri, destination);

        //        //wc.DownloadProgressChanged += HandleDownloadProgress;
        //        //wc.DownloadFileCompleted += HandleDownloadComplete;

        //        //var syncObj = new Object();
        //        //lock (syncObj)
        //        //{
        //        //    wc.DownloadFile(uri, destination);
        //        //    //This would block the thread until download completes
        //        //    Monitor.Wait(syncObj);
        //        //}
        //    }

        //    //Do more stuff after download was complete
        //}

        //public void HandleDownloadComplete(object sender, AsyncCompletedEventArgs e)
        //{
        //    lock (e.UserState)
        //    {
        //        //releases blocked thread
        //        Monitor.Pulse(e.UserState);
        //    }
        //}


        //public void HandleDownloadProgress(object sender, DownloadProgressChangedEventArgs args)
        //{
        //    //Process progress updates here
        //    Console.Write("i am here");
        //}

        //private void CrawBetMate(string urlAddress)
        //{
        //    if (string.IsNullOrEmpty(urlAddress))
        //        return;
          
        //    //create a webclient object that can hold a dom documents for an url address
        //    Uri url = new Uri(urlAddress);
        //    WebClient client = new WebClient();
        //    client.Encoding = Encoding.UTF8;

        //    //start downloading cnn tech on different thread 
        //    client.DownloadStringAsync(url);
        //    client.DownloadStringCompleted += (sender, e) =>
        //    {
        //        if (e.Error == null)
        //        {
        //            var data = e.Result;
        //            //get all cnn head line of a cnn document which is existed in data variable.
        //            List<MatchInfo> lstMatch = CrawlerController.GetAllMatchInfo(data);

        //            if (lstMatch == null)
        //                return;

                   
        //            //done and have a list of coupons, need to update url for top 50 coupons
        //            MessageBox.Show("Done. Successfully crawl: " + urlAddress);
        //        }
        //    };
        //}

        
    }
}
