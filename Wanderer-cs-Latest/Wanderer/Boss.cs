using GreenFox;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wanderer
{
    class Boss : BadGuy
    {
        public Boss(FoxDraw foxdraw, int x, int y, Map map, Hero hero) : base(foxdraw, x, y, map, hero)
        {
            this.image.Source = new Avalonia.Media.Imaging.Bitmap(@"../../../../Assets/bossReduced.png");
            foxDraw.AddImage(this.image, this.xPos, this.yPos);
            this.maxHealth = 120;
            this.currentHealth = this.maxHealth;
            this.strikePower = 15;
        }

    }
}
