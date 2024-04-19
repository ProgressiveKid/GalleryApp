using SharedResources.Models;

namespace ServerSide.Services.Interfaces
{
	public interface IGalleryService
	{
		string CreateNewItem(GalleryItem galleryItem);
		string UpdateItem(GalleryItem editableGalleryItem);
		string DeleteItem(int idGalleryItem);
		List<GalleryItem> GetAllGalleryItems();
	}
}
