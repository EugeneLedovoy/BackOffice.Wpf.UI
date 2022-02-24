using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Wpf.Core
{
    public static class VisualTreeHelperExtensions
    {
        public static DependencyObject GetPrimalParent(this DependencyObject child)
        {
            var parent = VisualTreeHelper.GetParent(child);
            return parent == null ? child : GetPrimalParent(parent);
        }

        public static DependencyObject GetAncestor(this DependencyObject child, int ancestorLevel = 1)
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent == null)
                return child;

            int count = 0;
            while (ancestorLevel > ++count)
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent;
        }

        public static DependencyObject GetAncestor(this DependencyObject child, Type ancestorType, int ancestorLevel = 1)
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent == null)
                return child;

            if (parent.GetType() == ancestorType && ancestorLevel == 1)
                return parent;

            int count = 0;
            while (ancestorLevel > count)
            {
                parent = VisualTreeHelper.GetParent(parent);
                if (parent.GetType() == ancestorType)
                    count++;
            }
            return parent;
        }

        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T ch)
                    {
                        yield return ch;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
