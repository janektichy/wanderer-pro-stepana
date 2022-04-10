using System;
using System.Collections.Generic;
using System.Text;
using GreenFox;
using DrawingApplication;
using Avalonia;
using Avalonia.Media;

namespace Wanderer
{
    public class Wall : Tile
    {
        public Wall(FoxDraw foxDraw, int x, int y) : base(foxDraw, x, y)
        {
            this.foxDraw = foxDraw;
            //this.image = new Avalonia.Controls.Image();
            this.image.Source = new Avalonia.Media.Imaging.Bitmap(@"../../../../Assets/wallreduced.png");
            foxDraw.AddImage(this.image, x, y);
        }
    }
}
