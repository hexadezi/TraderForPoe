using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace TraderForPoe.Classes
{
    public class SqueezeTabPanel : Panel
    {
        private double _rowHeight;
        private double _scaleFactor;

        // Ensure tabbing works correctly
        static SqueezeTabPanel()
        {
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(SqueezeTabPanel), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
            KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(SqueezeTabPanel), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
        }

        // This Panel lays its children out horizontally.
        // If all children cannot fit in the allocated space,
        // the available space is divided proportionally between them.
        protected override Size MeasureOverride(Size availableSize)
        {
            // See how much room the children want
            double width = 0.0;
            this._rowHeight = 0.0;
            foreach (UIElement element in this.Children)
            {
                element.Measure(availableSize);
                Size size = this.GetDesiredSizeLessMargin(element);
                this._rowHeight = Math.Max(this._rowHeight, size.Height);
                width += size.Width;
            }

            // If not enough room, scale the
            // children to the available width
            if (width > availableSize.Width)
            {
                this._scaleFactor = availableSize.Width / width;
                width = 0.0;
                foreach (UIElement element in this.Children)
                {
                    element.Measure(new Size(element.DesiredSize.Width * this._scaleFactor, availableSize.Height));
                    width += element.DesiredSize.Width;
                }
            }
            else
                this._scaleFactor = 1.0;

            return new Size(width, this._rowHeight);
        }

        // Perform arranging of children based on the final size
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            Point point = new Point();
            foreach (UIElement element in this.Children)
            {
                Size size1 = element.DesiredSize;
                Size size2 = this.GetDesiredSizeLessMargin(element);
                Thickness margin = (Thickness)element.GetValue(FrameworkElement.MarginProperty);
                double width = size2.Width;
                if (element.DesiredSize.Width != size2.Width)
                    width = arrangeSize.Width - point.X; // Last-tab-selected "fix"
                element.Arrange(new Rect(
                    point,
                    new Size(Math.Min(width, size2.Width), this._rowHeight)));
                double leftRightMargin = Math.Max(0.0, -(margin.Left + margin.Right));
                point.X += size1.Width + (leftRightMargin * this._scaleFactor);
            }

            return arrangeSize;
        }

        // Return element's size
        // after subtracting margin
        private Size GetDesiredSizeLessMargin(UIElement element)
        {
            Thickness margin = (Thickness)element.GetValue(FrameworkElement.MarginProperty);
            Size size = new Size();
            size.Height = Math.Max(0.0, element.DesiredSize.Height - (margin.Top + margin.Bottom));
            size.Width = Math.Max(0.0, element.DesiredSize.Width - (margin.Left + margin.Right));
            return size;
        }
    }
}
