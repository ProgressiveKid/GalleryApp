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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using WpfApp1.Services;
using ClientSide.Functional;
using System.Linq;
using System.Globalization;
using System.Windows.Data;

namespace WpfApp1.ModelViews
{
	public class GalleryItemViewModel : INotifyPropertyChanged
	{
		public ICommand TabSelectionChangedCommand { get; }
		public GalleryItemViewModel()
		{
		}
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
					if (_imageName != null)
					{
						int idItem = Convert.ToInt32(_imageName.Split('.')[0]);
						string valueItem = string.Join(".", _imageName.Split('.').Skip(1));
						GalleryItem foundItem = GalleryItems.FirstOrDefault(item => item.Id.Equals(idItem) && item.ImageName.Equals(valueItem));
						if (foundItem != null)
						{
							Image = ImageHandler.FromStringToBitMap(foundItem.Image);
							OnPropertyChanged("Image");
						}
					}
				}
			}
		}

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
					//IsActiveTabControl = !IsActiveTabControl; // меняем на противположную активную секцию
				}
			}
		}

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

		#region Buttons
		/// <summary>
		/// Выбрать с устройства изображение
		/// </summary>
		public RelayCommand OpenLocalStorage
		{
			get { 
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
							Image = bitmap;
						//	ImgForCreate.Source = bitmap;
						}
						catch (Exception ex)
						{
							MessageBox.Show("Ошибка при загрузке изображения: " + ex.Message);
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
								//string imageName = txtImgName.Text;
								// Преобразование массива байтов в строку Base64
								// Преобразование изображения в массив байтов
								// Преобразование изображения в строку Base64
								//	BitmapImage bitmap = new BitmapImage(new Uri(selectedImagePath));
								//	ImgForCreate.Source = bitmap;
								//	string base64String = ImageHandler.FromUriToString64(ImageSource);
								string base64String = ImageHandler.FromImageToString64(Image);
								GalleryItem galleryData = new GalleryItem() { ImageName = ImageName, Image = base64String };
								string messageOnServer = JsonConvert.SerializeObject(galleryData);
								var content = new StringContent(messageOnServer, System.Text.Encoding.UTF8, "application/json");
								// Выполнение GET-запроса к веб-API
								HttpResponseMessage response = await client.PostAsync("http://localhost:5258/Gallery/CreateGalleryItem", content);
								// Проверка успешности запроса
								if (response.IsSuccessStatusCode)
								{
									// Получение ответа в виде строки
									string responseBody = await response.Content.ReadAsStringAsync();
									// Обработка полученных данных
									MessageBox.Show(responseBody);
									//Console.WriteLine(responseBody);
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
					}
				});
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
							 		Image = ImageHandler.FromStringToBitMap(GalleryItems[GalleryItems.Count - 1].Image);
									CollectionImageNames.Clear();
									foreach (var item in galleryItems)
									{
										CollectionImageNames.Add($"{item.Id}.{item.ImageName}");
									}
								 IsActiveLoad = !IsActiveLoad;

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
					}
					
				});
			}
		}
		#endregion

		public async void RefreshPage()
		{
			Image = null;
			ImageName = "";
		}
		// Реализация интерфейса INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public class BoolToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool boolValue && boolValue)
				return "Visible";
			else
				return "Hidden";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
