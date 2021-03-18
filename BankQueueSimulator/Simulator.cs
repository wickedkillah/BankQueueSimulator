using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace BankQueueSimulator
{
    /// <summary>
    /// Simulate queue
    /// </summary>
    public class Simulator
    {
        readonly TimeSpan interval = TimeSpan.FromMilliseconds(30);

        Random random;
        int randomWorkTime => random.Next(minTimeToWork, maxTimeToWork);
        int randomArrivalTime => random.Next(minTimeToArrival, maxTimeToArrival);

        int minTimeToWork, maxTimeToWork, minTimeToArrival, maxTimeToArrival;
        MainWindow window;
        Teller[] tellers;
        bool stop;

        TimeSpan timeSinceStart;
        TimeSpan currentTimeToArrival;

        Queue<int> queue;

        int number = 1;

        public Simulator(int tellersCount, int minTimeToWork, int maxTimeToWork, int minTimeToArrival, int maxTimeToArrival, MainWindow window)
        {
            this.window = window;
            this.minTimeToWork = minTimeToWork;
            this.maxTimeToWork = maxTimeToWork;
            this.minTimeToArrival = minTimeToArrival;
            this.maxTimeToArrival = maxTimeToArrival;
            random = new Random();
            timeSinceStart = new TimeSpan();
            currentTimeToArrival = TimeSpan.FromSeconds(randomArrivalTime);
            queue = new Queue<int>();
            tellers = new Teller[tellersCount];
            for (int i = 0; i < tellersCount; i++)
            {
                tellers[i] = new Teller(TimeSpan.FromSeconds(randomWorkTime));
            }
        }

        /// <summary>
        /// Init and start main loop
        /// </summary>
        public async Task Start()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = interval;
            timer.Tick += Update;
            timer.Start();
            while (!stop)
            {
                await Task.Delay(30);
            }
            timer.Tick -= Update;
            timer.Stop();
        }

        public void Stop() => stop = true;

        void Update(object sender, EventArgs e)
        {
            timeSinceStart += interval;
            window.timeSinceStart.Text = timeSinceStart.ToString(@"mm\:ss");

            SpawnClient();
            Work();
            UpdateCurrentQueue();
            UpdateTellersQueue();
        }

        void SpawnClient()
        {
            currentTimeToArrival -= interval;

            if (currentTimeToArrival.TotalSeconds < 0.05)
            {
                currentTimeToArrival = TimeSpan.FromSeconds(randomArrivalTime);
                queue.Enqueue(number++);
            }
        }

        void Work()
        {
            foreach (Teller item in tellers)
            {
                item.currentTimeToWork -= interval;

                if ((item.currentTimeToWork.TotalSeconds < 0.05 || item.currentClient == null) && queue.Count != 0)
                {
                    item.currentClient = queue.Dequeue();
                    item.currentTimeToWork = TimeSpan.FromSeconds(randomWorkTime);
                }
                else if (item.currentTimeToWork.TotalSeconds < 0.05 && item.currentClient != null)
                {
                    item.currentClient = null;
                }
            }
        }

        void UpdateCurrentQueue()
        {
            window.currentQueue.Text = "";
            foreach (var item in queue)
            {
                window.currentQueue.Text += $"{item} ";
            }
        }

        void UpdateTellersQueue()
        {
            window.currentTellersQueue.Text = "";
            foreach (var item in tellers)
            {
                if (item.currentClient != null)
                    window.currentTellersQueue.Text += $"{item.currentClient} ";
                else
                    window.currentTellersQueue.Text += $"X ";
            }
        }

        class Teller
        {
            internal int? currentClient;
            internal TimeSpan currentTimeToWork;


            internal Teller(TimeSpan currentTimeToWork)
            {
                this.currentTimeToWork = currentTimeToWork;
            }
        }
    }
}
