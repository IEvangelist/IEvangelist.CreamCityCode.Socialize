using System;
using System.Threading.Tasks;

namespace IEvangelist.CreamCityCode.Socialize.Services
{
    public interface IImageRepository
    {
        Task<Uri> GetImageUriAsync(string id);

        Task<Uri[]> GetAllImageUrisAsync();
    }
}