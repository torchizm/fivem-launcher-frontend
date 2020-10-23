using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Launchwares.CustomElements
{
    public class CustomScrollViewer : ScrollViewer
    {
        //Register a DependencyProperty which has a onChange callback
        public static DependencyProperty CurrentVerticalOffsetProperty = DependencyProperty.Register("CurrentVerticalOffset", typeof(double), typeof(CustomScrollViewer), new PropertyMetadata(new PropertyChangedCallback(OnVerticalChanged)));
        public static DependencyProperty CurrentHorizontalOffsetProperty = DependencyProperty.Register("CurrentHorizontalOffsetOffset", typeof(double), typeof(CustomScrollViewer), new PropertyMetadata(new PropertyChangedCallback(OnHorizontalChanged)));

        //When the DependencyProperty is changed change the vertical offset, thus 'animating' the scrollViewer
        private static void OnVerticalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomScrollViewer viewer = d as CustomScrollViewer;
            viewer.ScrollToVerticalOffset((double)e.NewValue);
        }

        //When the DependencyProperty is changed change the vertical offset, thus 'animating' the scrollViewer
        private static void OnHorizontalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomScrollViewer viewer = d as CustomScrollViewer;
            viewer.ScrollToHorizontalOffset((double)e.NewValue);
        }


        public double CurrentHorizontalOffset
        {
            get { return (double)this.GetValue(CurrentHorizontalOffsetProperty); }
            set { this.SetValue(CurrentHorizontalOffsetProperty, value); }
        }

        public double CurrentVerticalOffset
        {
            get { return (double)this.GetValue(CurrentVerticalOffsetProperty); }
            set { this.SetValue(CurrentVerticalOffsetProperty, value); }
        }
    }
}