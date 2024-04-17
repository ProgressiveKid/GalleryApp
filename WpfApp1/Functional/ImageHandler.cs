﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace ClientSide.Functional
{
	class ImageHandler
	{
		/// <summary>
		/// Конвертирует изображение из файла по указанному пути в строку Base64.
		/// </summary>
		/// <param name="path">Путь к файлу изображения.</param>
		/// <returns>Строка Base64, представляющая изображение.</returns>
		public static string FromUriToString64(string path)
		{ 
			byte[] imageBytes = File.ReadAllBytes(path);
			return Convert.ToBase64String(imageBytes);
		}

		/// <summary>
		/// Конвертирует строку Base64 в изображение BitmapImage.
		/// </summary>
		/// <param name="image">Строка Base64, представляющая изображение.</param>
		/// <returns>Экземпляр BitmapImage, представляющий изображение.</returns>

		public static BitmapImage FromStringToBitMap(string image)
		{
			try
			{
				byte[] imageBytes = Convert.FromBase64String(image);
				BitmapImage bitmap = new BitmapImage();
				// Загрузка изображения из массива байтов
				using (MemoryStream stream = new MemoryStream(imageBytes))
				{
					bitmap.BeginInit();
					bitmap.CacheOption = BitmapCacheOption.OnLoad;
					bitmap.StreamSource = stream;
					bitmap.EndInit();
				}
				return bitmap;
			}
			catch (Exception ex)
			{
				// Пробросить исключение об ошибке
				throw new Exception("Ошибка при преобразовании строки Base64 в изображение BitmapImage: " + ex.Message);
			}
		}
	}
}