using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TraderForPoe.Classes
{
    static class WindowViewLoaderService
    {
        private static Dictionary<Type, Type> viewDictionary = new Dictionary<Type, Type>();
        
        public static void Register(Type viewmodel, Type view)
        {
            viewDictionary.Add(viewmodel, view);
        }

        public static void Show(Type viewmodel)
        {
            try
            {
                Window tmpWindows = (Window)Activator.CreateInstance(viewDictionary[viewmodel]);
                tmpWindows.Show();
                tmpWindows.Activate();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while creating View in WindowsViewLoaderService\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
