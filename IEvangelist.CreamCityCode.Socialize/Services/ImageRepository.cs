using Centare.Extensions;
using IEvangelist.CreamCityCode.Socialize.Providers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IEvangelist.CreamCityCode.Socialize.Services
{
    public class ImageRepository : IImageRepository
    {
        private readonly IContainerProvider _containerProvider;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ImageRepository> _logger;

        public ImageRepository(
            IContainerProvider containerProvider,
            IMemoryCache cache,
            ILogger<ImageRepository> logger)
        {
            _containerProvider = containerProvider ?? throw new ArgumentNullException(nameof(containerProvider));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Uri> GetImageUriAsync(string id)
        {
            var cachedUri = await _cache.GetOrCreateAsync(id, async entry =>
            {
                try
                {
                    entry.SlidingExpiration = TimeSpan.FromHours(6);

                    var container = await _containerProvider.GetContainerAsync();
                    var reference = await container.GetBlobReferenceFromServerAsync(id);

                    return reference.Uri;
                }
                catch (Exception ex)
                {
                    ex.TryLogException(_logger);

                    // Worst case scenario, fallback to somewhat reasonable URI.
                    return new Uri($"https://ievangelistphotobooth.blob.core.windows.net/photoboothimages/{id}");
                }
            });

            return cachedUri;
        }

        public async Task<Uri[]> GetAllImageUrisAsync()
        {
            try
            {
                var continuation = new BlobContinuationToken();
                var container = await _containerProvider.GetContainerAsync();
                var blobs = await container.ListBlobsSegmentedAsync(continuation);
                
                var urls = blobs.Results
                                .Select(blob => blob.Uri)
                                .OrderByDescending(uri => uri.ToString())
                                .Distinct()
                                .ToArray();

                return urls;
            }
            catch (Exception ex)
            {
                ex.TryLogException(_logger);

                // Worst case scenario, fallback to empty array.
                return new Uri[0];
            }
        }
    }
}