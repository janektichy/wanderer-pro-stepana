using System;
using System.Collections.Generic;
using System.Text;
using GreenFox;

namespace Wanderer
{
    class Skeleton : BadGuy
    {
        public Skeleton(FoxDraw foxdraw, int x, int y, Map map, Hero hero) : base(foxdraw, x, y, map, hero)
        {
            this.image.Source = new Avalonia.Media.Imaging.Bitmap(@"../../../../Assets/Skeletonreduced.png");
            foxDraw.AddImage(this.image, this.xPos, this.yPos);
            this.maxHealth = 50;
            this.currentHealth = this.maxHealth;
            this.strikePower = 5;

        }

    }
}
