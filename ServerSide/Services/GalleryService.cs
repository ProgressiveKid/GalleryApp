using Microsoft.AspNetCore.Http.HttpResults;
using ServerSide.Services.Interfaces;
using SharedResources.Models;

namespace ServerSide.Services
{
	public class GalleryManageService : IGalleryService
	{
		//private DBNull ApplicationContext;
		private ApplicationContext db;
		public GalleryManageService(ApplicationContext constructDb)
		{
			db = constructDb;
		}
		string IGalleryService.CreateNewItem(GalleryItem newGalleryItem)
		{
			var a = db.GalleryItems.ToList();
			db.GalleryItems.Add(newGalleryItem);
			db.SaveChanges();
			return $"В системе создан новый элемнет с ID = {newGalleryItem.Id}";
		}

		string IGalleryService.Delete(int IdGalleryItem)
		{
			throw new NotImplementedException();
		}

		List<GalleryItem> IGalleryService.GetAllGalleryItems() => db.GalleryItems.ToList(); 


		string IGalleryService.Update(int IdGalleryItem)
		{
			throw new NotImplementedException();
		}
	}
}
