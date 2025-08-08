using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net.Http;

namespace DKGeoMap.Models
{
    public class MapModel
    {
        public async Task<Image> GetMapImageAsync(string wmsUrl)
        {
            using var httpClient = new HttpClient();
            var imageBytes = await httpClient.GetByteArrayAsync(wmsUrl);
            using var ms = new System.IO.MemoryStream(imageBytes);
            return Image.FromStream(ms);
        }

        public async Task<Image> GetLegendImageAsync(string legendUrl)
        {
            using var httpClient = new HttpClient();
            var imageBytes = await httpClient.GetByteArrayAsync(legendUrl);
            using var ms = new System.IO.MemoryStream(imageBytes);
            return Image.FromStream(ms);
        }
    }
}
