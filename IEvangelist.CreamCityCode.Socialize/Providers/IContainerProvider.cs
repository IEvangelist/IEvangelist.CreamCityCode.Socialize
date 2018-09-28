using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;

namespace IEvangelist.CreamCityCode.Socialize.Providers
{
    public interface IContainerProvider
    {
        Task<CloudBlobContainer> GetContainerAsync();
    }
}