using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace XPlatformCloudKit.Controls
{
    public class DynamicGridView : GridView
    {
        protected override void PrepareContainerForItemOverride(Windows.UI.Xaml.DependencyObject element, object item)
        {
            try
            {
                dynamic _Item = item;
                element.SetValue(Windows.UI.Xaml.Controls.GridViewItem.WidthProperty, _Item.ColSpan);
                element.SetValue(Windows.UI.Xaml.Controls.GridViewItem.HeightProperty, _Item.RowSpan);;
                element.SetValue(Windows.UI.Xaml.Controls.VariableSizedWrapGrid.ColumnSpanProperty, _Item.ColSpan);
                element.SetValue(Windows.UI.Xaml.Controls.VariableSizedWrapGrid.RowSpanProperty, _Item.RowSpan);
            }
            catch
            {
                throw new Exception("Unable to set ColSpan or RowSpan from Item");
            }
            finally
            {
                base.PrepareContainerForItemOverride(element, item);
            }
        }
    }

}
