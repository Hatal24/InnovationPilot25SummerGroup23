using System;
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
            // Existing overlay
            Overlays.Add(new OverlayModel(
                "Tørverig lavbund 2024",
                "https://geodata.fvm.dk/geoserver/ows?service=WMS&version=1.3.0&request=GetMap&layers=Jordbunds_og_terraenforhold:Toerverig_lavbund_2024&bbox=243259,5935450,994252,6645680&width=3000&height=2400&crs=EPSG:25832&format=image/png&transparent=TRUE"
            ));

            // Nitrate retention overlay (compatible with basemap)
            Overlays.Add(new OverlayModel(
                "Kvælstofretention",
                "https://data.geus.dk/arcgis/services/Denmark/Kvaelstofretention/MapServer/WMSServer?service=WMS&version=1.3.0&request=GetMap&layers=Kvaelstofretention&styles=&crs=EPSG:25832&bbox=243259,5935450,994252,6645680&width=3000&height=2400&format=image/png&transparent=TRUE"
            ));

            //Municipal boundaries overlay
            Overlays.Add(new OverlayModel(
                "Kommunegrænser",
                "https://services.datafordeler.dk/MATRIKLEN2/MatGaeldendeOgForeloebigWMS/1.0.0/WMS?username=UFZLDDPIJS&password=DAIdatafordel123&SERVICE=WMS&VERSION=1.3.0&REQUEST=GetMap&LAYERS=Matrikelkommune_Gaeldende&STYLES=Matrikelkommune_gaeldende_outline&CRS=EPSG:25832&BBOX=243259,5935450,994252,6645680&WIDTH=3000&HEIGHT=2400&FORMAT=image/png&TRANSPARENT=TRUE"
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

            var visibleOverlays = new List<Image>();
            foreach (var overlay in Overlays)
            {
                if (overlay.IsVisible)
                {
                    overlay.OverlayImage = await GetMapImageAsync(overlay.WmsUrl);
                    if (overlay.OverlayImage != null)
                        visibleOverlays.Add(overlay.OverlayImage);
                }
            }

            if (visibleOverlays.Count > 0)
                MapImage = OverlayImages(MapImage, visibleOverlays.ToArray());
        }

        private async Task<Image> GetMapImageAsync(string wmsUrl)
        {
            using var httpClient = new System.Net.Http.HttpClient();
            try
            {
                var imageBytes = await httpClient.GetByteArrayAsync(wmsUrl);
                using var ms = new System.IO.MemoryStream(imageBytes);
                return Image.FromStream(ms);
            }
            catch (Exception ex)
            {
                // Log or handle the error as needed
                System.Diagnostics.Debug.WriteLine($"Failed to load image from {wmsUrl}: {ex.Message}");
                return null;
            }
        }

        public async Task<Image> GetLegendImageAsync(string legendUrl)
        {
            using var httpClient = new System.Net.Http.HttpClient();
            var imageBytes = await httpClient.GetByteArrayAsync(legendUrl);
            using var ms = new System.IO.MemoryStream(imageBytes);
            return Image.FromStream(ms);
        }

        private Image OverlayImages(Image baseImage, params Image[] overlayImages)
        {
            Bitmap result = new Bitmap(baseImage.Width, baseImage.Height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(baseImage, 0, 0);

                for (int i = 0; i < overlayImages.Length; i++)
                {
                    var overlay = overlayImages[i];
                    // Apply 50% opacity only to the nitrate retention overlay (assumed to be the second overlay)
                    if (i == 1) // index 1: nitrate retention overlay
                    {
                        var colorMatrix = new System.Drawing.Imaging.ColorMatrix
                        {
                            Matrix33 = 0.3f // 50% opacity
                        };
                        var imageAttributes = new System.Drawing.Imaging.ImageAttributes();
                        imageAttributes.SetColorMatrix(colorMatrix, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Bitmap);

                        g.DrawImage(
                            overlay,
                            new Rectangle(0, 0, result.Width, result.Height),
                            0, 0, overlay.Width, overlay.Height,
                            GraphicsUnit.Pixel,
                            imageAttributes
                        );
                    }
                    else
                    {
                        g.DrawImage(overlay, 0, 0);
                    }
                }
            }
            return result;
        }
    }
}
