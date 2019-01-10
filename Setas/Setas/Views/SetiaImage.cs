using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Setas.Views
{
    public class SetiaImage : CachedImage
    {
        public SetiaImage()
        {
            CacheDuration = TimeSpan.FromDays(30);
            RetryCount = 3;
            RetryDelay = 1000;
            LoadingPlaceholder = "loading.gif";
            ErrorPlaceholder = "broken.png";
            Aspect = Xamarin.Forms.Aspect.AspectFill;
        }
    }
}
