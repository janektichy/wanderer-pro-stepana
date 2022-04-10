using System;
using System.Collections.Generic;
using System.Text;
using GreenFox;
using DrawingApplication;
using Avalonia;
using Avalonia.Media;

namespace Wanderer
{
    public class Floor : Tile
    {
        public Floor(FoxDraw foxDraw, int x, int y) : base(foxDraw, x, y)
        {
            this.foxDraw = base.foxDraw;
            this.image.Source = new Avalonia.Media.Imaging.Bitmap(@"../../../../Assets/floorreduced.png");
            foxDraw.AddImage(this.image, x, y);
        }
    }
}