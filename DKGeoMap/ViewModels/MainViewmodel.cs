using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using DKGeoMap.Models;

namespace DKGeoMap.ViewModels
{
    public class MainViewmodel
    {
        public Image MapImage { get; private set; }
        public List<OverlayModel> Overlays { get; } = new List<OverlayModel>();

        public MainViewmodel()
        {
            Overlays.Add(new OverlayModel(
                "Toerverig lavbund 2024",
                "https://geodata.fvm.dk/geoserver/ows?service=WMS&version=1.3.0&request=GetMap&layers=Jordbunds_og_terraenforhold:Toerverig_lavbund_2024&bbox=243259,5935450,994252,6645680&width=4000&height=3200&crs=EPSG:25832&format=image/png&transparent=TRUE"
            ));
        }

        public void SetOverlayVisibility(bool visible)
        {
            foreach (var overlay in Overlays)
                overlay.IsVisible = visible;
        }

        public async Task LoadMapAsync(string baseMapUrl)
        {
            MapImage = await GetMapImageAsync(baseMapUrl);
            foreach (var overlay in Overlays)
            {
                if (overlay.IsVisible)
                {
                    overlay.OverlayImage = await GetMapImageAsync(overlay.WmsUrl);
                    MapImage = OverlayImages(MapImage, overlay.OverlayImage);
                }
            }
        }

        private async Task<Image> GetMapImageAsync(string wmsUrl)
        {
            using var httpClient = new System.Net.Http.HttpClient();
            var imageBytes = await httpClient.GetByteArrayAsync(wmsUrl);
            using var ms = new System.IO.MemoryStream(imageBytes);
            return Image.FromStream(ms);
        }

        private Image OverlayImages(Image baseImage, Image overlayImage)
        {
            Bitmap result = new Bitmap(baseImage.Width, baseImage.Height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(baseImage, 0, 0);
                g.DrawImage(overlayImage, 0, 0);
            }
            return result;
        }
    }
}
