using System;
using System.Collections.Generic;
using System.Text;
using GreenFox;
using DrawingApplication;
using Avalonia;
using System.Drawing;
using Avalonia.Controls;

namespace Wanderer
{
    public class Tile
    {
        public FoxDraw foxDraw;
        public Avalonia.Controls.Image image;
        public Tile(FoxDraw foxDraw, int x, int y)
        {
            this.foxDraw = foxDraw;
            this.image = new Avalonia.Controls.Image();
        }
    }
}
