using SharedResources.Models;

namespace ServerSide.Services.Interfaces
{
	public interface IGalleryService
	{
		string CreateNewItem(GalleryItem galleryItem);
		string Update(int IdGalleryItem);
		string Delete(int IdGalleryItem);
		List<GalleryItem> GetAllGalleryItems();
	}
}
