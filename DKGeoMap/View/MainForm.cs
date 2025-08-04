using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using DKGeoMap.ViewModels;

namespace DKGeoMap.View
{
    public class MainForm : Form
    {
        private readonly MainViewmodel _viewModel = new MainViewmodel();
        private MapPanel _mapPanel;
        private Button _loadButton;
        private Button _toggleOverlayButton;
        private bool _overlayEnabled = true;

        public MainForm()
        {
            this.Text = "WMS Map Viewer";
            this.Width = 800;
            this.Height = 600;

            _mapPanel = new MapPanel
            {
                Dock = DockStyle.Fill
            };
            _loadButton = new Button
            {
                Text = "Load Map",
                Dock = DockStyle.Top
            };
            _loadButton.Click += async (s, e) => await LoadMapAsync();

            _toggleOverlayButton = new Button
            {
                Text = "Toggle Overlay",
                Dock = DockStyle.Top
            };
            _toggleOverlayButton.Click += async (s, e) =>
            {
                _overlayEnabled = !_overlayEnabled;
                _viewModel.SetOverlayVisibility(_overlayEnabled);
                await LoadMapAsync();
            };

            this.Controls.Add(_mapPanel);
            this.Controls.Add(_toggleOverlayButton);
            this.Controls.Add(_loadButton);
        }

        private async Task LoadMapAsync()
        {
            string wmsUrl = "https://api.dataforsyningen.dk/service?servicename=forvaltning2&service=WMS&version=1.3.0&request=GetMap&token=413317c1dddfc35112064a070b26ddbd&layers=Basis_kort&crs=EPSG:25832&bbox=243259,5935450,994252,6645680&width=4000&height=3200&format=image/png&transparent=FALSE";
            await _viewModel.LoadMapAsync(wmsUrl);
            _mapPanel.SetImage(_viewModel.MapImage);
        }
    }
}

