using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;

namespace PlatformService.Data
{
	public static class PrepDb
	{
		public static void PrepPopulation(IApplicationBuilder app, bool isProd)
		{
			using (var servicesScope = app.ApplicationServices.CreateScope())
			{
				SeedData(servicesScope.ServiceProvider.GetService<AppDbContext>(), isProd);
			}
		}

		private static void SeedData(AppDbContext context, bool isProd)
		{

			if (isProd)
			{
				Console.WriteLine("--> Attempting to apply migrations...");
				try
				{
					context.Database.Migrate();
				}
				catch (Exception exc)
				{
					Console.WriteLine($"--> Could not apply migrations... {exc.Message}");
				}
				
			}
			if (!context.Platforms.Any())
			{
				Console.WriteLine("--> Seeding data...");

				context.Platforms.AddRange(
					GetFakePlatforms()
				);

				context.SaveChanges();
			}
			else
			{
				Console.WriteLine("--> We already have data");
			}
		}

		private static List<Platform> GetFakePlatforms()
		{
			var platforms = new List<Platform>() {
			new Platform() { Name="Dot Net", Publisher="Microsoft", Cost="Free" },
			new Platform() { Name="SQL Server Express", Publisher="Microsoft", Cost="Free" },
			new Platform() { Name="Docker", Publisher="Docker Company", Cost="Free" },
			new Platform() { Name="Kubernetes", Publisher="Cloud Native Computing Foundation", Cost="Free" },
			new Platform() { Name="Python", Publisher="Comunity", Cost="Free" },
	  };

			return platforms;
		}
	}
}