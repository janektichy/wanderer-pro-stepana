using Avalonia.Input;
using Avalonia.Threading;
using GreenFox;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wanderer
{
    class Hero : Creature
    {
        public Dictionary<Key, bool> keyPress;
        protected DispatcherTimer timerMoveHero;
        protected List<BadGuy> enemies;
        protected int score;
        protected int stage;
        protected int time;
        public int Score { get { return score; } set { score = value; } }
        public int Time { get { return time; } set { time = value; } }
        public int Stage { get { return stage; } set { stage = value; } }
        public Hero (FoxDraw foxdraw, int x, int y, Map map) : base(foxdraw, x, y, map)
        {
            this.image.Source = new Avalonia.Media.Imaging.Bitmap(@"../../../../Assets/heroDownReduced.png");
            foxDraw.AddImage(image, xPos, yPos);
            this.maxHealth = 100;
            this.currentHealth = this.maxHealth;
            this.strikePower = 10;
            this.timerMoveHero = new DispatcherTimer();
            this.enemies = new List<BadGuy>();
            this.keyPress = new Dictionary<Key, bool>();
            this.score = 0;
            this.stage = 0;
            keyPress.Add(Key.Up, false);
            keyPress.Add(Key.Down, false);
            keyPress.Add(Key.Right, false);
            keyPress.Add(Key.Left, false);
            keyPress.Add(Key.Space, false);
            keyPress.Add(Key.A, false);
            keyPress.Add(Key.LeftShift, false);
        }
        public List<BadGuy> Enemies { get { return enemies; } set { enemies = value; } }
        public void Move()
        {
            this.timerMoveHero.Interval = TimeSpan.FromMilliseconds(1000);
            this.timerMoveHero.Tick += ChooseActionTicker;
            this.timerMoveHero.Start();
        }
        public void ChooseActionTicker(object source, EventArgs e)
        {

        }
        public void MoveHero(int x, int y, string direction, string path)
        {
            this.image.Source = new Avalonia.Media.Imaging.Bitmap(path);
            int i = this.xPos;
            while (i % this.map.size != 0)
            {
                --i;
            }
            int j = this.yPos;
            while (j % this.map.size != 0)
            {
                --j;
            }
 
            switch (direction)
            {
            case "up":
                    if ((!IsWallInFront(direction) || !IsCreatureOnEdgeOfTheBox(direction)))
                    {
                        MoveHeroToDirection(x, y);
                    }
                break;
            case "down":
                    if ((!IsWallInFront(direction) || !IsCreatureOnEdgeOfTheBox(direction)))
                    {
                        MoveHeroToDirection(x, y);
                    }
                break;
            case "left":
                    if ((!IsWallInFront(direction) || !IsCreatureOnEdgeOfTheBox(direction)))
                    {
                        MoveHeroToDirection(x, y);
                    }
                break;
            case "right":
                    if ((!IsWallInFront(direction) || !IsCreatureOnEdgeOfTheBox(direction)))
                    {
                        MoveHeroToDirection(x, y);
                    }
                break;
            }
        }
        public void KickSkeletonInFace()
        {

        }
        public void MoveHeroToDirection(int x, int y)
        {
            foxDraw.SetPosition(this.image, this.xPos + x, this.yPos + y);
            this.xPos += x;
            this.yPos += y;
        }
        public List<bool> IsAnyEnemyInRange()
        {
            List<bool> isInRangeByIndex = new List<bool>();
            for (int i = 0; i < this.enemies.Count; i++)
            {
                double vektorX = this.xPos - this.enemies[i].XPos;
                double vektorY = this.yPos - this.enemies[i].YPos;

                if ((vektorX < 31 && vektorX > -31 && vektorY > -31 && vektorY < 31))
                {
                    bool isInRange = true;
                    isInRangeByIndex.Add(isInRange);
                }
                else
                {
                    bool isNotInRange = false;
                    isInRangeByIndex.Add(isNotInRange);
                }
            }
            return isInRangeByIndex;
        }
    }
}
