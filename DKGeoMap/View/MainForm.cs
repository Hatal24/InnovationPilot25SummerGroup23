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
        private readonly Panel legendScrollPanel = new Panel();
        private MapPanel _mapPanel;
        private MenuStrip _menuStrip;
        private ToolStripMenuItem _optionsMenu;
        private ToolStripMenuItem _resetMenu;
        private readonly LegendFactory _legendFactory = new LegendFactory();

        // --- Loading overlay controls ---
        private readonly Panel loadingPanel = new Panel();
        private readonly Label loadingLabel = new Label();
        // --- End loading overlay controls ---

        public MainForm()
        {
            this.Text = "WMS Map Viewer";
            this.Width = 1600;
            this.Height = 800;

            _viewModel.SetOverlayVisibility(false);

            _menuStrip = new MenuStrip();
            _optionsMenu = new ToolStripMenuItem("Load Overlay Data Set");
            _resetMenu = new ToolStripMenuItem("Reset Map");

            _resetMenu.Click += async (s, e) => {
                _viewModel.SetOverlayVisibility(false);
                await ShowLoadingWhileAsync(LoadMapAsync);
                UpdateLegendVisibility();
                legendPictureBox.Image = await _legendFactory.GetCombinedLegendsAsync(_viewModel);
            };

            foreach (var overlay in _viewModel.Overlays)
            {
                var overlayMenuItem = MenuItemFactory.CreateOverlayMenuItem(
                    overlay.Name, overlay.IsVisible,
                    async (item, e) => {
                        item.Checked = !item.Checked;
                        overlay.IsVisible = item.Checked;
                        await ShowLoadingWhileAsync(ReloadMapWithOverlaysAsync);
                        UpdateLegendVisibility();
                        legendPictureBox.Image = await _legendFactory.GetCombinedLegendsAsync(_viewModel);
                    }
                );
                _optionsMenu.DropDownItems.Add(overlayMenuItem);
            }

            _menuStrip.Items.Add(_optionsMenu);
            _menuStrip.Items.Add(_resetMenu);

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

            // --- Setup loading overlay ---
            loadingPanel.Dock = DockStyle.Fill;
            loadingPanel.BackColor = Color.FromArgb(128, Color.LightGray);
            loadingPanel.Visible = false;
            loadingPanel.BringToFront();

            loadingLabel.Text = "Loading...";
            loadingLabel.Font = new Font(FontFamily.GenericSansSerif, 18, FontStyle.Bold);
            loadingLabel.AutoSize = true;
            loadingLabel.BackColor = Color.Transparent;
            loadingLabel.ForeColor = Color.Black;
            loadingLabel.Parent = loadingPanel;
            loadingPanel.Controls.Add(loadingLabel);

            // Center the label when the panel is resized
            loadingPanel.Resize += (s, e) =>
            {
                loadingLabel.Left = (loadingPanel.Width - loadingLabel.Width) / 2;
                loadingLabel.Top = (loadingPanel.Height - loadingLabel.Height) / 2;
            };

            this.Controls.Add(loadingPanel);
            // --- End setup loading overlay ---
        }

        private async Task LoadMapAsync()
        {
            string wmsUrl = "https://api.dataforsyningen.dk/service?servicename=forvaltning2&service=WMS&version=1.3.0&request=GetMap&token=413317c1dddfc35112064a070b26ddbd&layers=Basis_kort&crs=EPSG:25832&bbox=243259,5935450,994252,6645680&width=3000&height=2400&format=image/png&transparent=FALSE";
            _viewModel.SetOverlayVisibility(false);
            await ShowLoadingWhileAsync(async () =>
            {
                await _viewModel.LoadMapAsync(wmsUrl);
                _mapPanel.SetImage(_viewModel.MapImage);
            });
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

        // --- Helper to show loading overlay during async operation ---
        private async Task ShowLoadingWhileAsync(Func<Task> asyncAction)
        {
            try
            {
                loadingPanel.Visible = true;
                loadingPanel.BringToFront();
                await Task.Yield(); // Ensure UI updates
                await asyncAction();
            }
            finally
            {
                loadingPanel.Visible = false;
            }
        }
    }
}

