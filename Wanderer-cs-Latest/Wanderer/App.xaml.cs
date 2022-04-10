using Avalonia;
using Avalonia.Markup.Xaml;

namespace DrawingApplication
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
