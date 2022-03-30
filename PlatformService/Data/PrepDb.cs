using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;

namespace PlatformService.Data
{
  public static class PrepDb
  {
    public static void PrepPopulation(IApplicationBuilder app)
    {
      using (var servicesScope = app.ApplicationServices.CreateScope())
      {
        SeedData(servicesScope.ServiceProvider.GetService<AppDbContext>());
      }
    }

    private static void SeedData(AppDbContext context)
    {
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