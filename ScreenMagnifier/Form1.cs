using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;




namespace ScreenMagnifier
{
    public partial class Form1 : Form
    {
        private Timer timer;
        private Bitmap zoomedCapture;
        private Point lastCursorPosition;

        public Form1()
        {
            InitializeComponent();
            this.TopMost = true;
            // Initialize the timer
            timer = new Timer();
            timer.Interval = 100; // Adjust the interval as needed
            timer.Tick += Timer_Tick;
            timer.Start();


        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Capture the screen around the mouse cursor
            Point mousePosition = Cursor.Position;
            Rectangle captureRect = new Rectangle(
                mousePosition.X - (pictureBox.Width / 2),
                mousePosition.Y - (pictureBox.Height / 2),
                pictureBox.Width,
                pictureBox.Height);

            // Only capture if the cursor position has changed
            if (mousePosition != lastCursorPosition)
            {
                Bitmap screenCapture = CaptureScreen(captureRect);

                // Zoom the captured screen image
                if (screenCapture != null)
                {
                    zoomedCapture?.Dispose();
                    zoomedCapture = ZoomImage(screenCapture, 3); // 3X zoom

                    // Update the PictureBox
                    pictureBox.Image?.Dispose();
                    pictureBox.Image = zoomedCapture;
                }
            }

            lastCursorPosition = mousePosition;
        }

        private Bitmap CaptureScreen(Rectangle captureRect)
        {
            try
            {
                Bitmap screenshot = new Bitmap(captureRect.Width, captureRect.Height);

                using (Graphics g = Graphics.FromImage(screenshot))
                {
                    g.CopyFromScreen(captureRect.Location, Point.Empty, captureRect.Size);
                }

                return screenshot;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error capturing the screen: " + ex.Message);
                return null;
            }
        }

        private Bitmap ZoomImage(Bitmap image, int zoomFactor)
        {
            int newWidth = image.Width * zoomFactor;
            int newHeight = image.Height * zoomFactor;

            Bitmap zoomedImage = new Bitmap(newWidth, newHeight);

            using (Graphics g = Graphics.FromImage(zoomedImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImage(image, new Rectangle(0, 0, newWidth, newHeight));
            }

            return zoomedImage;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}
