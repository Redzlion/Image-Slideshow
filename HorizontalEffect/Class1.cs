using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
public class HorizontalEffect : ISlideshowEffect
{
    public string Name => "Horizontal Effect";

    /*    public void PlaySlideshow(Image imageIn, Image imageOut, double windowWidth, double windowHeight)
        {
            StoryboardIn(windowWidth, imageIn).Begin(imageIn);
            StoryboardOut(imageOut).Begin(imageOut);
        }
        Storyboard StoryboardIn (double windowWidth, Image imageIn){
            Storyboard sbIn = new Storyboard();

            DoubleAnimation HorizontalIn = new DoubleAnimation
            {
                From = 0,
                To = windowWidth,
                Duration = TimeSpan.FromSeconds(1)
            };
            ThicknessAnimation ThicknessIn = new ThicknessAnimation() {
                Duration = TimeSpan.FromSeconds(0),
                To = new System.Windows.Thickness(0, 0, 0, 0)
            };

            Storyboard.SetTarget(HorizontalIn, imageIn);
            Storyboard.SetTargetProperty(HorizontalIn, new System.Windows.PropertyPath(FrameworkElement.WidthProperty));
            Storyboard.SetTarget(ThicknessIn, imageIn);
            Storyboard.SetTargetProperty(ThicknessIn, new System.Windows.PropertyPath(FrameworkElement.MarginProperty));

            sbIn.Children.Add(HorizontalIn);
            sbIn.Children.Add(ThicknessIn);

            return sbIn;
        }
        Storyboard StoryboardOut(Image imageOut)
        {
            Storyboard sbOut = new Storyboard();
            DoubleAnimation HorizontalOut = new DoubleAnimation
            {
                Duration = TimeSpan.FromSeconds(1),
                To = 0
            };
            *//*ThicknessAnimation ThicknessOut = new ThicknessAnimation
            {
                Duration = TimeSpan.FromSeconds(1),
                To = new System.Windows.Thickness(windowWidth, 0, 0, 0)
            };
    *//*
            Storyboard.SetTarget(HorizontalOut, imageOut);
            Storyboard.SetTargetProperty(HorizontalOut, new System.Windows.PropertyPath(FrameworkElement.WidthProperty));

            sbOut.Children.Add(HorizontalOut);

            return sbOut;
        }*/

    public void PlaySlideshow(Image imageIn, Image imageOut, double windowWidth, double windowHeight)
    {
        Storyboard StoryboardOut = new Storyboard();
        Storyboard StoryboardIn = new Storyboard();

        DoubleAnimation HorizontalIn = new DoubleAnimation
        {
            From = 0,
            To = windowWidth,
            Duration = TimeSpan.FromSeconds(0.5)
        };
        ThicknessAnimation ThicknessIn = new ThicknessAnimation()
        {
            Duration = TimeSpan.FromSeconds(0),
            To = new System.Windows.Thickness(0, 0, 0, 0)
        };

        Storyboard.SetTarget(HorizontalIn, imageIn);
        Storyboard.SetTargetProperty(HorizontalIn, new System.Windows.PropertyPath(FrameworkElement.WidthProperty));
        Storyboard.SetTarget(ThicknessIn, imageIn);
        Storyboard.SetTargetProperty(ThicknessIn, new System.Windows.PropertyPath(FrameworkElement.MarginProperty));

        StoryboardIn.Children.Add(HorizontalIn);
        StoryboardIn.Children.Add(ThicknessIn);

        DoubleAnimation HorizontalOut = new DoubleAnimation
        {
            Duration = TimeSpan.FromSeconds(0.5),
            To = 0
        };
        ThicknessAnimation ThicknessOut = new ThicknessAnimation
        {
            Duration = TimeSpan.FromSeconds(0.5),
            To = new System.Windows.Thickness(windowWidth, 0, 0, 0)
        };

        Storyboard.SetTarget(HorizontalOut, imageOut);
        Storyboard.SetTargetProperty(HorizontalOut, new System.Windows.PropertyPath(FrameworkElement.WidthProperty));
        Storyboard.SetTarget(ThicknessOut, imageOut);
        Storyboard.SetTargetProperty(ThicknessOut, new System.Windows.PropertyPath(FrameworkElement.MarginProperty));

        StoryboardOut.Children.Add(HorizontalOut);
        StoryboardOut.Children.Add(ThicknessOut);

        StoryboardOut.Begin(imageOut);
        StoryboardIn.Begin(imageIn);

    }
}