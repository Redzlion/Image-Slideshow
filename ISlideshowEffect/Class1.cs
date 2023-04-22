using System.Windows.Controls;

public interface ISlideshowEffect
{
    string Name { get; }

    void PlaySlideshow(Image imageIn, Image imageOut, double windowWidth, double windowHeight);

}