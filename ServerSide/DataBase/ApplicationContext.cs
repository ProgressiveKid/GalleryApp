using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

using System.Data;

using System.Collections.Generic;
using System.Reflection.Emit;
using SharedResources.Models;
using Microsoft.Extensions.Options;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ServerSide
{
	public class ApplicationContext : DbContext
	{
		public DbSet<GalleryItem> GalleryItems { get; set; }
		
		public ApplicationContext()
		{

		}
		public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
		{
			try
			{
				if (Database.CanConnect())
				{

					//  Database.EnsureDeleted();
					//  Database.EnsureCreated();
				}
				else
				{ // first call

					Database.EnsureCreated();
				}

			}
			catch (Exception ex)
			{

			}
		}
		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{ // убрали логирование запрос EF
			options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddFilter((category, level) =>
				category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)));
		}


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{





		}
	}
}
