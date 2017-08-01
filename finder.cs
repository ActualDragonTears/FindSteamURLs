using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Net;

namespace FindSteamCustomURLs
{
    class finder
    {
        private Thread main_thread;
        private string[] ids;

        public finder()
        {
            // Puts all the words in the list into an array.
            ids = File.ReadAllLines("wordlist.txt"); // Imports the word list into an array

            main_thread = new Thread(work); // Creates a new thread
            main_thread.Start(); // Starts a new thread
        }

        private void log(string text) // Writes a string to the found.txt file
        {
            File.AppendAllText("found.txt", text); // Adds it on from the last entry
        }

        public void work()
        {

            for (int i = 0; i < ids.Length; i++)
            {

                if (i == ids.Length)
                    Console.Write("Finished.");

                string id = ids[i];

                if (id.Contains(" ") || id.Contains("&") || id.Contains(",") || id.Contains(".") || id.Contains("/")) // Checking if it is valid for steams requirements
                    continue;

                if (id.Length < 2) // Minimum 2 characters for the id
                    continue;

                // Adds the string from the array to the default link
                string communityUrl = string.Format("http://steamcommunity.com/id/{0}", id); 
                string groupUrl = string.Format("http://steamcommunity.com/groups/{0}", id);

                using (WebClient wc = new WebClient()) // Creates a new web client for ids
                {
                    wc.DownloadStringCompleted += (sender, e) => { response_for_id(sender, e, id); };
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    wc.DownloadStringAsync(new Uri(communityUrl));
                }

                using (WebClient wc = new WebClient()) // Creates a new web client for groups
                {
                    wc.DownloadStringCompleted += (sender, e) => { response_for_group(sender, e, id); };
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    wc.DownloadStringAsync(new Uri(groupUrl));
                }

            }
        }

        private void response_for_id(object o, DownloadStringCompletedEventArgs e, string steamId)
        {
            string pageSource = e.Result;
            if (pageSource.Contains("The specified profile could not be found."))
            {
                Console.Write("Steam URL found : {0} \n", steamId);
                string temp = string.Format("Found ID : {0} {1}", steamId, Environment.NewLine);
                log(temp);
            }
            else
                return;
        }

        private void response_for_group(object o, DownloadStringCompletedEventArgs e, string steamId)
        {
            string pageSource = e.Result;
            if (pageSource.Contains("No group could be retrieved for the given URL."))
            {
                Console.Write("Group URL found : {0} \n", steamId);
                string temp = string.Format("Found group : {0} {1}", steamId, Environment.NewLine);
                log(temp);
            }
            else
                return;
        }
    }
}
