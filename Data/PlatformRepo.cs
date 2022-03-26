using System.Collections.Generic;
using PlatformService.Models;

namespace PlatformService.Data
{
  public class PlatformRepo : IPlatformRepo
  {
    private readonly AppDbContext _context;

    public PlatformRepo(AppDbContext context)
    {
      _context = context;
    }
    public void CreatePlatform(Platform platform)
    {
      throw new System.NotImplementedException();
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
      throw new System.NotImplementedException();
    }

    public Platform GetPlatformById(int id)
    {
      throw new System.NotImplementedException();
    }

    public bool SaveChanges()
    {
      return (_context.SaveChanges() >= 0);
    }
  }
}