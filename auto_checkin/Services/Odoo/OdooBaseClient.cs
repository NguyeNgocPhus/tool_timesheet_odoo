using System.Diagnostics;
using System.Net;
using System.Text;
using System.Web;

namespace auto_checkin.Services.Odoo
{
    public class OdooBaseClient
    {
        protected string sendHttpGetRequest(string endpoint, Dictionary<string, dynamic> param, Dictionary<string, string> header)
        {
            UriBuilder builder = new UriBuilder(endpoint);
            var query = HttpUtility.ParseQueryString(builder.Query);
            if (param != null)
            {
                foreach (KeyValuePair<string, dynamic> entry in param)
                {
                    if (entry.Value is string)
                    {
                        query[entry.Key] = entry.Value;
                    }
                }
            }
            builder.Query = query.ToString();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpClient httpClient = new HttpClient();
            if (header != null)
            {
                foreach (KeyValuePair<string, string> entry in header)
                {
                    httpClient.DefaultRequestHeaders.Add(entry.Key, entry.Value);
                }
            }

            return httpClient.GetStringAsync(builder.ToString()).Result;
        }

        protected string sendHttpPostRequestWithBody(string endpoint, Dictionary<string, dynamic> param, string body, Dictionary<string, string> header)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            HttpClient httpClient = new HttpClient(handler);

            if (header != null)
            {
                foreach (KeyValuePair<string, string> entry in header)
                {
                    httpClient.DefaultRequestHeaders.Add(entry.Key, entry.Value);

                }
            }

            UriBuilder builder = new UriBuilder(endpoint);


            var query = HttpUtility.ParseQueryString(builder.Query);
            if (param != null)
            {
                foreach (KeyValuePair<string, dynamic> entry in param)
                {
                    if (entry.Value is string && !entry.Key.Equals("body"))
                    {
                        query[entry.Key] = entry.Value;
                    }
                    if (entry.Value is string && entry.Key.Equals("session_id"))
                    {
                        cookieContainer.Add(new Uri(endpoint), new Cookie("session_id", entry.Value));
                    }
                }

            }
            builder.Query = query.ToString();

            if (body == null)
            {
                body = "";
            }
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            HttpResponseMessage response = httpClient.PostAsync(builder.ToString(), content).Result;
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
