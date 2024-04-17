using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using SharedResources.Models;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Net.Http.Json;
using ClientSide.Functional;

namespace WpfApp1
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}
		/// <summary>
		/// Путь до изображения
		/// </summary>
		private string ImageSource { get; set; }
		#region CreateNewGalleryItem
		private void ButtonOpenImage_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.gif) | *.jpg; *.jpeg; *.png; *.gif";
			if (openFileDialog.ShowDialog() == true)
			{
				string selectedImagePath = openFileDialog.FileName;
				// Здесь вы можете обработать выбранное изображение
				MessageBox.Show("Выбранное изображение: " + selectedImagePath);
				try
				{
					ImageSource = selectedImagePath;
					BitmapImage bitmap = new BitmapImage(new Uri(selectedImagePath));
					Image.Source = bitmap;
				}
				catch (Exception ex)
				{
					MessageBox.Show("Ошибка при загрузке изображения: " + ex.Message);
				}
			}
		}
		private async void ButtonServerSend_Click(object sender, RoutedEventArgs e)
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					string imageName = ImageName.Text;
					// Преобразование массива байтов в строку Base64
					// Преобразование изображения в массив байтов

					// Преобразование изображения в строку Base64
					string base64String = ImageHandler.FromUriToString64(ImageSource);
					GalleryItem galleryData = new GalleryItem() { ImageName = imageName, Image = base64String };
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
			// Преобразование изображения в строку Base64
		}
		#endregion
		public void RefreshPage()
		{
		}
		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					HttpResponseMessage response = await client.GetAsync("http://localhost:5258/Gallery/GetAllGalleryItems");
					// Проверка успешности запроса
					if (response.IsSuccessStatusCode)
					{
						// Получение ответа в виде строки
						string responseBody = await response.Content.ReadAsStringAsync();
						List<GalleryItem> galleryItems = JsonConvert.DeserializeObject<List<GalleryItem>>(responseBody);
						Image.Source = ImageHandler.FromStringToBitMap(galleryItems[0].Image);
						

						// Присвоение BitmapImage свойству Source вашего элемента Image
						
						//Image.Source = ;
						// Обработка полученных данных
						//MessageBox.Show(responseBody);
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
		}
	}
}
