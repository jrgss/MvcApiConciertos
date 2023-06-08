using MvcApiConciertos.Helpers;
using MvcApiConciertos.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MvcApiConciertos.Services
{
    public class ServiceConcierto
    {
        private MediaTypeWithQualityHeaderValue Header;
        private string UrlApi;
        public ServiceConcierto(IConfiguration configuration)
        {
            this.Header =
            new MediaTypeWithQualityHeaderValue("application/json");
            //this.UrlApi =
            //    configuration.GetValue<string>("ApiUrls:ApiConcierto");
            string json = HelperSecretManager.GetSecret().Result;
            KeysModel model=JsonConvert.DeserializeObject<KeysModel>(json);
            this.UrlApi = model.Api;

        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                string url = this.UrlApi + request;
                HttpResponseMessage response =
                    await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }
        public async Task<List<CategoriaEvento>> GetCategoriasAsync()
        {
            string request = "/api/categoria";
            List<CategoriaEvento> categorias= await CallApiAsync<List<CategoriaEvento>>(request);
            return categorias;
        }
         public async Task<List<Evento>> GetEventosAsync()
        {
            string request = "/api/evento";
            List<Evento> eventos= await CallApiAsync<List<Evento>>(request);
            return eventos;
        } 
        public async Task<List<Evento>> GetEventosCategoriaAsync(int id)
        {
            string request = "/api/evento/"+id;
            List<Evento> eventos= await CallApiAsync<List<Evento>>(request);
            return eventos;
        }
        public async Task CreateEvento(Evento evento)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/evento";
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                string jsonEvento = JsonConvert.SerializeObject(evento);
                StringContent content =
                new StringContent(jsonEvento, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(this.UrlApi + request, content);
            }
        }

    }
}
