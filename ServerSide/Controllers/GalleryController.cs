using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServerSide.Services.Interfaces;
using SharedResources.Models;

namespace ServerSide.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class GalleryController : Controller
	{
		// GET: GalleryController
		private readonly IGalleryService _galleryService;
		public GalleryController(IGalleryService galleryService)
		{
			_galleryService = galleryService;
		}

		[HttpGet("GetAllGalleryItems")]
		public async Task<IActionResult> GetAllGalleryItems()
		{
			//LoggerMethod(order);
			var result = _galleryService.GetAllGalleryItems();
			return Ok(result);
		}

		[HttpPost("CreateGalleryItem")]
		public async Task<IActionResult> CreateGalleryItem(GalleryItem galleryItem)
		{
			//LoggerMethod(order);
			var result = _galleryService.CreateNewItem(galleryItem);
			return Ok(result);
		}

		[HttpGet("sayHay")]
		public string sayHay()
		{
			//LoggerMethod(order);

			return "hello";
		}


	}
}
