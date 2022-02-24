using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MyWpfApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //WalkDictionary(this.Resources);

            base.OnStartup(e);
        }

        private static void WalkDictionary(ResourceDictionary resources)
        {
            foreach (DictionaryEntry entry in resources) { }

            foreach (ResourceDictionary rd in resources.MergedDictionaries)
                WalkDictionary(rd);
        }

        internal static void ChangeAppTheme(bool isDarkMode)
        {
            Application.Current.Resources.MergedDictionaries[1].Clear();
            Application.Current.Resources.MergedDictionaries.RemoveAt(1);
            InsertThemeDictionary(isDarkMode ? "DarkThemeDictionary" : "LightThemeDictionary");
        }

        private static void InsertThemeDictionary(string fileName, int position = 1)
        {
            Application.Current.Resources.MergedDictionaries.Insert(position, new ResourceDictionary() { Source = new Uri($"/Resources/AppThemes/{fileName}.xaml", UriKind.Relative) });
        }
    }
}
