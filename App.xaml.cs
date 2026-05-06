using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using WeighBridge.Models;
using WeighBridge.Repositories;
using WeighBridge.Repositories.Interfaces;
using WeighBridge.Services;
using WeighBridge.Services.Interfaces;
using WeighBridge.Views.Windows;

//namespace WeighBridge
//{
// public partial class App : Application
//{
//protected override void OnStartup(StartupEventArgs e)
//{
//base.OnStartup(e);

//DispatcherUnhandledException += (s, args) =>
//{
//MessageBox.Show($"Une erreur est survenue : {args.Exception.Message}\n\n{args.Exception.StackTrace}",
//"Erreur critique", MessageBoxButton.OK, MessageBoxImage.Error);
//args.Handled = true;
//Shutdown();
//};

//AppDomain.CurrentDomain.UnhandledException += (s, args) =>
//{
//MessageBox.Show($"Erreur non gérée : {args.ExceptionObject}",
//"Erreur critique", MessageBoxButton.OK, MessageBoxImage.Error);
//};
//}
//}

// App.xaml.cs
namespace WeighBridge
{
    public partial class App : Application
    {
        // ── Static access ─────────────────────────────────────
        //public static UserModel? LoggedInUser { get; set; }
        //public static IServiceProvider? ServiceProvider { get; private set; }

        public static UserModel LoggedInUser { get; set; } = null!;
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        private const bool USE_MOCK = true;

        // public static UserModel LoggedInUser { get; set; } = null!;
        //public static IServiceProvider ServiceProvider { get; private set; } = null!;

        //protected override void OnStartup(StartupEventArgs e)
        //{
        //base.OnStartup(e);

        //var services = new ServiceCollection();

        //if (USE_MOCK)
        //{
        //services.AddSingleton<IWeighmentRepository, MockWeighmentRepository>();
        //services.AddSingleton<IZodiacService, MockZodiacService>();
        //services.AddSingleton<ITOSRepository, MockTOSRepository>();
        //}

        //var provider = services.BuildServiceProvider();

        // ← user comes from your login window
        // get it from wherever you store it after login
        //var user = App.LoggedInUser;

        //var window = new MainWindow(
        //user,
        //provider.GetRequiredService<IWeighmentRepository>(),
        //provider.GetRequiredService<IZodiacService>(),
        //provider.GetRequiredService<ITOSRepository>()
        //);

        //window.Show();
        //}

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // ── Build DI container ────────────────────────────
            var services = new ServiceCollection();

            if (USE_MOCK)
            {
                services.AddSingleton<IWeighmentRepository, WeighmentRepository>();
                services.AddSingleton<IZodiacService, ZodiacService>();
                services.AddSingleton<ITOSRepository, TOSRepository>();
            }

            ServiceProvider = services.BuildServiceProvider();

            // ── Show Login first ──────────────────────────────
            new LoginWindow().Show();
        }
    }
}