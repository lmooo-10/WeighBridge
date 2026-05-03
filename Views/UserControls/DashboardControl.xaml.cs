using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using WeighBridge.ViewModels;

namespace WeighBridge.Views.UserControls
{
    public partial class DashboardControl : UserControl
    {
        private bool _animationStarted = false;

        public DashboardControl()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is DashboardViewModel vm)
            {
                vm.PropertyChanged += (_, args) =>
                {
                    if (args.PropertyName == nameof(DashboardViewModel.TickerMessage)
                        && !_animationStarted)
                    {
                        Dispatcher.BeginInvoke(
                            RestartAnimation,
                            System.Windows.Threading.DispatcherPriority.Render);
                    }
                };
            }
        }

        private void RestartAnimation()
        {
            TickerText.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            double textWidth = TickerText.DesiredSize.Width;
            double panelWidth = ActualWidth - 70; // subtract LIVE badge width

            if (textWidth < 1 || panelWidth < 1) return;

            _animationStarted = true;

            TickerTranslate.BeginAnimation(TranslateTransform.XProperty, null);

            var anim = new DoubleAnimation
            {
                From = panelWidth,
                To = -textWidth,
                Duration = TimeSpan.FromSeconds((panelWidth + textWidth) / 80),
                RepeatBehavior = RepeatBehavior.Forever
            };

            TickerTranslate.BeginAnimation(TranslateTransform.XProperty, anim);
        }
    }
}