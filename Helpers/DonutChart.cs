using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WeighBridge.Helpers
{
    public class DonutChart : Control
    {
        // ── Dependency Properties ──────────────────────────────

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(DonutChart),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender, OnPropertyChanged));

        public static readonly DependencyProperty PrimaryColorProperty =
            DependencyProperty.Register(nameof(PrimaryColor), typeof(Brush), typeof(DonutChart),
                new FrameworkPropertyMetadata(Brushes.DeepPink,
                    FrameworkPropertyMetadataOptions.AffectsRender, OnPropertyChanged));

        public static readonly DependencyProperty SecondaryColorProperty =
            DependencyProperty.Register(nameof(SecondaryColor), typeof(Brush), typeof(DonutChart),
                new FrameworkPropertyMetadata(Brushes.Cyan,
                    FrameworkPropertyMetadataOptions.AffectsRender, OnPropertyChanged));

        public static readonly DependencyProperty ArcThicknessProperty =
            DependencyProperty.Register(nameof(ArcThickness), typeof(double), typeof(DonutChart),
                new FrameworkPropertyMetadata(22.0,
                    FrameworkPropertyMetadataOptions.AffectsRender, OnPropertyChanged));

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public Brush PrimaryColor
        {
            get => (Brush)GetValue(PrimaryColorProperty);
            set => SetValue(PrimaryColorProperty, value);
        }

        public Brush SecondaryColor
        {
            get => (Brush)GetValue(SecondaryColorProperty);
            set => SetValue(SecondaryColorProperty, value);
        }

        public double ArcThickness
        {
            get => (double)GetValue(ArcThicknessProperty);
            set => SetValue(ArcThicknessProperty, value);
        }

        // ── Visual Children ───────────────────────────────────
        private readonly Canvas _canvas = new();
        private readonly Ellipse _background = new() { Fill = Brushes.Transparent };
        private readonly Path _primaryArc = new() { Fill = Brushes.Transparent };
        private readonly Path _secondaryArc = new() { Fill = Brushes.Transparent };

        public DonutChart()
        {
            _canvas.Children.Add(_background);
            _canvas.Children.Add(_secondaryArc);
            _canvas.Children.Add(_primaryArc);
            AddVisualChild(_canvas);
            AddLogicalChild(_canvas);
        }

        protected override int VisualChildrenCount => 1;
        protected override Visual GetVisualChild(int index) => _canvas;

        protected override Size MeasureOverride(Size availableSize)
        {
            double w = double.IsInfinity(availableSize.Width) ? 150 : availableSize.Width;
            double h = double.IsInfinity(availableSize.Height) ? 150 : availableSize.Height;
            var size = new Size(w, h);
            _canvas.Measure(size);
            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _canvas.Arrange(new Rect(finalSize));
            DrawArcs(finalSize);
            return finalSize;
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DonutChart chart)
                chart.InvalidateArrange();
        }

        // ── Drawing ───────────────────────────────────────────
        private void DrawArcs(Size size)
        {
            double cx = size.Width / 2;
            double cy = size.Height / 2;
            double thickness = ArcThickness;
            double radius = Math.Min(cx, cy) - thickness / 2 - 2;

            // clamp value between 0.01 and 99.99 to avoid degenerate arcs
            double pct = Math.Max(0.01, Math.Min(99.99, Value));

            // ── Background ring ───────────────────────────────
            double bgSize = radius * 2 + thickness;
            _background.Width = bgSize;
            _background.Height = bgSize;
            _background.StrokeThickness = thickness;
            _background.Stroke = new SolidColorBrush(Color.FromRgb(240, 242, 248));
            Canvas.SetLeft(_background, cx - radius - thickness / 2);
            Canvas.SetTop(_background, cy - radius - thickness / 2);

            // ── Primary arc (e.g. Import) ─────────────────────
            _primaryArc.Stroke = PrimaryColor;
            _primaryArc.StrokeThickness = thickness;
            _primaryArc.StrokeStartLineCap = PenLineCap.Round;
            _primaryArc.StrokeEndLineCap = PenLineCap.Round;
            _primaryArc.Data = BuildArc(cx, cy, radius, 0, pct);

            // ── Secondary arc (e.g. Export) ───────────────────
            _secondaryArc.Stroke = SecondaryColor;
            _secondaryArc.StrokeThickness = thickness;
            _secondaryArc.StrokeStartLineCap = PenLineCap.Round;
            _secondaryArc.StrokeEndLineCap = PenLineCap.Round;
            _secondaryArc.Data = BuildArc(cx, cy, radius, pct, 100);
        }

        private static PathGeometry BuildArc(
            double cx, double cy, double radius,
            double startPct, double endPct)
        {
            double startAngle = startPct / 100.0 * 360.0 - 90.0;
            double endAngle = endPct / 100.0 * 360.0 - 90.0;

            double x1 = cx + radius * Math.Cos(DegToRad(startAngle));
            double y1 = cy + radius * Math.Sin(DegToRad(startAngle));
            double x2 = cx + radius * Math.Cos(DegToRad(endAngle));
            double y2 = cy + radius * Math.Sin(DegToRad(endAngle));

            var figure = new PathFigure
            {
                StartPoint = new Point(x1, y1),
                IsClosed = false
            };

            figure.Segments.Add(new ArcSegment
            {
                Point = new Point(x2, y2),
                Size = new Size(radius, radius),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = (endPct - startPct) > 50,
                RotationAngle = 0
            });

            return new PathGeometry(new[] { figure });
        }

        private static double DegToRad(double degrees)
            => degrees * Math.PI / 180.0;
    }
}
