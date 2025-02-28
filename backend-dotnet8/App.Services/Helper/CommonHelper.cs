using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace App.Services.Helper
{
    public class CommonHelper
    {
        public static string? GetIPAddress(HttpContext httpContext, bool lan = false)
        {
            var ipAddress = string.Empty;

            ipAddress = Convert.ToString(httpContext.Connection.RemoteIpAddress?.ToString());

            if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ipAddress = Convert.ToString(httpContext.Request.Headers["X-Forwarded-For"]);
            }

            if (string.IsNullOrEmpty(ipAddress) || ipAddress.Trim() == "::1")
            {
                lan = true;
                ipAddress = string.Empty;
            }

            if (lan)
            {
                if (string.IsNullOrEmpty(ipAddress))
                {
                    //This is for Local (LAN) Connected ID Address
                    string stringHostName = Dns.GetHostName();
                    //Get Ip Host Entry
                    IPHostEntry ipHostEntries = Dns.GetHostEntry(stringHostName);
                    System.Net.IPAddress[] arrIpAddress = ipHostEntries.AddressList;

                    try
                    {
                        foreach (IPAddress ipAddressItem in arrIpAddress)
                        {
                            if (ipAddressItem.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                ipAddress = Convert.ToString(ipAddressItem);
                            }
                        }
                    }
                    catch
                    {

                        if (string.IsNullOrEmpty(ipAddress))
                            ipAddress = Convert.ToString(arrIpAddress[arrIpAddress.Length - 1]);
                        try
                        {
                            ipAddress = Convert.ToString(arrIpAddress[0]);
                        }
                        catch
                        {
                            try
                            {
                                arrIpAddress = Dns.GetHostAddresses(stringHostName);
                                ipAddress = Convert.ToString(arrIpAddress[0]);
                            }
                            catch
                            {
                                //local address
                                ipAddress = "127.0.0.1";
                            }
                        }
                    }
                }
            }

            return ipAddress;
        }
        public static string GetComputerName(HttpContext httpContext)
        {
            try
            {
                var ipAddress = GetIPAddress(httpContext);
                var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
                var deviceId = $"{ipAddress}-{userAgent}";
                if (ipAddress != null)
                {
                    IPHostEntry hostEntry = Dns.GetHostEntry(ipAddress);
                    return hostEntry?.HostName ?? string.Empty;
                }
            }
            catch (System.Exception)
            {
                // Handle DNS lookup failure
            }
            return string.Empty;
        }
        public static async Task<string?> GetPostalCode(double latitude, double longitude)
        {
            try
            {
                HttpClient _httpClient = new HttpClient();
                var apiUrl = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={latitude}&lon={longitude}";
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.0.0 Safari/537.36");
                _httpClient.DefaultRequestHeaders.Add("Referer", "http://www.microsoft.com");
                var response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var data = JObject.Parse(content);

                    var address = data["address"];
                    string? postalCode = address?["postcode"]?.ToString();

                    return postalCode;
                }
            }
            catch (System.Exception)
            {
                // Handle DNS lookup failure
            }
            return string.Empty;
        }
        public static async Task<string?> SetPostalCode(HttpContext httpContext, double latitude, double longitude)
        {
            string? postalCode = string.Empty;
            if (String.IsNullOrWhiteSpace(httpContext.Session.GetString("PostalCode")))
            {
                postalCode = await GetPostalCode(latitude, longitude);
                httpContext.Session.SetString("PostalCode", postalCode);
            }
            return postalCode;
        }
    }
}
