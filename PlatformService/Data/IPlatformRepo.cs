using PlatformService.Models;
using System.Collections.Generic;

namespace PlatformService.Data{
    public interface IPlatformRepo
    {
        bool SaveChanges();

        IEnumerable<Platform> GetAllPlatforms();    

        Platform GetPlatformByID(int id);

        void CreatePlatform(Platform plat);
    }
}