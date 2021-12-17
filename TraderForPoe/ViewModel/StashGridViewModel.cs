using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using TraderForPoe.Classes;

namespace TraderForPoe.ViewModel
{
    class StashGridViewModel : ViewModelBase
    {
        private static StashGridViewModel instance;

        DispatcherTimer timerPoeLocation = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(500) };

        // Constructor is private, because  there is no need for many instances.
        // An instance can be recieved with the Instance property.
        private StashGridViewModel()
        {
            timerPoeLocation.Tick += TimerPoeLocation_Tick;
            timerPoeLocation.Start();
        }

        private void TimerPoeLocation_Tick(object sender, EventArgs e)
        {
            WinAPI.GetClientRect(Poe.GetHandle(), out WinAPI.RECT clientRect);
            WinAPI.GetWindowRect(Poe.GetHandle(), out WinAPI.RECT windowRect);

            double borderSize = (windowRect.Right - (windowRect.Left + clientRect.Right)) / 2;
            double titleBarSize = (windowRect.Bottom - (windowRect.Top + clientRect.Bottom)) - borderSize;

            double x = windowRect.Left + borderSize;
            double y = windowRect.Top + titleBarSize;
            double h = clientRect.Bottom;
            double w = clientRect.Right;

            WindowLocationTop = y + (h * 0.118);
            WindowHeight = (h * 0.585);
            WindowWidth = (h * 0.585);

            if ((w / h) < 1.35)
			{
				WindowLocationLeft = x + (w * 0.011);
			}

			else if ((w / h) < 1.9)
			{
				WindowLocationLeft = x + (w * 0.0081);
			}

			else if ((w / h) < 1.99)
			{
				WindowLocationLeft = x + (w * 0.008);
			}

			else if ((w / h) < 2.3)
			{
				WindowLocationLeft = x + (w * 0.0075);
			}

			else if ((w / h) < 2.58)
			{
				WindowLocationLeft = x + (w * 0.0065);

			}

			else if ((w / h) < 2.8)
			{
				WindowLocationLeft = x + (w * 0.0055);

			}

			else if ((w / h) < 3.1)
			{
				WindowLocationLeft = x + (w * 0.0055);
			}

			else
			{
				WindowLocationLeft = x + (w * 0.005);
			}
		}

        public static StashGridViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StashGridViewModel();
                }
                return instance;
            }
        }

        private double windowLocationLeft;
        public double WindowLocationLeft
        {
            get { return windowLocationLeft; }
            set
            {
                if (windowLocationLeft != value)
                {
                    windowLocationLeft = value;
                    this.OnPropertyChanged();
                }
            }
        }




        private double windowLocationTop;
        public double WindowLocationTop
        {
            get { return windowLocationTop; }
            set
            {
                if (windowLocationTop != value)
                {
                    windowLocationTop = value;
                    this.OnPropertyChanged();
                }
            }
        }



        private double windowHeight;
        public double WindowHeight
        {
            get { return windowHeight; }
            set
            {
                if (windowHeight != value)
                {
                    windowHeight = value;
                    this.OnPropertyChanged();
                }
            }
        }



        private double windowWidth;
        public double WindowWidth
        {
            get { return windowWidth; }
            set
            {
                if (windowWidth != value)
                {
                    windowWidth = value;
                    this.OnPropertyChanged();
                }
            }
        }
    }
}
