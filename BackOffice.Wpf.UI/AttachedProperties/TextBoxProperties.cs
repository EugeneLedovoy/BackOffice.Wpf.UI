using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyWpfApp
{
    public class IsAllowNegativeProperty : AttachedPropertyBase<IsAllowNegativeProperty, bool>
    {
        
    }

    #region IsLong

    public class IsLongProperty : AttachedPropertyBase<IsLongProperty, bool>
    {
        protected override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox tbx)
            {
                if ((bool)e.OldValue && !(bool)e.NewValue)
                {
                    tbx.PreviewKeyDown -= Tbx_PreviewKeyDown;
                    tbx.PreviewTextInput -= Tbx_PreviewTextInputLong;
                    DataObject.RemovePastingHandler(tbx, OnPasteForLong);
                }
                if ((bool)e.NewValue)
                {
                    tbx.PreviewKeyDown += Tbx_PreviewKeyDown;
                    tbx.PreviewTextInput += Tbx_PreviewTextInputLong; ;
                    DataObject.AddPastingHandler(tbx, OnPasteForLong);
                }
            }
        }

        private static void Tbx_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }

        private static void Tbx_PreviewTextInputLong(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox tbx)
            {
                string fullText = GetFullText(tbx, e.Text);
                e.Handled = !IsValidLong(fullText, IsAllowNegativeProperty.GetValue(tbx));
            }
        }

        private static string GetFullText(TextBox textBox, string inputText)
        {
            return textBox.SelectionLength > 0 ? textBox.Text.Replace(textBox.SelectedText, inputText) : textBox.Text.Insert(textBox.CaretIndex, inputText);
        }

        private static bool IsValidLong(string text, bool isAllowedNegative)
        {
            return isAllowedNegative
                ? long.TryParse(text, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out _)
                    || string.IsNullOrEmpty(text)
                    || text == CultureInfo.InvariantCulture.NumberFormat.NegativeSign
                : ulong.TryParse(text, out _);
        }

        private static void OnPasteForLong(object sender, DataObjectPastingEventArgs e)
        {
            if (sender is TextBox tbx)
            {
                var isText = e.SourceDataObject.GetDataPresent(DataFormats.Text, true);
                if (!isText) return;

                var clipboardText = (string)e.SourceDataObject.GetData(DataFormats.Text);

                string fullText = GetFullText(tbx, clipboardText);

                if (!IsValidLong(fullText, IsAllowNegativeProperty.GetValue(tbx)))
                    e.CancelCommand();
            }
        }
    }

    #endregion

    #region IsDecimal

    public class IsDecimalSelectAllOnMouseDoubleClickProperty : AttachedPropertyBase<IsDecimalSelectAllOnMouseDoubleClickProperty, bool>
    {

    }

    public class IsDecimalProperty : AttachedPropertyBase<IsDecimalProperty, bool>
    {
        protected override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox tbx)
            {
                if ((bool)e.OldValue && !(bool)e.NewValue)
                {
                    tbx.PreviewKeyDown -= Tbx_PreviewKeyDown;
                    tbx.PreviewTextInput -= Tbx_PreviewTextInputDecimal;
                    tbx.TextChanged -= Tbx_TextChanged;
                    DataObject.RemovePastingHandler(tbx, OnPasteForDecimal);

                    if (IsDecimalSelectAllOnMouseDoubleClickProperty.GetValue(tbx))
                        tbx.MouseDoubleClick -= Tbx_MouseDoubleClick;
                }
                if ((bool)e.NewValue)
                {
                    tbx.PreviewKeyDown += Tbx_PreviewKeyDown;
                    tbx.PreviewTextInput += Tbx_PreviewTextInputDecimal;
                    tbx.TextChanged += Tbx_TextChanged;
                    DataObject.AddPastingHandler(tbx, OnPasteForDecimal);

                    if (IsDecimalSelectAllOnMouseDoubleClickProperty.GetValue(tbx))
                        tbx.MouseDoubleClick += Tbx_MouseDoubleClick;
                }
            }
        }

        private static void Tbx_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }

        private static void Tbx_PreviewTextInputDecimal(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox tbx)
            {
                string fullText = GetFullText(tbx, e.Text);
                e.Handled = !IsValidDecimal(fullText, IsAllowNegativeProperty.GetValue(tbx));
            }
        }

        private static void OnPasteForDecimal(object sender, DataObjectPastingEventArgs e)
        {
            if (sender is TextBox tbx)
            {
                var isText = e.SourceDataObject.GetDataPresent(DataFormats.Text, true);
                if (!isText) return;

                var clipboardText = (string)e.SourceDataObject.GetData(DataFormats.Text);

                string fullText = GetFullText(tbx, clipboardText);

                if (!IsValidDecimal(fullText, IsAllowNegativeProperty.GetValue(tbx)))
                    e.CancelCommand();
            }
        }

        private static void Tbx_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox tbx && tbx.Text.Contains(","))
            {
                var forReplace = tbx.Text?.Replace(",", ".");
                var carretIndex = tbx.CaretIndex;
                tbx.Text = forReplace;
                tbx.CaretIndex = carretIndex;
            }
        }

        private static bool IsValidDecimal(string text, bool isAllowedNegative)
        {
            var temp = text?.Replace(",", ".");
            return isAllowedNegative
                ? decimal.TryParse(temp, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out _)
                    || string.IsNullOrEmpty(temp)
                    || temp == CultureInfo.InvariantCulture.NumberFormat.NegativeSign
                : decimal.TryParse(temp, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _) || string.IsNullOrEmpty(temp);
        }

        private static string GetFullText(TextBox textBox, string inputText)
        {
            return textBox.SelectionLength > 0 ? textBox.Text.Replace(textBox.SelectedText, inputText) : textBox.Text.Insert(textBox.CaretIndex, inputText);
        }

        private static void Tbx_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox tbx)
            {
                tbx.SelectAll();
            }
        }
    }

    #endregion

}
