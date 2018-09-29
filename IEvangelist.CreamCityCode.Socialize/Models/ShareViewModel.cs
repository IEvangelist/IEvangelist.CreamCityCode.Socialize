using System;

namespace IEvangelist.CreamCityCode.Socialize.Models
{
    public class ShareViewModel
    {
        public bool IsSingleImage => (AllImageUrls?.Length ?? 0) == 1;

        public Uri ImageUrl => IsSingleImage ? AllImageUrls[0] : null;
        
        public Uri[] AllImageUrls { get; set; }
    }
}