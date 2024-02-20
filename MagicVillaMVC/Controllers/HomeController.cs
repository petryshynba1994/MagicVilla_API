using MagicVillaMVC.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace MagicVillaMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        string BaseUrl = "https://localhost:7140/";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var villas = new List<Villa>();
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/VillaAPI");
                if(Res.IsSuccessStatusCode)
                {
                    var villasResponse = Res.Content.ReadAsStringAsync().Result;
                    villas = JsonConvert.DeserializeObject<List<Villa>>(villasResponse);
                }
            }
            return View(villas);
        }
        public async Task<IActionResult> GetVilla(int id)
        {
            Villa villa = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync($"api/VillaAPI/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var villasResponse = Res.Content.ReadAsStringAsync().Result;
                    villa = JsonConvert.DeserializeObject<Villa>(villasResponse);
                }
            }
            return View(villa);
        }
        [HttpGet]
        public async Task<IActionResult> EditVilla(int id)
        {
            Villa villa = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync($"api/VillaAPI/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var villasResponse = Res.Content.ReadAsStringAsync().Result;
                    villa = JsonConvert.DeserializeObject<Villa>(villasResponse);
                }
            }
            return View(villa);
        }
        [HttpPost]
        public async Task<IActionResult> EditVilla(int id, Villa villa)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Создаем HTTPContent из объекта villa для отправки на сервер
                var content = new StringContent(JsonConvert.SerializeObject(villa), Encoding.UTF8, "application/json");

                HttpResponseMessage Res = await client.PutAsync($"api/VillaAPI/{id}", content); // Используем метод PutAsync для отправки PUT-запроса

                if (Res.IsSuccessStatusCode)
                {
                    // Если успешно, перенаправляем на страницу с деталями виллы
                    return RedirectToAction("Index");
                }
                else
                {
                    // Если есть ошибки, добавляем их в модель и возвращаем представление для редактирования
                    ModelState.AddModelError(string.Empty, "Ошибка при редактировании виллы.");
                    return View(villa);
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.DeleteAsync($"api/VillaAPI/{id}"); // Используем метод PutAsync для отправки PUT-запроса
                if (Res.IsSuccessStatusCode)
                {
                    // Если успешно, перенаправляем на страницу с деталями виллы
                    return RedirectToAction("Index");
                }
                else
                {
                    // Если есть ошибки, добавляем их в модель и возвращаем представление для редактирования
                    ModelState.AddModelError(string.Empty, "Ошибка при редактировании виллы.");
                    return RedirectToAction("Index");
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> CreateVilla()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateVilla(Villa villa)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Создаем HTTPContent из объекта villa для отправки на сервер
                var content = new StringContent(JsonConvert.SerializeObject(villa), Encoding.UTF8, "application/json");

                HttpResponseMessage Res = await client.PostAsync($"api/VillaAPI", content); // Используем метод PutAsync для отправки PUT-запроса

                if (Res.IsSuccessStatusCode)
                {
                    // Если успешно, перенаправляем на страницу с деталями виллы
                    return RedirectToAction("Index");
                }
                else
                {
                    // Если есть ошибки, добавляем их в модель и возвращаем представление для редактирования
                    ModelState.AddModelError(string.Empty, "Ошибка при редактировании виллы.");
                    return View(villa);
                }
            }
        }
        public async Task<IActionResult> PatchVilla(int id)
        {
            Villa villa = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync($"api/VillaAPI/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var villasResponse = Res.Content.ReadAsStringAsync().Result;
                    villa = JsonConvert.DeserializeObject<Villa>(villasResponse);
                }
            }
            return View(villa);
        }
        [HttpPost]
        public async Task<IActionResult> PatchVilla(int id, Villa villa)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Создаем JsonPatchDocument для представления изменений в виде операций патча
                JsonPatchDocument<Villa> patchDocument = new JsonPatchDocument<Villa>();
                patchDocument.Replace(x => x.Name, villa.Name);
                patchDocument.Replace(x => x.Details, villa.Details);
                // Продолжайте для остальных свойств

                // Сериализуем JsonPatchDocument в JSON и создаем контент запроса
                var jsonPatchContent = new StringContent(JsonConvert.SerializeObject(patchDocument), Encoding.UTF8, "application/json-patch+json");

                // Отправляем PATCH-запрос на ваше API
                HttpResponseMessage response = await client.PatchAsync($"api/VillaAPI/{id}", jsonPatchContent);

                if (response.IsSuccessStatusCode)
                {
                    // Если запрос завершен успешно, перенаправляем пользователя на страницу с деталями виллы
                    return RedirectToAction("Index");
                }
                else
                {
                    // Если запрос завершается неудачно, показываем ошибку или возвращаем пользователя обратно на страницу редактирования
                    ModelState.AddModelError(string.Empty, "Ошибка при обновлении виллы.");
                    return View(villa);
                }
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}