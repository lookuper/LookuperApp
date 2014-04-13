using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace LookuperView
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var error = e.Exception;
            var formattedError = String.Format("{0} | {1}", error.Source, error.Message);
            var mainWindow = Application.Current.MainWindow;

            var viewModel = mainWindow.DataContext as LookuperViewModel;
            if (viewModel != null)
            {
                viewModel.SetError(formattedError, error.StackTrace);
            }
            else
            {
                var inputFormViewModel = mainWindow.DataContext as InputFormViewModel;
                if (inputFormViewModel != null)
                {
                    //inputFormViewModel.ShowError(formattedError, error.StackTrace);
                }
            }

            e.Handled = true;
        }
    }
}
