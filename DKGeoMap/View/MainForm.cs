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
        private MenuStrip _menuStrip;
        private ToolStripMenuItem _optionsMenu;
        private ToolStripMenuItem _loadMapMenuItem;
        private ToolStripMenuItem _toggleOverlayMenuItem;
        private bool _overlayEnabled = false;

        public MainForm()
        {
            this.Text = "WMS Map Viewer";
            this.Width = 800;
            this.Height = 600;

            // Create menu bar
            _menuStrip = new MenuStrip();
            _optionsMenu = new ToolStripMenuItem("Options");
            _loadMapMenuItem = new ToolStripMenuItem("Load Map");
            /*_toggleOverlayMenuItem = new ToolStripMenuItem("Toggle Overlay")
            {
                CheckOnClick = false // We'll manage the check manually
            };*/


            foreach (var overlay in _viewModel.Overlays)
            {
                var overlayMenuItem = MenuItemFactory.CreateOverlayMenuItem(
                    overlay.Name, overlay.IsVisible,
                    async (item, e) => {
                        item.Checked = !item.Checked;
                        overlay.IsVisible = item.Checked;
                        await ReloadMapWithOverlaysAsync();
                    }
                );
                _optionsMenu.DropDownItems.Add(overlayMenuItem);
            }

            _optionsMenu.DropDownItems.Add(_loadMapMenuItem);
            //_optionsMenu.DropDownItems.Add(_toggleOverlayMenuItem);
            _menuStrip.Items.Add(_optionsMenu);

            _mapPanel = new MapPanel
            {
                Dock = DockStyle.Fill
            };

            this.Controls.Add(_mapPanel);
            this.Controls.Add(_menuStrip);
            this.MainMenuStrip = _menuStrip;
        }

        private async Task LoadMapAsync()
        {
            string wmsUrl = "https://api.dataforsyningen.dk/service?servicename=forvaltning2&service=WMS&version=1.3.0&request=GetMap&token=413317c1dddfc35112064a070b26ddbd&layers=Basis_kort&crs=EPSG:25832&bbox=243259,5935450,994252,6645680&width=3000&height=2400&format=image/png&transparent=FALSE";
            _viewModel.SetOverlayVisibility(false);
            await _viewModel.LoadMapAsync(wmsUrl);
            _mapPanel.SetImage(_viewModel.MapImage);
        }

        private async Task ReloadMapWithOverlaysAsync()
        {
            string baseMapUrl = "https://api.dataforsyningen.dk/service?servicename=forvaltning2&service=WMS&version=1.3.0&request=GetMap&token=413317c1dddfc35112064a070b26ddbd&layers=Basis_kort&crs=EPSG:25832&bbox=243259,5935450,994252,6645680&width=3000&height=2400&format=image/png&transparent=FALSE";
            await _viewModel.LoadMapAsync(baseMapUrl);
            _mapPanel.SetImage(_viewModel.MapImage);
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await LoadMapAsync();
        }
    }
}

