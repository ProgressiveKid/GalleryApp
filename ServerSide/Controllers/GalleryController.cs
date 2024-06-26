﻿using Microsoft.AspNetCore.Http;
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
			string createdId = _galleryService.CreateNewItem(galleryItem);
			return Ok($"В системе создан новый элемент с ID = {createdId}");
		}
		[HttpPut("UpdateItem")]
		public async Task<IActionResult> UpdateItem(GalleryItem galleryItem)
		{
			//LoggerMethod(order);
			var result = _galleryService.UpdateItem(galleryItem);
			return Ok(result);
		}
		[HttpPut("DeleteItem")]
		public async Task<IActionResult> DeleteItem([FromBody] int idGalleryItem)
		{
			//LoggerMethod(order);
			var result = _galleryService.DeleteItem(idGalleryItem);
			return Ok(result);
		}
	}
}
