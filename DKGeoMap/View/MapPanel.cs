using System;
using System.Drawing;
using System.Windows.Forms;

namespace DKGeoMap.View
{
    public class MapPanel : UserControl
    {
        private readonly Panel _scrollPanel;
        private readonly PictureBox _pictureBox;
        private readonly Button _zoomInButton;
        private readonly Button _zoomOutButton;
        private float _zoom = 1.0f;
        private Image _originalImage;

        public MapPanel()
        {
            // Set up scrollable panel
            _scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            // Set up picture box
            _pictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(0, 0)
            };
            _scrollPanel.Controls.Add(_pictureBox);

            // Set up zoom buttons (bottom right, absolute positioning)
            _zoomInButton = new Button
            {
                Text = "+",
                Width = 32,
                Height = 32
            };
            _zoomInButton.Click += (s, e) => Zoom(1.2f);

            _zoomOutButton = new Button
            {
                Text = "-",
                Width = 32,
                Height = 32
            };
            _zoomOutButton.Click += (s, e) => Zoom(1 / 1.2f);

            // Add controls
            this.Controls.Add(_scrollPanel);
            this.Controls.Add(_zoomInButton);
            this.Controls.Add(_zoomOutButton);

            // Position buttons in bottom right on resize
            this.Resize += (s, e) => PositionButtons();
            PositionButtons();
        }

        private void PositionButtons()
        {
            int padding = 10;
            int totalHeight = _zoomInButton.Height + _zoomOutButton.Height + padding;
            int x = this.ClientSize.Width - _zoomInButton.Width - padding;
            int yIn = this.ClientSize.Height - totalHeight;
            int yOut = this.ClientSize.Height - _zoomOutButton.Height - padding;

            _zoomInButton.Location = new Point(x, yIn);
            _zoomOutButton.Location = new Point(x, yOut);
            _zoomInButton.BringToFront();
            _zoomOutButton.BringToFront();
        }

        public void SetImage(Image image)
        {
            _originalImage = image;
            _zoom = 1.0f;
            UpdateImage();

            // Center the scrollbars on the image
            CenterScroll();
        }

        private void CenterScroll()
        {
            if (_pictureBox.Image == null) return;

            // Calculate the center position
            int x = Math.Max(0, (_pictureBox.Width - _scrollPanel.ClientSize.Width) / 2);
            int y = Math.Max(0, (_pictureBox.Height - _scrollPanel.ClientSize.Height) / 2);

            _scrollPanel.AutoScrollPosition = new Point(x, y);
        }

        private void Zoom(float factor)
        {
            if (_originalImage == null) return;
            _zoom *= factor;
            if (_zoom < 0.1f) _zoom = 0.1f;
            if (_zoom > 10f) _zoom = 10f;
            UpdateImage();
            CenterScroll();
        }

        private void UpdateImage()
        {
            if (_originalImage == null) return;
            int newWidth = (int)(_originalImage.Width * _zoom);
            int newHeight = (int)(_originalImage.Height * _zoom);
            _pictureBox.Image = new Bitmap(_originalImage, new Size(newWidth, newHeight));
            _pictureBox.Size = new Size(newWidth, newHeight);
        }
    }
}
