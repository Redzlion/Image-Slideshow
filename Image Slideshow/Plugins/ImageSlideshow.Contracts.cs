using System.Windows.Controls;

namespace Image_Slideshow.Contracts
{
    public interface ISlideshowEffect
    {
        string Name { get; }

        void PlaySlideshow(Image imageIn, Image imageOut, double windowWidth, double windowHeight);
    }
}