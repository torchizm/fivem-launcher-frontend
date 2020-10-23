using Launchwares.CustomElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Launchwares.AnimationHelpers
{
    internal static class ScrollViewerHelper
    {
        internal static void ScrollToHorizontalPosition(CustomScrollViewer scrollViewer, double y)
        {
            DoubleAnimation horizontalAnim = new DoubleAnimation {
                From = scrollViewer.HorizontalOffset,
                To = y,
                DecelerationRatio = .2,
                Duration = new Duration(TimeSpan.FromMilliseconds(400))
            };

            Storyboard sb = new Storyboard();
            sb.Children.Add(horizontalAnim);
            Storyboard.SetTarget(horizontalAnim, scrollViewer);
            Storyboard.SetTargetProperty(horizontalAnim, new PropertyPath(CustomScrollViewer.CurrentHorizontalOffsetProperty));
            sb.Begin();
        }
    }
}
