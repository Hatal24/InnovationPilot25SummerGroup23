using System.Collections.Generic;
using DKGeoMap.Models;

namespace DKGeoMap.ViewModels
{
    public class OverlayFactory
    {
        public static List<OverlayModel> CreateDefaultOverlays()
        {
            return new List<OverlayModel>
            {
                // Peat overlay (compatible with basemap)
                new OverlayModel(
                    "Tørverig lavbund 2024",
                    "https://geodata.fvm.dk/geoserver/ows?service=WMS&version=1.3.0&request=GetMap&layers=Jordbunds_og_terraenforhold:Toerverig_lavbund_2024&bbox=243259,5935450,994252,6645680&width=3000&height=2400&crs=EPSG:25832&format=image/png&transparent=TRUE"
                ),
                // Nitrate retention overlay (compatible with basemap)
                new OverlayModel(
                    "Kvælstofretention",
                    "https://data.geus.dk/arcgis/services/Denmark/Kvaelstofretention/MapServer/WMSServer?service=WMS&version=1.3.0&request=GetMap&layers=Kvaelstofretention&styles=&crs=EPSG:25832&bbox=243259,5935450,994252,6645680&width=3000&height=2400&format=image/png&transparent=TRUE"
                ),
                // Municipal boundaries overlay
                new OverlayModel(
                    "Kommunegrænser",
                    "https://services.datafordeler.dk/MATRIKLEN2/MatGaeldendeOgForeloebigWMS/1.0.0/WMS?username=UFZLDDPIJS&password=DAIdatafordel123&SERVICE=WMS&VERSION=1.3.0&REQUEST=GetMap&LAYERS=Matrikelkommune_Gaeldende&STYLES=Matrikelkommune_gaeldende_outline&CRS=EPSG:25832&BBOX=243259,5935450,994252,6645680&WIDTH=3000&HEIGHT=2400&FORMAT=image/png&TRANSPARENT=TRUE"
                ),

                // Topographic map overlay 1870-1899
                new OverlayModel(
                    "Topografisk kort 1870-1899 (Høje målebordsblade)",
                    "https://api.dataforsyningen.dk/topo20_hoeje_maalebordsblade_DAF?service=WMS&request=GetMap&version=1.3.0&token=413317c1dddfc35112064a070b26ddbd&layers=dtk_hoeje_maalebordsblade&styles=&crs=EPSG:25832&bbox=243259,5935450,994252,6645680&width=3000&height=2400&format=image/png&TRANSPARENT=TRUE"
                ),

                // Topographic map overlay 1901-1971
                new OverlayModel(
                    "Topografisk kort 1901-1971 (Lave målebordsblade)",
                    "https://api.dataforsyningen.dk/topo20_lave_maalebordsblade_DAF?service=WMS&request=GetMap&version=1.3.0&token=413317c1dddfc35112064a070b26ddbd&layers=dtk_lave_maalebordsblade&styles=&crs=EPSG:25832&bbox=243259,5935450,994252,6645680&width=3000&height=2400&format=image/png&TRANSPARENT=TRUE"
                ),

                // Soil type overlay
                new OverlayModel(
                    "Jordartskort uden hav",
                    "https://data.geus.dk/arcgis/services/Denmark/Jordartskort_25000/MapServer/WMSServer?service=WMS&version=1.3.0&request=GetMap&layers=Jordart_25000_uden_hav&styles=&crs=EPSG:25832&bbox=243259,5935450,994252,6645680&width=3000&height=2400&format=image/png&transparent=TRUE"
                ),

                // Coastline types overlay
                new OverlayModel(
                    "Kystlinietyper",
                    "https://data.geus.dk/arcgis/services/Denmark/Jordartskort_25000/MapServer/WMSServer?service=WMS&version=1.3.0&request=GetMap&layers=Jordart_25000_kystlinie&styles=&crs=EPSG:25832&bbox=243259,5935450,994252,6645680&width=3000&height=2400&format=image/png&transparent=TRUE"
                ),

                // Protected areas overlay
                new OverlayModel(
                    "Natura 2000 beskyttede vandområder",
                    "https://miljoegis.mim.dk/wms?servicename=miljoegis-natura2000_wms&SERVICE=WMS&version=1.3.0&request=GetMap&layers=theme-pg-natura_2000_omraader_Natura_2000_omraader_i0&styles=&crs=EPSG:25832&bbox=243259,5935450,994252,6645680&width=3000&height=2400&format=image/png&transparent=TRUE"
                ),

                // Protected land areas overlay
                new OverlayModel(
                    "Natura 2000 beskyttede habitatområder",
                    "https://arealeditering-dist-geo.miljoeportal.dk/geoserver/ows?SERVICE=WMS&SERVICE=WMS&version=1.3.0&request=GetMap&layers=dai:habitat_omr&styles=&crs=EPSG:25832&bbox=243259,5935450,994252,6645680&width=3000&height=2400&format=image/png&transparent=TRUE"
                )
            };
        }
    }
}
