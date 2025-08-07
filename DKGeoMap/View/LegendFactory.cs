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
            { "Kommunegrænser", "https://services.datafordeler.dk/MATRIKLEN2/MatGaeldendeOgForeloebigWMS/1.0.0/WMS?username=UFZLDDPIJS&password=DAIdatafordel123&SERVICE=WMS&VERSION=1.3.0&REQUEST=GetLegendGraphic&LAYER=Matrikelkommune_Gaeldende&STYLE=Matrikelkommune_gaeldende_outline&FORMAT=image/png&SLD_VERSION=1.1.0" },
            { "Jordartskort uden hav", "https://data.geus.dk/arcgis/services/Denmark/Jordartskort_25000/MapServer/WMSServer?request=GetLegendGraphic%26version=1.3.0%26format=image/png%26layer=Jordart_25000_uden_hav" },
            { "Kystlinietyper", "https://data.geus.dk/arcgis/services/Denmark/Jordartskort_25000/MapServer/WMSServer?request=GetLegendGraphic%26version=1.3.0%26format=image/png%26layer=Jordart_25000_kystlinie" },
            { "Natura 2000 beskyttede vandområder", "https://miljoegis.mim.dk/wms?servicename=miljoegis-natura2000_wms&version=1.3.0&service=WMS&request=GetLegendGraphic&sld_version=1.1.0&layer=theme-pg-natura_2000_omraader_Natura_2000_omraader_i0&format=image/png&STYLE=default" },
            { "Natura 2000 beskyttede habitatområder", "https://arealeditering-dist-geo.miljoeportal.dk/geoserver/ows?service=WMS&version=1.3.0&request=GetLegendGraphic&format=image%2Fpng&width=20&height=20&layer=dai%3Ahabitat_omr" }
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
