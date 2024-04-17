using Newtonsoft.Json;
using SharedResources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClientSide.Services
{
	public class GalleryService
	{
		private readonly HttpClient _httpClient;

		public GalleryService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<List<GalleryItem>> GetAllGalleryItems()
		{
			try
			{
				HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5258/Gallery/GetAllGalleryItems");
				if (response.IsSuccessStatusCode)
				{
					string responseBody = await response.Content.ReadAsStringAsync();
					List<GalleryItem> galleryItems = JsonConvert.DeserializeObject<List<GalleryItem>>(responseBody);
					return galleryItems;
				}
				else
				{
					Console.WriteLine("Ошибка при выполнении запроса: " + response.StatusCode);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Ошибка: " + ex.Message);
			}
			return null;
		}
	}
}
