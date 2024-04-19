using ClientSide.ModelViews;
using Microsoft.Win32;
using Newtonsoft.Json;
using SharedResources.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using ClientSide.Functional;
using System.Linq;
using System.Globalization;
using System.Windows.Data;
using Newtonsoft.Json.Linq;
using System.IO;
namespace WpfApp1.ModelViews
{
	public class GalleryViewModel : INotifyPropertyChanged
	{
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
		/// <summary>
		/// Текст для изображения
		/// </summary>
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
		/// <summary>
		/// Изображение 
		/// </summary>
		private BitmapImage _image;
		public BitmapImage Image
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
		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}



		public class GalleryItemViewModel : INotifyPropertyChanged
	{
		public GalleryItemViewModel()
		{
		}
		/// <summary>
		/// Коллекция получаемых с сервера изображении и их описания
		/// </summary>
		private ObservableCollection<GalleryItem> _galleryItems;
		public ObservableCollection<GalleryItem> GalleryItems
		{
			get { return _galleryItems; }
			set
			{
				_galleryItems = value;
				OnPropertyChanged(); // Оповещаем об изменении данных
			}
		}
		/// <summary>
		/// Коллекция строк {id.названием_изображения} для ComboBox
		/// </summary>
		public ObservableCollection<string> CollectionImageNames { get; } = new ObservableCollection<string>();
		#region Binding_Свойства
		/// <summary>
		/// Id полученого изображения
		/// </summary>
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
		/// <summary>
		/// Текст для изображения
		/// </summary>
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
		/// <summary>
		/// Свойства для combox с выбором картинок
		/// </summary>
		private string _selectedImageName;
		public string SelectedImageName
		{
			get { return _selectedImageName; }
			set
			{
				if (_selectedImageName != value)
				{
					_selectedImageName = value;
					OnPropertyChanged(nameof(SelectedImageName));
					UpdateSelectedImage();
				}
			}
		}
		/// <summary>
		/// Метод который мы повесили на combobox для обработки выбора картины
		/// </summary>
		private void UpdateSelectedImage()
		{
			if (!string.IsNullOrEmpty(_selectedImageName))
			{
				int idItem = Convert.ToInt32(_selectedImageName.Split('.')[0]);
				string valueItem = string.Join(".", _selectedImageName.Split('.').Skip(1));
				GalleryItem foundItem = GalleryItems.FirstOrDefault(item => item.Id == idItem && item.ImageName == valueItem);
				if (foundItem != null)
				{
					Id = idItem;
					Image = ImageHandler.FromStringToBitMap(foundItem.Image);
					if (_selectedImageName.Contains(Id.ToString()))
					{
						int index = _selectedImageName.LastIndexOf(Id.ToString());
						if (index != -1 && index < _selectedImageName.Length - 1)
						{
							_selectedImageName = _selectedImageName.Substring(index + Id.ToString().Length + 1);
							ImageName = _selectedImageName;
							OnPropertyChanged(nameof(SelectedImageName));
							OnPropertyChanged(nameof(ImageName));
						}
					}
					OnPropertyChanged(nameof(Image));
				}
			}
		}
		/// <summary>
		/// Ограничение допустимое длины для текса под картинкой
		/// </summary>
		private const int MaxImageNameLength = 50;
		/// <summary>
		/// Максимальный объем загружаемого изображения
		/// </summary>
		private const int MaxImageSizeMB = 10;
		/// <summary>
		/// Изображение 
		/// </summary>
		private BitmapImage _image;
		public BitmapImage Image
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
		/// <summary>
		/// Свойства, которое указывает на включение анимации за
		/// </summary>
		private bool _isActiveLoad = false;//Visible
		public bool IsActiveLoad
		{
			get { return _isActiveLoad; }
			set
			{
				if (_isActiveLoad != value)
				{
					_isActiveLoad = value;
					OnPropertyChanged();
					if (_isActiveLoad)
						IsActiveTabControl = false;
					if (!_isActiveLoad)
						IsActiveTabControl = true;
				}
			}
		}
		/// <summary>
		/// Свойства, которое указывает на блокирование визуального интерфейса во время запросов на сервер
		/// </summary>
		private bool _isActiveTabControl = true;
		public bool IsActiveTabControl
		{
			get { return _isActiveTabControl; }
			set
			{
				if (_isActiveTabControl != value)
				{
					_isActiveTabControl = value;
					OnPropertyChanged();
				}
			}
		}
		/// <summary>
		/// Свойства, которое указывает на доступность для редактирования
		/// </summary>
		private bool _isEditable = false;//Visible
		public bool IsEditable
		{
			get { return _isEditable; }
			set
			{
				if (_isEditable != value)
				{
					_isEditable = value;
					OnPropertyChanged();
					if (_isEditable)
						IsLabelActive = false;
					if (!_isEditable)
						IsLabelActive = true;
					//IsActiveTabControl = !IsActiveTabControl; // меняем на противоположную активную секцию
				}
			}
		}
		/// <summary>
		/// Свойства, которое указывает на видимость Label
		/// </summary>
		private bool _isLabelActive = true;
		public bool IsLabelActive
		{
			get { return _isLabelActive; }
			set
			{
				_isLabelActive = value;
				OnPropertyChanged(nameof(IsLabelActive));
			}
		}
		#endregion
		#region SnackBar
		/// <summary>
		/// Сообщение для SnackBar
		/// </summary>
		private string _snackbarMessage;
		public string SnackbarMessage
		{
			get { return _snackbarMessage; }
			set
			{
				_snackbarMessage = value;
				OnPropertyChanged(nameof(SnackbarMessage));
			}
		}
		/// <summary>
		/// Свойство для показа уведомления
		/// </summary>
		private bool _isSnackbarActive;
		public bool IsSnackbarActive
		{
			get { return _isSnackbarActive; }
			set
			{
				_isSnackbarActive = value;
				OnPropertyChanged(nameof(IsSnackbarActive));
			}
		}
		/// <summary>
		/// Метод для показа уведомления об ошибке Snackbar 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="durationInMilliseconds"></param>
		public void ShowErrorSnackbar(string errorMessage)
		{
			SnackbarMessage = errorMessage;
			IsSnackbarActive = true;
			// Установите таймер для автоматического скрытия Snackbar после определенного времени
			// Например:
			Task.Delay(5000).ContinueWith(_ =>
			{
				IsSnackbarActive = false;
			});
		}
		/// <summary>
		/// Метод для показа уведомления Snackbar с заданным текстом и временем показа
		/// </summary>
		/// <param name="message"></param>
		/// <param name="durationInMilliseconds"></param>
		public async void ShowSnackbar(string message, int durationInMilliseconds)
		{
			SnackbarMessage = message;
			IsSnackbarActive = true;
			// Установите таймер для автоматического скрытия Snackbar после указанного времени
			Task.Delay(durationInMilliseconds).ContinueWith(_ =>
			{
				IsSnackbarActive = false;
			});
		}
		#endregion
		#region Функционал_кнопок
		//функционал локальных кнопок
		/// <summary>
		/// Включение функционала для редактирования
		/// </summary>
		public RelayCommand GetPermissionToEdit
		{
			get
			{
				return new RelayCommand((obj) =>
				{
					IsEditable = !IsEditable;
				}, (obj) => Id != 0);
			}
		}
		/// <summary>
		/// Обновление формы
		/// </summary>
		public RelayCommand RefreshTabPage
		{
			get
			{
				return new RelayCommand((obj) =>
				{
					RefreshPage();
				});
			}
		}
		/// <summary>
		/// Выбрать с устройства изображение
		/// </summary>
		public RelayCommand OpenLocalStorage
		{
			get
			{
				return new RelayCommand((obj) =>
				{
					OpenFileDialog openFileDialog = new OpenFileDialog();
					openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.gif) | *.jpg; *.jpeg; *.png; *.gif";
					if (openFileDialog.ShowDialog() == true)
					{
						string selectedImagePath = openFileDialog.FileName;
						// Здесь вы можете обработать выбранное изображение
						//MessageBox.Show("Выбранное изображение: " + selectedImagePath);
						try
						{
							//ImageSource = selectedImagePath;
							BitmapImage bitmap = new BitmapImage(new Uri(selectedImagePath));
							FileInfo fileInfo = new FileInfo(selectedImagePath);
							double fileSizeInMegabytes = fileInfo.Length / 1024.0 / 1024.0;                          // Проверяем, что размер файла не превышает максимально допустимый
							if (fileSizeInMegabytes <= MaxImageSizeMB) // Конвертируем мегабайты в байты
							{
								Image = bitmap;
								ShowSnackbar("Изображение успешно открыто", 3500);
							}
							else
							{
								ShowErrorSnackbar($"Ошибка: изображение превышает {MaxImageSizeMB} Мб");
							}
							//	ImgForCreate.Source = bitmap;
						}
						catch (Exception ex)
						{
							ShowErrorSnackbar("Ошибка при загрузке изображения: " + ex.Message);
						}
					}
				});
			}
		}
		/// <summary>
		/// Создать в бд новую запись
		/// </summary>
		public RelayCommand CreateNewItem
		{
			get
			{
				return new RelayCommand(async (obj) =>
				{
					using (HttpClient client = new HttpClient())
					{
						try
						{
							IsActiveLoad = !IsActiveLoad;
							if (ImageName.Length <= MaxImageNameLength)
							{
								string base64String = ImageHandler.FromImageToString64(Image);
								GalleryItem galleryData = new GalleryItem() { ImageName = ImageName, Image = base64String };
								string messageOnServer = JsonConvert.SerializeObject(galleryData);
								var content = new StringContent(messageOnServer, System.Text.Encoding.UTF8, "application/json");
								HttpResponseMessage response = await client.PostAsync("http://localhost:5258/Gallery/CreateGalleryItem", content);
								if (response.IsSuccessStatusCode)
								{
									string responseBody = await response.Content.ReadAsStringAsync();
									ShowSnackbar("Изображение успешно отправлено на сервер", 3500);
									RefreshPage();
								}
								else
								{
									ShowErrorSnackbar("Ошибка при выполнении запроса: " + response.StatusCode);
								}
							}
							else
							{
								ShowErrorSnackbar("Превышена максимальная длина текста картинки: " + MaxImageNameLength);
							}
						}
						catch (Exception ex)
						{
							Console.WriteLine("Ошибка: " + ex.Message);
						}
						finally
						{
							IsActiveLoad = false;
						}
					}
				}, (obj) => ImageName != null && Image != null);
			}
		}
		/// <summary>
		/// Получить все элементы галереи
		/// </summary>
		public RelayCommand GetAllItems
		{
			get
			{
				return new RelayCommand(async (obj) =>
				{
					using (HttpClient client = new HttpClient())
					{
						try
						{
							RefreshPage();
							IsActiveLoad = !IsActiveLoad;
							HttpResponseMessage response = await client.GetAsync("http://localhost:5258/Gallery/GetAllGalleryItems");
							if (response.IsSuccessStatusCode)
							{
								string responseBody = await response.Content.ReadAsStringAsync();
								List<GalleryItem> galleryItems = JsonConvert.DeserializeObject<List<GalleryItem>>(responseBody);
								//GalleryItems.Clear();
								GalleryItems = new ObservableCollection<GalleryItem>(galleryItems);
								CollectionImageNames.Clear();
								foreach (var item in galleryItems)
								{
									CollectionImageNames.Add($"{item.Id}.{item.ImageName}");
								}
								//ImageName = CollectionImageNames[GalleryItems.Count - 1];
								//Image = ImageHandler.FromStringToBitMap(GalleryItems[GalleryItems.Count - 1].Image);
								//ShowSnackbar("Галлерея получена,отображена последняя добавленная картина", 4500);
								//_isActiveLoad = true;
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
						finally
						{
							IsActiveLoad = false;
						}
					}
				});
			}
		}
		/// <summary>
		/// Удалить элемент из галлерии
		/// </summary>
		public RelayCommand DeleteItem
		{
			get
			{
				return new RelayCommand(async (obj) =>
				{
					using (HttpClient client = new HttpClient())
					{
						try
						{
							IsActiveLoad = !IsActiveLoad;// HttpDelete
							string messageOnServer = Id.ToString();
							var content = new StringContent(messageOnServer, System.Text.Encoding.UTF8, "application/json");
							HttpResponseMessage response = await client.PutAsync("http://localhost:5258/Gallery/DeleteItem", content);
							if (response.IsSuccessStatusCode)
							{
								//GalleryItems.Clear();
								string responseBody = await response.Content.ReadAsStringAsync();
								ShowSnackbar(responseBody, 3500);
								GetAllItems.Execute(obj);
								//_isActiveLoad = true;
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
						finally
						{
							IsActiveLoad = !IsActiveLoad;
							IsEditable = false;
						}
					}
				}, (obj) => Id != 0);
			}
		}
		/// <summary>
		/// Редактировать выбранный элемент
		/// </summary>
		public RelayCommand EditItem
		{
			get
			{
				return new RelayCommand(async (obj) =>
				{
					using (HttpClient client = new HttpClient())
					{
						try
						{
							if (ImageName.Length <= MaxImageNameLength)
							{
								IsActiveLoad = !IsActiveLoad;
								string base64String = ImageHandler.FromImageToString64(Image);
								GalleryItem editableGalleryItem = GalleryItems.FirstOrDefault(i => i.Id == Id);
								editableGalleryItem.ImageName = ImageName;
								editableGalleryItem.Image = editableGalleryItem.Image != ImageName ? ImageHandler.FromImageToString64(Image) : editableGalleryItem.Image;
								//GalleryItem galleryData = new GalleryItem() { ImageName = ImageName, Image = base64String };
								string messageOnServer = JsonConvert.SerializeObject(editableGalleryItem);
								var content = new StringContent(messageOnServer, System.Text.Encoding.UTF8, "application/json");
								HttpResponseMessage response = await client.PutAsync("http://localhost:5258/Gallery/UpdateItem", content);
								if (response.IsSuccessStatusCode)
								{
									string responseBody = await response.Content.ReadAsStringAsync();
									ShowSnackbar(responseBody, 3500);
									GetAllItems.Execute(obj);
								}
								else
								{
									Console.WriteLine("Ошибка при выполнении запроса: " + response.StatusCode);
								}
							}
							else {
								ShowErrorSnackbar("Превышена максимальная длина текста картинки: " + MaxImageNameLength);
								throw new FormatException();
							}
						}
						catch (Exception ex)
						{
							Console.WriteLine("Ошибка: " + ex.Message);
						}
						finally
						{
							IsActiveLoad = !IsActiveLoad;
							IsEditable = false;
						}
					}
				}, (obj) => Id != 0);
			}
		}
		#endregion
		public async void RefreshPage()
		{
			Id = 0;
			Image = null;
			IsEditable = false;
			ImageName = null;
		}
		// Реализация интерфейса INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
