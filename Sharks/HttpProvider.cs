using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sharks
{
    class HttpProvider
    {
        public async Task<string> GetAsync(Uri uri)
        {
            using (var client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(uri))
            {
                if (response.IsSuccessStatusCode)
                {
                    using (HttpContent content = response.Content)
                    {
                        return await content.ReadAsStringAsync();
                    }
                }
                throw new Exception();
            }
        }

        public async Task<string> PostAsync(Uri uri, string content)
        {
            using (var client = new HttpClient())
            using (HttpResponseMessage response = await client.PostAsync(uri, new StringContent(content, Encoding.UTF8, "application/json")))
            {
                return response.IsSuccessStatusCode ? "Success" : "Error";
            }
        }
    }
}
