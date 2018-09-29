using System;

namespace IEvangelist.CreamCityCode.Socialize.Models
{
    public class ShareViewModel
    {
        public bool IsSingleImage => (AllImageUrls?.Length ?? 0) == 1;

        public Uri ImageUrl => IsSingleImage ? AllImageUrls[0] : null;
        
        internal Uri[] AllImageUrls { get; set; }

        public ArraySegment<Uri> FirstQuarter => new ArraySegment<Uri>(AllImageUrls, 0, QuarterLength);

        public ArraySegment<Uri> SecondQuarter => new ArraySegment<Uri>(AllImageUrls, QuarterLength, QuarterLength);

        public ArraySegment<Uri> ThirdQuarter => new ArraySegment<Uri>(AllImageUrls, QuarterLength * 2, QuarterLength);

        public ArraySegment<Uri> FourthQuarter => new ArraySegment<Uri>(AllImageUrls, QuarterLength * 3, QuarterLength);

        private int QuarterLength => AllImageUrls.Length / 4;
    }
}