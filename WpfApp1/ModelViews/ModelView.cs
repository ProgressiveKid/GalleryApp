using ClientSide.Services;
using SharedResources.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SharedResources.ModelViews
{
	public class GalleryItemViewModel : INotifyPropertyChanged
	{
		private readonly GalleryService _galleryService;
		public GalleryItemViewModel(GalleryService galleryService)
		{
			_galleryService = galleryService;
		}
		public ObservableCollection<GalleryItem> GalleryItems { get; } = new ObservableCollection<GalleryItem>();
		public ObservableCollection<string> CollectionImageNames { get; } = new ObservableCollection<string>();

		private int _id;
		public int Id
		{
			get { return _id; }
			set
			{
				if (_id != value)
				{
					_id = value;
					OnPropertyChanged();
				}
			}
		}

		private string _imageName;
		public string ImageName
		{
			get { return _imageName; }
			set
			{
				if (_imageName != value)
				{
					_imageName = value;
					OnPropertyChanged();
				}
			}
		}

		private string _image;
		public string Image
		{
			get { return _image; }
			set
			{
				if (_image != value)
				{
					_image = value;
					OnPropertyChanged();
				}
			}
		}


		public async Task LoadGalleryItems()
		{
			List<GalleryItem> galleryItems = await _galleryService.GetAllGalleryItems();
			if (galleryItems != null)
			{
				GalleryItems.Clear();
				CollectionImageNames.Clear();
				foreach (var item in galleryItems)
				{
					GalleryItems.Add(item);
					CollectionImageNames.Add(item.ImageName);
				}
			}
		}
		// Реализация интерфейса INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
