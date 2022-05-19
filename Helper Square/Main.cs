using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using NAudio;
using NAudio.Wave;
using System.Management;
using System.Diagnostics;

namespace Helper_Square
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        int SlideInPos = 0;
        int SlideOutPos = 431;
        int InitNum = 0;
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;
        string currentState = "HELLO_INTRODUCE";
        string toHelpApp = "NONE";

        bool[] completedList =
        {
            false //Chrome
        };

        private void Main_Load(object sender, EventArgs e)
        {
            Location = new Point(Screen.PrimaryScreen.Bounds.Width - 431, Screen.PrimaryScreen.Bounds.Height - 270);

            this.label1.Text = "Hello! Im your assistant, Helper Square!";
            outputDevice = new WaveOutEvent();

            audioFile = new AudioFileReader("Hello.wav");
            outputDevice.Init(audioFile);

            outputDevice.Play();


            this.Initializer.Start();
            this.SlideInTimer.Start();
        }

        private void SlideInTimer_Tick(object sender, EventArgs e)
        {
            Location = new Point(Location.X - 5, Location.Y);
            SlideInPos += 5;
            SlideOutPos -= 5;

            if (SlideInPos >= 431)
            {
                this.SlideInTimer.Stop();
                this.NextButton.Visible = true;
            }
        }

        private void Initializer_Tick(object sender, EventArgs e)
        {
            Location = new Point(Location.X + 220, Location.Y);
            InitNum += 1;

            if (InitNum >= 2)
            {
                this.Initializer.Stop();
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (currentState == "HELLO_INTRODUCE")
            {
                currentState = "HELLO_INTRODUCE_2";

                this.label1.Text = "I will pop up anytime that you need my help! Especially with new applications and programs!";
            }
            else if (currentState == "HELLO_INTRODUCE_2")
            {
                currentState = "HELLO_INTRODUCE_3";

                this.label1.Text = "You can also configure me to whenever popup or never!";
            }
            else if (currentState == "HELLO_INTRODUCE_3")
            {
                currentState = "HELLO_INTRODUCE_4";

                this.label1.Text = "Anyways, Thanks for using me! And enjoy!";
            }
            else if (currentState == "HELLO_INTRODUCE_4")
            {
                this.label1.Text = "";

                currentState = "IDLE";

                this.SlideOutTimer.Start();
                this.AppOpenListener.Start();
            } else if (currentState == "HELP")
            {
                if (toHelpApp == "Chrome")
                {
                    Location = new Point(0 + Screen.PrimaryScreen.Bounds.Width / 8, 0);

                    label1.Text = "You see this + button? Great! Now click it to open a new tab!";

                    currentState = "HELP_2";
                }
            } else if (currentState == "HELP_2")
            {
                if (toHelpApp == "Chrome")
                {
                    Location = new Point(0 + Screen.PrimaryScreen.Bounds.Width / 2, 25);

                    label1.Text = "Nice! You just opened a new tab! Now type anything on this long bar to search! Don't forget to press Enter!";

                    currentState = "HELP_3";
                }
            } else if (currentState == "HELP_3")
            {
                if (toHelpApp == "Chrome")
                {
                    Location = new Point((0 + Screen.PrimaryScreen.Bounds.Width / 2) - Screen.PrimaryScreen.Bounds.Width / 4, Screen.PrimaryScreen.Bounds.Height / 2);

                    label1.Text = "Now, click on a blue text to open a website to view stuff! That's all for now! Enjoy!";

                    currentState = "HELP_4";
                }
            } else if (currentState == "HELP_4")
            {
                if (toHelpApp == "Chrome")
                {
                    Location = new Point(Screen.PrimaryScreen.Bounds.Width - 431, Screen.PrimaryScreen.Bounds.Height - 270);

                    label1.Text = "";

                    currentState = "IDLE";
                    toHelpApp = "NONE";
                    completedList[0] = true;

                    this.SlideOutTimer.Start();
                }
            }
        }

        private void SlideOutTimer_Tick(object sender, EventArgs e)
        {
            Location = new Point(Location.X + 5, Location.Y);
            SlideOutPos += 5;
            SlideInPos -= 5;

            if (SlideOutPos >= 431)
            {
                this.SlideOutTimer.Stop();
                this.NextButton.Visible = false;
            }
        }

        private void AppOpenListener_Tick(object sender, EventArgs e)
        {
            if (currentState == "IDLE")
            {
                if (Process.GetProcessesByName("chrome").Length > 0)
                {
                    if (completedList[0] == false)
                    {
                        currentState = "HELP";
                        toHelpApp = "Chrome";

                        this.SlideInTimer.Start();
                        outputDevice.Stop();
                        outputDevice.Init(audioFile);
                        outputDevice.Play();
                        this.label1.Text = "Hey there! Look's like you're new to Google Chrome! Let's get a tour!";
                    }
                }
            }
        }
    }
}
