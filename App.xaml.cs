using System.Windows;

namespace WeighBridge
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DispatcherUnhandledException += (s, args) =>
            {
                MessageBox.Show($"Une erreur est survenue : {args.Exception.Message}\n\n{args.Exception.StackTrace}",
                                "Erreur critique", MessageBoxButton.OK, MessageBoxImage.Error);
                args.Handled = true;
                Shutdown();
            };

            AppDomain.CurrentDomain.UnhandledException += (s, args) =>
            {
                MessageBox.Show($"Erreur non gérée : {args.ExceptionObject}",
                                "Erreur critique", MessageBoxButton.OK, MessageBoxImage.Error);
            };
        }
    }
}
