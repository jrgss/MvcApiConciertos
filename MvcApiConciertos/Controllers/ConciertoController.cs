using Microsoft.AspNetCore.Mvc;
using MvcApiConciertos.Helpers;
using MvcApiConciertos.Models;
using MvcApiConciertos.Services;
using Newtonsoft.Json;

namespace MvcApiConciertos.Controllers
{
    public class ConciertoController : Controller
    {
        private ServiceConcierto service;
        private string BucketUrl;
        private ServiceStorageS3 s3service;
        public ConciertoController(ServiceConcierto service,ServiceStorageS3 s3service)
        {
            this.service = service;
            this.s3service = s3service;
            
        }

        public async Task<IActionResult> Categorias()
        {
            List<CategoriaEvento> categorias = await this.service.GetCategoriasAsync();
            return View(categorias);
        }
        //[HttpGet]
        public async Task<IActionResult> Eventos()
        {
            string jsonSecreto= await HelperSecretManager.GetSecret();
            KeysModel modelo = JsonConvert.DeserializeObject<KeysModel>(jsonSecreto);
            ViewData["BUCKETURL"] = modelo.S3Bucket;
            List<Evento> eventos = await this.service.GetEventosAsync();
            return View(eventos);
        }
        //[HttpPost]
         public async Task<IActionResult> EventosCategoria(int id)
        {
            string jsonSecreto = await HelperSecretManager.GetSecret();
            KeysModel modelo = JsonConvert.DeserializeObject<KeysModel>(jsonSecreto);
            ViewData["BUCKETURL"] = modelo.S3Bucket;
            List<Evento> eventos = await this.service.GetEventosCategoriaAsync(id);
            return View(eventos);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Create(string nombre,string artista,int idcategoria, IFormFile file)
        {
            Evento e = new Evento
            {
                IdEvento = 0,
                Nombre = nombre,
                Artista = artista,
                IdCategoria = idcategoria,
                Imagen = file.FileName
            };
            await this.service.CreateEvento(e);
            using (Stream stream = file.OpenReadStream())
            {
                await this.s3service.UploadFileAsync(file.FileName, stream);
            }
           
            return RedirectToAction("Eventos");
        }

    }
}
