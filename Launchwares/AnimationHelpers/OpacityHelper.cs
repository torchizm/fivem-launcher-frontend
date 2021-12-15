using Launchwares.CustomElements;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Launchwares.AnimationHelpers
{
    internal static class OpacityHelper
    {
        internal static void FadeInButton(Button element, double to)
        {
            element.Visibility = Visibility.Visible;
            DoubleAnimation animation = new DoubleAnimation {
                From = element.Opacity,
                To = to,
                DecelerationRatio = .2,
                Duration = new Duration(TimeSpan.FromMilliseconds(200))
            };

            Storyboard sb = new Storyboard();
            sb.Children.Add(animation);
            Storyboard.SetTarget(animation, element);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Button.OpacityProperty));
            sb.Begin();
        }

        internal static void FadeOutButton(Button element)
        {
            DoubleAnimation animation = new DoubleAnimation {
                From = element.Opacity,
                To = 0,
                DecelerationRatio = .2,
                Duration = new Duration(TimeSpan.FromMilliseconds(200))
            };

            Storyboard sb = new Storyboard();
            sb.Children.Add(animation);
            Storyboard.SetTarget(animation, element);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Button.VisibilityProperty));
            sb.Completed += delegate { element.Visibility = Visibility.Hidden; };
            sb.Begin();
        }

        internal static void FadeInUpdateBox(BigUpdateBox element, double to)
        {
            element.Visibility = Visibility.Visible;
            DoubleAnimation animation = new DoubleAnimation {
                From = element.Opacity,
                To = to,
                DecelerationRatio = .2,
                Duration = new Duration(TimeSpan.FromMilliseconds(200))
            };

            Storyboard sb = new Storyboard();
            sb.Children.Add(animation);
            Storyboard.SetTarget(animation, element);
            Storyboard.SetTargetProperty(animation, new PropertyPath(BigUpdateBox.OpacityProperty));
            sb.Begin();
        }

        internal static void FadeOutUpdateBox(BigUpdateBox element)
        {
            DoubleAnimation animation = new DoubleAnimation {
                From = element.Opacity,
                To = 0,
                DecelerationRatio = .2,
                Duration = new Duration(TimeSpan.FromMilliseconds(200))
            };

            Storyboard sb = new Storyboard();
            sb.Children.Add(animation);
            Storyboard.SetTarget(animation, element);
            Storyboard.SetTargetProperty(animation, new PropertyPath(BigUpdateBox.OpacityProperty));
            sb.Completed += delegate { element.Visibility = Visibility.Hidden; };
            sb.Begin();
        }
    }
}
