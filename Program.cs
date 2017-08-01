using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace FindSteamCustomURLs
{
    class Program
    {
        private static finder m_run;

        static void Main(string[] args)
        {

            if (!File.Exists("found.txt"))
                File.Create("found.txt");

            if (!File.Exists("wordlist.txt"))
                File.Create("wordlist.txt");

            m_run = new finder();

            while ( true )
            {

                Console.Title = "Find steam URLs";
                Thread.Sleep(100);

            }

        }
    }
}
