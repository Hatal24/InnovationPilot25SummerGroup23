using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKGeoMap.Models;

namespace DKGeoMap.ViewModels
{
    internal class MainViewmodel
    {
        private readonly MapModel _mapModel = new MapModel();

        public Image MapImage { get; private set; }

        public async Task LoadMapAsync(string wmsUrl)
        {
            MapImage = await _mapModel.GetMapImageAsync(wmsUrl);
        }
    }
}
