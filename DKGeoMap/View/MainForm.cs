using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DKGeoMap.ViewModels;


namespace DKGeoMap.View
{
    public class MainForm : Form
    {
        private readonly MainViewmodel _viewModel = new MainViewmodel();
        private readonly PictureBox legendPictureBox = new PictureBox();
        private readonly Panel legendScrollPanel = new Panel(); // Add this line
        private MapPanel _mapPanel;
        private MenuStrip _menuStrip;
        private ToolStripMenuItem _optionsMenu;
        private readonly LegendFactory _legendFactory = new LegendFactory();

        public MainForm()
        {
            this.Text = "WMS Map Viewer";
            this.Width = 1600;
            this.Height = 800;

            // Ensure overlays are not visible before creating menu items
            _viewModel.SetOverlayVisibility(false);

            // Create menu bar
            _menuStrip = new MenuStrip();
            _optionsMenu = new ToolStripMenuItem("Load Overlay Data Set");

            foreach (var overlay in _viewModel.Overlays)
            {
                var overlayMenuItem = MenuItemFactory.CreateOverlayMenuItem(
                    overlay.Name, overlay.IsVisible, // IsVisible is false here
                    async (item, e) => {
                        item.Checked = !item.Checked;
                        overlay.IsVisible = item.Checked;
                        await ReloadMapWithOverlaysAsync();
                        UpdateLegendVisibility();

                        legendPictureBox.Image = await _legendFactory.GetCombinedLegendsAsync(_viewModel);
                    }
                );
                _optionsMenu.DropDownItems.Add(overlayMenuItem);
            }

            _menuStrip.Items.Add(_optionsMenu);

            _mapPanel = new MapPanel
            {
                Dock = DockStyle.Fill
            };

            this.Controls.Add(_mapPanel);
            this.Controls.Add(_menuStrip);
            this.MainMenuStrip = _menuStrip;

            legendPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            legendScrollPanel.Dock = DockStyle.Right;
            legendScrollPanel.Width = 250;
            legendScrollPanel.AutoScroll = true;
            legendScrollPanel.Controls.Add(legendPictureBox);
            this.Controls.Add(legendScrollPanel);

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
            legendPictureBox.Image = await _legendFactory.GetCombinedLegendsAsync(_viewModel);
            UpdateLegendVisibility();
        }


        private void UpdateLegendVisibility()
        {
            legendPictureBox.Visible = _viewModel.Overlays.Any(o => o.IsVisible);
        }
    }
}

