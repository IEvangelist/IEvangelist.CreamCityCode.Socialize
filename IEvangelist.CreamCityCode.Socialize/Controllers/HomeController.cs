using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IEvangelist.CreamCityCode.Socialize.Models;
using IEvangelist.CreamCityCode.Socialize.Services;
using System.Threading.Tasks;

namespace IEvangelist.CreamCityCode.Socialize.Controllers
{
    public class HomeController : Controller
    {
        // 67231fd6-dac8-43af-8314-bd4c62858595
        [Route("/{id?}")]
        public async Task<IActionResult> Index(
            [FromRoute] string id,
            [FromServices] IImageRepository imageRepository) 
            => View(new ShareViewModel { ImageUrl = await imageRepository.GetImageUriAsync(id) });

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() 
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}