using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;

namespace HW11
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }




      // Delegate to update the form once the thread is done. 
        private delegate void DisplayParallelDelegate(List<int> i);
        

        /// <summary>
        /// Start parralel sort.
        /// </summary>
        private void StartParallel()
        {
            List<int> list = new List<int>();
            textBox1.Invoke(new DisplayParallelDelegate(DisplayParallel), list);
        }

        /// <summary>
        /// Updates the form.
        /// </summary>
        /// <param name="OKlist"></param>
        private void DisplayParallel(List<int> OKlist)
        {
            // sort the display time
            DateTime startTime1 = DateTime.Now;

            OKlist.Sort();

            // Time junk
            DateTime EndTime = DateTime.Now;
            TimeSpan span = EndTime - startTime1;
            int time = (int)span.TotalMilliseconds;
            textBox1.Text += "Sort: " + time.ToString();

        }

        /// <summary>
        /// Delegate to renable the button when done. 
        /// </summary>
        private delegate void EnableButtonDelegate(); // in startcounting

        private void EnableButton()
        {
            button1.Enabled = true;
        }

        /// <summary>
        /// Start the LHS of the form sorting.
        /// </summary>
        private void StartSort()
        {
            DateTime start1 = DateTime.Now;

            List<List<int>> eightLists = generateRandomLists();

            foreach (List<int> list in eightLists)
            {
                list.Sort();
            }

            DateTime stop1 = DateTime.Now;
            TimeSpan span1 = stop1 - start1;
            string time1 = span1.TotalMilliseconds.ToString();


            // SortOne generates a list w/ 1 mil. elements then sorts it. 
            List<Thread> threadList = new List<Thread>();
            for (int i = 0; i < 8; i++)
            {
                var thread = new Thread(SortOne);
                threadList.Add(thread);
                thread.Start();
            }

            // For readability of while loop. 
            bool running = true;

            // Date time to capture time for parallel sort
            DateTime start2 = DateTime.Now;

            while (running == true)
            {
                int numAlive = 8;
                foreach (Thread thread in threadList)
                {
                    if (!thread.IsAlive)
                    {
                        numAlive -= 1;
                    }
                }

                // All threads are done sorting and break out of the loop
                if (numAlive == 0)
                {
                    break;
                }
            }


            DateTime stop2 = DateTime.Now;
            TimeSpan span2 = stop2 - stop1;
            string timeParallel = span2.TotalMilliseconds.ToString();
            button1.Invoke(new EnableButtonDelegate(EnableButton));
            textBox1.Invoke(new DisplayParallelTimeDelegate(DisplayTimes), time1, timeParallel);
        }

        private delegate void DisplayParallelTimeDelegate(string serialSortTime, string parallelSortTime);
        private void DisplayTimes(string serialSortTime, string parallelSortTime)
        {
            textBox1.Text = "The serial sort time was: " + serialSortTime + "\n" + " The parallel sort time was: " + parallelSortTime;
        }
        private void SortOne()
        {
            Random rand = new Random();
            List<int> listToSort = new List<int>();
            for (int i = 0; i < 999999; i++)
            {
                listToSort.Add(rand.Next());
            }

            // Have a thread sort this random list of size 1,000,000
            // No update from single thread.
            listToSort.Sort();
        }

        /// <summary>
        /// Starts the left hand of the form, sorts a list serialy then in parallel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            // Start the thread to sort 
            var thread1 = new Thread(StartSort);
            thread1.IsBackground = true;
            thread1.Start();
            button1.Enabled = false;

        }

        /// <summary>
        /// Generate eight random lists.
        /// </summary>
        /// <returns> list of eight lists.</returns>
        private List<List<int>> generateRandomLists()
        {
            Random rand = new Random();
            List<List<int>> list = new List<List<int>>();


            for (int i = 0; i < 8; i++)
            {
                list.Add(new List<int>());

                for (int k = 0; k < 999999; k++)
                {
                    list[i].Add(rand.Next());
                }
            }

            return list;


        }

        private int Sort(List<int> list)
        {
            DateTime start = DateTime.Now;

            list.Sort();

            DateTime stop = DateTime.Now;
            TimeSpan span = stop - start;
            int ms = (int)span.TotalMilliseconds;
            return ms;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// Starts the download of the website html.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click_1(object sender, EventArgs e)
        {


            var thread = new Thread(StartDownload);
            thread.IsBackground = true;
            thread.Start();
            button2.Enabled = false;


        }


        /// <summary>
        /// Function for thread constructor.
        /// </summary>
        private void StartDownload()
        {
            WebClient client = new WebClient();

            // Add a user agent header in case the 
            // requested URI contains a query.

            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            string site = textBox2.Text;
            if (site.StartsWith("http"))
            {
                string s = client.DownloadString(site);
                textBox3.Invoke(new DisplayDownloadDelegate(DisplayDownload), s);
            }

            button2.Invoke(new EnableButtonDelegate(EnableButton2));
        }

        /// <summary>
        /// Enable button 2.
        /// </summary>
        private void EnableButton2()
        {
            button2.Enabled = true;
        }

        // delegate to display the html string
        private delegate void DisplayDownloadDelegate(string s);

        // Used in invoke in StartDownload. 
        private void DisplayDownload(string i)
        {
            textBox3.Text = i;
        }


        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        
    }
}



