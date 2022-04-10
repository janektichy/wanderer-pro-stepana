using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System.Collections.Generic;
using System.Linq;

namespace GreenFox
{
    public class FoxDraw
    {
        private Canvas Canvas { get; set; }
        private SolidColorBrush LineColor { get; set; } = new SolidColorBrush(Colors.Black);
        private SolidColorBrush ShapeColor { get; set; } = new SolidColorBrush(Colors.DarkGreen);

        private int StrokeThickness { get; set; } = 1;

        public FoxDraw(Canvas canvas)
        {
            Canvas = canvas;
            
        }

        public void SetBackgroundColor(Color color)
        {
            Canvas.Background = new SolidColorBrush(color);
        }

        public void SetStrokeThicknes(int thickness)
        {
            StrokeThickness = thickness;
        }

        public void SetStrokeColor(Color color)
        {
            LineColor = new SolidColorBrush(color);
        }

        public void SetFillColor(Color color)
        {
            ShapeColor = new SolidColorBrush(color);
        }

        public void DrawEllipse(double x, double y, double width, double height)
        {
            var ellipse = new Ellipse()
            {
                Stroke = LineColor,
                StrokeThickness = StrokeThickness,
                Fill = ShapeColor,
                Width = width,
                Height = height
            };

            Canvas.Children.Add(ellipse);
            SetPosition(ellipse, x, y);
        }

        public void DrawLine(Point start, Point end)
        {
            var line = new Line()
            {
                Stroke = LineColor,
                StrokeThickness = StrokeThickness,
                StartPoint = start,
                EndPoint = end
            };

            Canvas.Children.Add(line);
        }

        public void DrawLine(double x1, double y1, double x2, double y2)
        {
            DrawLine(new Point(x1, y1), new Point(x2, y2));
        }

        public void DrawPolygon(IEnumerable<Point> points)
        {
            var polygon = new Polygon()
            {
                Stroke = LineColor,
                StrokeThickness = StrokeThickness,
                Fill = ShapeColor,
                Points = points.ToList()
            };

            Canvas.Children.Add(polygon);
        }

        public void DrawRectangle(double x, double y, double width, double height)
        {
            var rectangle = new Rectangle()
            {
                Stroke = LineColor,
                StrokeThickness = StrokeThickness,
                Fill = ShapeColor,
                Width = width,
                Height = height
            };

            Canvas.Children.Add(rectangle);
            SetPosition(rectangle, x, y);
        }

        public void AddImage(Image image, double x, double y)
        {
            Canvas.Children.Add(image);
            SetPosition(image, x, y);
        }

        public void SetPosition(AvaloniaObject shape, double x, double y)
        {
            Canvas.SetLeft(shape, x);
            Canvas.SetTop(shape, y);
        }

        public void AddText(TextBox text)
        {
            Canvas.Children.Add(text);   
        }
        public void RemoveText(TextBox text)
        {
            Canvas.Children.Remove(text);
        }
        public void RemoveImage(Image image)
        {
            Canvas.Children.Remove(image);
        }
        public void ClearBoard()
        {
            for (int i = 0; i < Canvas.Children.Count; i++)
            {
                for (int j = 0; j < Canvas.Children.Count; j++)
                {
                    Canvas.Children.Remove(Canvas.Children[j]);
                }
                Canvas.Children.Remove(Canvas.Children[i]);
            }

        }
        public void DrawText(string text, TextFormatter formatter)
        {
            var textBox = new TextBox()
            {
                Text = text,
                FontSize = formatter.FontSize,
                Foreground = formatter.FontColor,
                IsReadOnly = true,
                BorderThickness = Thickness.Parse(formatter.BorderSize),
                BorderBrush = Brushes.Black
            };
            Canvas.Children.Add(textBox);
            SetPosition(textBox, formatter.Position.X, formatter.Position.Y);
        }
        public class TextFormatter
        {
            public double FontSize { get; private set; }
            public IBrush FontColor { get; private set; }
            public Point Position { get; private set; }
            public string BorderSize { get; private set; }


            public TextFormatter(double fontSize, int borderSize, IBrush fontColor, Point position)
            {
                FontSize = fontSize;
                FontColor = fontColor;
                Position = position;
                BorderSize = borderSize.ToString();
            }
        }
    }
}
