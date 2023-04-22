using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

public class OpacityEffect : ISlideshowEffect
{
    public string Name => "Opacity Effect";

    public void PlaySlideshow(Image imageIn, Image imageOut, double windowWidth, double windowHeight)
    {
        StoryboardIn(imageIn);
        StoryboardOut(imageOut);
    }
    void StoryboardIn(Image imageIn)
    {
        Storyboard sbIn = new Storyboard();

        DoubleAnimation OpacityIn = new DoubleAnimation
        {
            From = 0,
            To = 1,
            Duration = TimeSpan.FromSeconds(1)
        };


        Storyboard.SetTarget(OpacityIn, imageIn);
        Storyboard.SetTargetProperty(OpacityIn, new System.Windows.PropertyPath(UIElement.OpacityProperty));

        sbIn.Children.Add(OpacityIn);
        sbIn.Begin(imageIn);
    }
    void StoryboardOut(Image imageOut)
    {
        Storyboard sbOut = new Storyboard();
        DoubleAnimation OpacityOut = new DoubleAnimation
        {
            Duration = TimeSpan.FromSeconds(1),
            To = 0
        };

        Storyboard.SetTarget(OpacityOut, imageOut);
        Storyboard.SetTargetProperty(OpacityOut, new System.Windows.PropertyPath(UIElement.OpacityProperty));

        sbOut.Children.Add(OpacityOut);

        sbOut.Begin(imageOut);

    }
}