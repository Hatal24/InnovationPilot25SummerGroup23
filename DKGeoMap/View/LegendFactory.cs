using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using DKGeoMap.ViewModels;

namespace DKGeoMap.View
{
    public class LegendFactory
    {
        private readonly Dictionary<string, string> _legendUrls = new()
        {
            { "Tørverig lavbund 2024", "https://geodata.fvm.dk/geoserver/ows?service=WMS&version=1.3.0&request=GetLegendGraphic&format=image/png&width=20&height=20&layer=Jordbunds_og_terraenforhold:Toerverig_lavbund_2024" },
            { "Kvælstofretention", "https://data.geus.dk/arcgis/services/Denmark/Kvaelstofretention/MapServer/WMSServer?request=GetLegendGraphic&version=1.3.0&format=image/png&layer=Kvaelstofretention" },
            { "Kommunegrænser", "https://services.datafordeler.dk/MATRIKLEN2/MatGaeldendeOgForeloebigWMS/1.0.0/WMS?username=UFZLDDPIJS&password=DAIdatafordel123&SERVICE=WMS&VERSION=1.3.0&REQUEST=GetLegendGraphic&LAYER=Matrikelkommune_Gaeldende&STYLE=Matrikelkommune_gaeldende_outline&FORMAT=image/png&SLD_VERSION=1.1.0" }
        };

        public async Task<Image> GetCombinedLegendsAsync(MainViewmodel viewModel)
        {
            var legends = new List<Image>();

            foreach (var overlay in viewModel.Overlays)
            {
                if (overlay.IsVisible && _legendUrls.TryGetValue(overlay.Name, out var legendUrl))
                {
                    var legend = await viewModel.GetLegendImageAsync(legendUrl);
                    if (legend != null)
                        legends.Add(legend);
                }
            }

            if (legends.Count == 0)
                return null;
            if (legends.Count == 1)
                return legends[0];

            int width = legends.Max(img => img.Width);
            int height = legends.Sum(img => img.Height);

            var combined = new Bitmap(width, height);
            using (var g = Graphics.FromImage(combined))
            {
                int y = 0;
                foreach (var img in legends)
                {
                    g.DrawImage(img, 0, y, img.Width, img.Height);
                    y += img.Height;
                }
            }
            // Dispose the individual legend images to avoid memory leaks
            foreach (var img in legends)
            {
                img.Dispose();
            }
            return combined;
        }
    }
}
