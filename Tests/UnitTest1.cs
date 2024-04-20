using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using ServerSide.Services;
using SharedResources.Models;
using Microsoft.EntityFrameworkCore;
namespace ServerSide.Tests
{
	public class GalleryManageServiceTests
	{
		private readonly string _dataBaseName = "TestDatabase";
		public GalleryManageServiceTests()
		{
		}
		[Fact]
		public void GetAllGalleryItems_ReturnsItemsFromDataBase()
		{
			// Arrange
			var mockDbSet = new Mock<DbSet<GalleryItem>>();
			var context = new ApplicationContext(new DbContextOptionsBuilder<ApplicationContext>()
					.UseInMemoryDatabase(databaseName: _dataBaseName)
					.Options);
			var assertableList = new List<GalleryItem> { new GalleryItem { Id = 1, ImageName = "TestImageNameForTestGetAllGalleryItems", Image = "TestImageForTestGetAllGalleryItems" } };
			context.GalleryItems.AddRange(assertableList);
			context.SaveChanges();
			var mockCache = new Mock<IMemoryCache>();
			mockCache
				.Setup(x => x.CreateEntry(It.IsAny<object>()))
				.Returns(Mock.Of<ICacheEntry>);
			var service = new GalleryService(context, mockCache.Object);
			// Act
			var result = service.GetAllGalleryItems();
			// Assert
			Assert.Equal(assertableList, result);
		}
		[Fact]
		public void CreateNewItem_AddsItemToDatabase()
		{ 
			// Arrange
			var mockDbSet = new Mock<DbSet<GalleryItem>>();
			var context = new ApplicationContext(new DbContextOptionsBuilder<ApplicationContext>()
					.UseInMemoryDatabase(databaseName: _dataBaseName)
					.Options);
			var assertableItem =  new GalleryItem { ImageName = "TestImageNameForCreateNewItem", Image = "TestImageForCreateNewItem" };
			var a = context.GalleryItems.ToList();
			var mockCache = new Mock<IMemoryCache>();
			mockCache
				.Setup(x => x.CreateEntry(It.IsAny<object>()))
				.Returns(Mock.Of<ICacheEntry>);
			var service = new GalleryService(context, mockCache.Object);
			// Act
			int createdId = Convert.ToInt32(service.CreateNewItem(assertableItem));
			var itemInDB = context.GalleryItems.FirstOrDefault(i => i.Id == createdId);
			// Assert
			// ѕровер€ем, что элемент был успешно добавлен в базу данных
			Assert.NotNull(itemInDB);
			Assert.Equal(assertableItem.ImageName, itemInDB.ImageName);
			Assert.Equal(assertableItem.Image, itemInDB.Image);
		}
		[Fact]
		public void UpdateItem_UpdatesItemInCacheAndDatabase()
		{
			var mockDbSet = new Mock<DbSet<GalleryItem>>();
			var context = new ApplicationContext(new DbContextOptionsBuilder<ApplicationContext>()
					.UseInMemoryDatabase(databaseName: _dataBaseName)
					.Options);
			var assertableItem = new GalleryItem { ImageName = "TestImageNameForUpdateItem", Image = "TestImageForUpdateItem" };
			string oldItemImageName = assertableItem.ImageName;
			var a = context.GalleryItems.ToList();
			var mockCache = new Mock<IMemoryCache>();
			mockCache
				.Setup(x => x.CreateEntry(It.IsAny<object>()))
				.Returns(Mock.Of<ICacheEntry>);
			var service = new GalleryService(context, mockCache.Object);
			// Act
			// создали новый item
			int createdId = Convert.ToInt32(service.CreateNewItem(assertableItem));
			GalleryItem createdItemInDB = context.GalleryItems.FirstOrDefault(i => i.Id == createdId);
			Assert.NotNull(createdItemInDB);
			string newName = createdItemInDB.ImageName = "NewestImageName";
			// обновл€ем новый item
			var result = service.UpdateItem(createdItemInDB);
			var updatedItem = context.GalleryItems.FirstOrDefault(i => i.Id == createdId);
			Assert.NotNull(updatedItem);
			// провер€ем что название созданного Item изменилось
			// Assert
			Assert.NotEqual(oldItemImageName, updatedItem.ImageName);
		}
		[Fact]
		public void DeleteItem_RemovesItemFromDatabase()
		{
			var mockDbSet = new Mock<DbSet<GalleryItem>>();
			var context = new ApplicationContext(new DbContextOptionsBuilder<ApplicationContext>()
					.UseInMemoryDatabase(databaseName: _dataBaseName)
					.Options);
			var assertableItem = new GalleryItem { ImageName = "TestImageNameForDeleteItem", Image = "TestImageForDeleteItem" };
			var a = context.GalleryItems.ToList();
			var mockCache = new Mock<IMemoryCache>();
			mockCache
				.Setup(x => x.CreateEntry(It.IsAny<object>()))
				.Returns(Mock.Of<ICacheEntry>);
			var service = new GalleryService(context, mockCache.Object);
			// Act
			// создали новый item
			int createdId = Convert.ToInt32(service.CreateNewItem(assertableItem));
			var createdItemInDB = context.GalleryItems.FirstOrDefault(i => i.Id == createdId);
			Assert.NotNull(createdItemInDB);
			// удалили новый item
			var result = service.DeleteItem(createdId);
			var itemInDB = context.GalleryItems.FirstOrDefault(i => i.Id == createdId);
			// Assert
			// ѕровер€ем, что элемент был успешно удалЄн в базу данных
			Assert.Null(itemInDB);
		}
	}
}
