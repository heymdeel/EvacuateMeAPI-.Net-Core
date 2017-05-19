using EvacuateMe.BLL.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EvacuateMe.BLL.Services
{
    public class GoogleMapsService : IMapService
    {
        private async Task<dynamic> MakeRequestAsync(double lat1, double lon1, double lat2, double lon2)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";

            string key = "AIzaSyCiGqVbAL5HBIqlbAmL8gCWNg7LtgaMh9Q";
            string reqStr = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={lat1.ToString(nfi)},{lon1.ToString(nfi)}&destinations={lat2.ToString(nfi)},{lon2.ToString(nfi)}&key={key}";

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(reqStr);
            HttpWebResponse resp = (HttpWebResponse)await req.GetResponseAsync();

            string jsonResult;
            using (StreamReader stream = new StreamReader(
            resp.GetResponseStream(), Encoding.UTF8))
            {
                jsonResult = stream.ReadToEnd();
            }

            dynamic result = JsonConvert.DeserializeObject(jsonResult);
            return result;
        }

        public async Task<double> GetDistanceAsync(double lat1, double lon1, double lat2, double lon2)
        {
            var result = await MakeRequestAsync(lat1, lon1, lat2, lon2);
            Console.WriteLine(result["rows"][0]["elements"][0]["distance"]["value"]);
            return result["rows"][0]["elements"][0]["distance"]["value"];
        }

        public async Task<string> GetDurationAsync(double lat1, double lon1, double lat2, double lon2)
        {
            var result = await MakeRequestAsync(lat1, lon1, lat2, lon2);
            return result["rows"][0]["elements"][0]["duration"]["text"];
        }
    }
}
