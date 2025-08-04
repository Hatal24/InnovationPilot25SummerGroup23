using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKGeoMap.Models
{
    public class OverlayModel
    {
        public string Name { get; set; }              // Display name for the overlay
        public string WmsUrl { get; set; }            // Full WMS GetMap request URL
        public bool IsVisible { get; set; } = true;   // Whether the overlay is shown
        public Image OverlayImage { get; set; }       // Cached image (optional)

        public OverlayModel(string name, string wmsUrl)
        {
            Name = name;
            WmsUrl = wmsUrl;
        }
    }
}
