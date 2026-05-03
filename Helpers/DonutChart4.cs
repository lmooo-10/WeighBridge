using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WeighBridge.Helpers
{
    public class DonutChart4 : Control
    {
        // ── Dependency Properties ──────────────────────────────

        public static readonly DependencyProperty Value1Property =
            DependencyProperty.Register(nameof(Value1), typeof(double), typeof(DonutChart4),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, OnChanged));

        public static readonly DependencyProperty Value2Property =
            DependencyProperty.Register(nameof(Value2), typeof(double), typeof(DonutChart4),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, OnChanged));

        public static readonly DependencyProperty Value3Property =
            DependencyProperty.Register(nameof(Value3), typeof(double), typeof(DonutChart4),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, OnChanged));

        public static readonly DependencyProperty Value4Property =
            DependencyProperty.Register(nameof(Value4), typeof(double), typeof(DonutChart4),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, OnChanged));

        public static readonly DependencyProperty Color1Property =
            DependencyProperty.Register(nameof(Color1), typeof(Brush), typeof(DonutChart4),
                new FrameworkPropertyMetadata(Brushes.Purple, FrameworkPropertyMetadataOptions.AffectsRender, OnChanged));

        public static readonly DependencyProperty Color2Property =
            DependencyProperty.Register(nameof(Color2), typeof(Brush), typeof(DonutChart4),
                new FrameworkPropertyMetadata(Brushes.Red, FrameworkPropertyMetadataOptions.AffectsRender, OnChanged));

        public static readonly DependencyProperty Color3Property =
            DependencyProperty.Register(nameof(Color3), typeof(Brush), typeof(DonutChart4),
                new FrameworkPropertyMetadata(Brushes.Blue, FrameworkPropertyMetadataOptions.AffectsRender, OnChanged));

        public static readonly DependencyProperty Color4Property =
            DependencyProperty.Register(nameof(Color4), typeof(Brush), typeof(DonutChart4),
                new FrameworkPropertyMetadata(Brushes.Orange, FrameworkPropertyMetadataOptions.AffectsRender, OnChanged));

        public static readonly DependencyProperty ArcThicknessProperty =
            DependencyProperty.Register(nameof(ArcThickness), typeof(double), typeof(DonutChart4),
                new FrameworkPropertyMetadata(22.0, FrameworkPropertyMetadataOptions.AffectsRender, OnChanged));

        public double Value1 { get => (double)GetValue(Value1Property); set => SetValue(Value1Property, value); }
        public double Value2 { get => (double)GetValue(Value2Property); set => SetValue(Value2Property, value); }
        public double Value3 { get => (double)GetValue(Value3Property); set => SetValue(Value3Property, value); }
        public double Value4 { get => (double)GetValue(Value4Property); set => SetValue(Value4Property, value); }
        public Brush Color1 { get => (Brush)GetValue(Color1Property); set => SetValue(Color1Property, value); }
        public Brush Color2 { get => (Brush)GetValue(Color2Property); set => SetValue(Color2Property, value); }
        public Brush Color3 { get => (Brush)GetValue(Color3Property); set => SetValue(Color3Property, value); }
        public Brush Color4 { get => (Brush)GetValue(Color4Property); set => SetValue(Color4Property, value); }
        public double ArcThickness { get => (double)GetValue(ArcThicknessProperty); set => SetValue(ArcThicknessProperty, value); }

        // ── Visual Children ───────────────────────────────────
        private readonly Canvas _canvas = new();
        private readonly Ellipse _bg = new() { Fill = Brushes.Transparent };
        private readonly Path _arc1 = new() { Fill = Brushes.Transparent };
        private readonly Path _arc2 = new() { Fill = Brushes.Transparent };
        private readonly Path _arc3 = new() { Fill = Brushes.Transparent };
        private readonly Path _arc4 = new() { Fill = Brushes.Transparent };

        public DonutChart4()
        {
            _canvas.Children.Add(_bg);
            _canvas.Children.Add(_arc1);
            _canvas.Children.Add(_arc2);
            _canvas.Children.Add(_arc3);
            _canvas.Children.Add(_arc4);
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

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DonutChart4 chart) chart.InvalidateArrange();
        }

        private void DrawArcs(Size size)
        {
            double cx = size.Width / 2;
            double cy = size.Height / 2;
            double thickness = ArcThickness;
            double radius = Math.Min(cx, cy) - thickness / 2 - 2;

            // ── Total & percentages ───────────────────────────
            double total = Value1 + Value2 + Value3 + Value4;
            if (total <= 0) total = 1; // avoid division by zero

            double pct1 = Value1 / total * 100;
            double pct2 = Value2 / total * 100;
            double pct3 = Value3 / total * 100;
            double pct4 = Value4 / total * 100;

            // ── Background ring ───────────────────────────────
            double bgSize = radius * 2 + thickness;
            _bg.Width = bgSize;
            _bg.Height = bgSize;
            _bg.StrokeThickness = thickness;
            _bg.Stroke = new SolidColorBrush(Color.FromRgb(240, 242, 248));
            Canvas.SetLeft(_bg, cx - radius - thickness / 2);
            Canvas.SetTop(_bg, cy - radius - thickness / 2);

            // ── Draw 4 segments ───────────────────────────────
            double start = 0;

            DrawSegment(_arc1, Color1, thickness, cx, cy, radius, ref start, pct1);
            DrawSegment(_arc2, Color2, thickness, cx, cy, radius, ref start, pct2);
            DrawSegment(_arc3, Color3, thickness, cx, cy, radius, ref start, pct3);
            DrawSegment(_arc4, Color4, thickness, cx, cy, radius, ref start, pct4);
        }

        private static void DrawSegment(
            Path arc, Brush color, double thickness,
            double cx, double cy, double radius,
            ref double startPct, double spanPct)
        {
            // small gap between segments (1.5%)
            double gap = 1.5;
            double gapHalf = gap / 2;
            double s = startPct + gapHalf;
            double e = startPct + spanPct - gapHalf;

            arc.Stroke = color;
            arc.StrokeThickness = thickness;
            arc.StrokeStartLineCap = PenLineCap.Round;
            arc.StrokeEndLineCap = PenLineCap.Round;
            arc.Data = BuildArc(cx, cy, radius, s, e);

            startPct += spanPct;
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

        private static double DegToRad(double deg) => deg * Math.PI / 180.0;
    }
}