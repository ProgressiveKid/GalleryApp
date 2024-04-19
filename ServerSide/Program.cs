using Microsoft.EntityFrameworkCore;
using ServerSide.Services;
using ServerSide.Services.Interfaces;

namespace ServerSide
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			// Add services to the container.
			string connection = builder.Configuration.GetConnectionString("DefaultConnection");
			builder.Services.AddDbContext<ApplicationContext>(options =>
			{
					options.UseSqlServer(connection);
			});
			builder.Services.AddControllers();
			builder.Services.AddTransient<IGalleryService, GalleryManageService>();
			builder.Services.AddMemoryCache();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			var app = builder.Build();
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.UseHttpsRedirection();
			app.UseAuthorization();
			app.MapControllers();
			app.Run();
		}
	}
}