using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Memory;
using ServerSide.Services.Interfaces;
using SharedResources.Models;

namespace ServerSide.Services
{
	public class GalleryManageService : IGalleryService
	{
		private ApplicationContext db;
		private readonly IMemoryCache _cache;
		private readonly string cacheKey = "AllGalleryItems";
		public GalleryManageService(ApplicationContext constructDb, IMemoryCache cache)
		{
			db = constructDb;
			_cache = cache;
		}

		List<GalleryItem> IGalleryService.GetAllGalleryItems()
		{
			if (!_cache.TryGetValue(cacheKey, out List<GalleryItem> cachedItems))
			{
				// Если объект не найден в кэше, получаем данные из репозитория
				cachedItems = db.GalleryItems.ToList();

				// Определяем параметры кэширования
				var cacheEntryOptions = new MemoryCacheEntryOptions()
					.SetAbsoluteExpiration(TimeSpan.FromMinutes(20)); // Например, кэширование на 20 минут

				// Добавляем данные в кэш
				_cache.Set(cacheKey, cachedItems, cacheEntryOptions);
			}

			return cachedItems;
		}

		public string CreateNewItem(GalleryItem newGalleryItem)
		{
			var cachedItems = _cache.Get<List<GalleryItem>>(cacheKey);
			if (cachedItems != null)
			{
				cachedItems.Add(newGalleryItem);
				_cache.Set(cacheKey, cachedItems);
			}

			var result = db.GalleryItems.Add(newGalleryItem);
			db.SaveChanges();
			return $"В системе создан новый элемент с ID = {newGalleryItem.Id}";
		}


		public string DeleteItem(int IdGalleryItem)
		{
			var cachedItems = _cache.Get<List<GalleryItem>>(cacheKey);
			var galleryItem = cachedItems.FirstOrDefault(u => u.Id == IdGalleryItem);

			// Если элемент найден в кэше, удаляем его оттуда
			if (galleryItem != null)
			{
				cachedItems.Remove(galleryItem);
				_cache.Set(cacheKey, cachedItems);
			}

			// Удаляем элемент из базы данных
			var itemToDelete = db.GalleryItems.FirstOrDefault(u => u.Id == IdGalleryItem);
			if (itemToDelete != null)
			{
				db.GalleryItems.Remove(itemToDelete);
				db.SaveChanges();
				return $"Элемент с ID {IdGalleryItem} успешно удален.";
			}
			else
			{
				return $"Элемент с ID {IdGalleryItem} не найден.";
			}
		}




		public string UpdateItem(GalleryItem editableGalleryItem)
		{
			var cachedItems = _cache.Get<List<GalleryItem>>(cacheKey);
			var galleryItem = cachedItems.FirstOrDefault(u => u.Id == editableGalleryItem.Id);

			if (galleryItem != null)
			{
				if (galleryItem.ImageName == editableGalleryItem.ImageName && galleryItem.Image == editableGalleryItem.Image)
				{
					return "Элемент не изменился, обновление не требуется";
				}
				else
				{
					galleryItem.ImageName = editableGalleryItem.ImageName;
					galleryItem.Image = editableGalleryItem.Image;
					_cache.Set(cacheKey, cachedItems);
				}
			}

			var itemToUpdate = db.GalleryItems.FirstOrDefault(u => u.Id == editableGalleryItem.Id);
			if (itemToUpdate != null)
			{
				itemToUpdate.ImageName = editableGalleryItem.ImageName;
				itemToUpdate.Image = editableGalleryItem.Image;
				db.SaveChanges();
				return "Элемент успешно обновлен";
			}
			else
			{
				throw new InvalidOperationException("Элемент не найден.");
			}
		}
	}
}
