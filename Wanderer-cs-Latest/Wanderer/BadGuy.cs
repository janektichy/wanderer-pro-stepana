using Avalonia.Threading;
using GreenFox;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace Wanderer
{
    abstract class BadGuy : Creature
    {
        protected Random random;
        protected int halfChance;
        protected int negOrPos;
        protected bool didMove;
        protected Hero hero;
        protected bool wasHeroAttacked;
        protected int timerInterval;
        protected int[] previousPosition;
        protected bool isItFirstAttack;
        protected double attackInterval;
        protected double moveInterval;
        public DispatcherTimer timerMove;
        public DispatcherTimer timerAttack;
        protected int step;

        public double AttackInterval { get { return attackInterval; } set { attackInterval = value; } }
        public double MoveInterval { get { return moveInterval; } set { moveInterval = value; } }
        public BadGuy(FoxDraw foxdraw, int x, int y, Map map, Hero hero) : base(foxdraw, x, y, map)
        {
            this.timerMove = new DispatcherTimer();
            this.timerAttack = new DispatcherTimer();
            this.didMove = false;
            this.random = new Random();
            this.halfChance = 0;
            this.negOrPos = 1;
            this.hero = hero;
            this.timerInterval = 30;
            this.wasHeroAttacked = false;
            this.previousPosition = new int[2];
            this.isItFirstAttack = true;
            this.AttackInterval = 1000;
            this.MoveInterval = 30;
            this.step = 2;
        }
        public void MoveOrAttack()
        {
            this.timerMove.Interval = TimeSpan.FromMilliseconds(300);
            this.timerMove.Tick += ChooseActionTicker;
            this.timerMove.Start();
        }

        public void ChooseActionTicker(object source, EventArgs e)
        {
            
            if (this.wasHeroAttacked)
            {
                TakeAStepBack(this.previousPosition);
                this.wasHeroAttacked = false;
            }
            else if (this.isItFirstAttack && IsHeroInRange())
            {
                this.timerMove.Interval = TimeSpan.FromMilliseconds(this.attackInterval);
                WaitASecond();
                this.isItFirstAttack = false;
            }
            else
            {
                if (IsHeroInRange())
                {
                    this.timerMove.Interval = TimeSpan.FromMilliseconds(this.attackInterval);
                    this.previousPosition = this.AttackHero();
                    this.wasHeroAttacked = true;
                    if (IsHeroInRange())
                    {
                        this.hero.CurrentHealth -= this.strikePower;
                    }
                }
                if (!IsWallBetweenUs() && !IsHeroInRange())
                {
                    this.isItFirstAttack = true;
                    this.timerMove.Interval = TimeSpan.FromMilliseconds(this.moveInterval);
                    MoveToHero();
                }
                else if (!IsHeroInRange())
                {
                    this.isItFirstAttack = true;
                    this.timerMove.Interval = TimeSpan.FromMilliseconds(this.moveInterval);
                    MoveCreature();
                }
            }
            if (this.currentHealth <= 0)
            {
                foxDraw.RemoveImage(this.image);
                this.xPos = 0;
                this.yPos = 1000;
                this.timerMove.Tick -= ChooseActionTicker;
                this.timerMove.Stop();
                hero.Score += 10;
                this.isAlive = false;
            }
        }
        public int[] AttackHero()
        {
            int[] previousPosition = new int[] { this.xPos, this.yPos };
            foxDraw.SetPosition(this.image, hero.XPos, hero.YPos);
            this.xPos = hero.XPos;
            this.YPos = hero.YPos;
            return previousPosition;
        }
        public void TakeAStepBack(int[] previousPosition)
        {
            foxDraw.SetPosition(this.image, previousPosition[0], previousPosition[1]);
            this.xPos = previousPosition[0];
            this.YPos = previousPosition[1];
        }
        public void WaitASecond()
        {
        }

        public void MoveToHero()
        {
            int[,] positions = GetMapPositions();
            int mapPosXMe = positions[0, 0];
            int mapPosYMe = positions[0, 1];
            int mapPosXHim = positions[1, 0];
            int mapPosYHim = positions[1, 1];


            if (mapPosXMe == mapPosXHim && this.YPos < hero.YPos - 30)
            {
                this.didMove = TakeAStep(0, step, "down");
            }
            else if (mapPosXMe == mapPosXHim && this.YPos > hero.YPos + 30)
            {
                this.didMove = TakeAStep(0, -step, "up");
            }
            else if (mapPosYMe == mapPosYHim && this.XPos < hero.XPos + 30)
            {
                this.didMove = TakeAStep(step, 0, "right");
            }
            else if (mapPosYMe == mapPosYHim && this.XPos > hero.XPos - 30)
            {
                this.didMove = TakeAStep(-step, 0, "left");
            }
        }
        public void MoveCreature()
        {
            int willChangeDirection = random.Next(0, 1000);
            if (!this.didMove || !( willChangeDirection < 970))
            {
                this.halfChance = random.Next(0, 2);
                this.negOrPos = random.Next(0, 2);
            }

            if (this.halfChance == 0)
            {
                if (this.negOrPos == 0)
                {
                    this.didMove = TakeAStep(step, 0, "right");
                }
                else
                {
                    this.didMove = TakeAStep(-step, 0, "left");
                }
            }
            else
            {
                if (this.negOrPos == 0)
                {
                    this.didMove = TakeAStep(0, step, "down");
                }
                else
                {
                    this.didMove = TakeAStep(0, -step, "up");
                }
            }
        }
        public bool TakeAStep(int x, int y, string direction)
        {

            if (!IsWallInFront(direction) || !IsCreatureOnEdgeOfTheBox(direction))
            {
                foxDraw.SetPosition(this.image, this.xPos + x, this.yPos + y);
                this.xPos += x;
                this.yPos += y;
                return true;
            }
            return false;
        
        }
        public bool IsWallBetweenUs()
        {
            int[,] positions = GetMapPositions();
            int mapPosXMe = positions[0, 0];
            int mapPosYMe = positions[0, 1];
            int mapPosXHim = positions[1, 0];
            int mapPosYHim = positions[1, 1];

            if (mapPosXMe == mapPosXHim && mapPosYMe < mapPosYHim)
            {
                for (; mapPosYMe < mapPosYHim; mapPosYMe++)
                {
                    if (this.map.mapControler[mapPosXMe, mapPosYMe] == 1)
                    {
                        return true;
                    }
                }
            }
            else if (mapPosXMe == mapPosXHim && mapPosYMe > mapPosYHim)
            {
                for (; mapPosYMe > mapPosYHim; mapPosYMe--)
                {
                    if (this.map.mapControler[mapPosXMe, mapPosYMe] == 1)
                    {
                        return true;
                    }
                }
            }
            else if (mapPosYMe == mapPosYHim && mapPosXHim > mapPosXMe)
            {
                for (; mapPosXMe < mapPosXHim; mapPosXMe++)
                {
                    if (this.map.mapControler[mapPosXMe, mapPosYMe] == 1)
                    {
                        return true;
                    }
                }
            }
            else if (mapPosYMe == mapPosYHim && mapPosXHim < mapPosXMe)
            {
                for (; mapPosXHim < mapPosXMe; mapPosXMe--)
                {
                    if (this.map.mapControler[mapPosXMe, mapPosYMe] == 1)
                    {
                        return true;
                    }
                }
            }
            else if (mapPosYMe != mapPosYHim && mapPosXMe != mapPosXHim)
            {
                return true;
            }
            return false;
        }
        
        public int[,] GetMapPositions()
        {
            int[,] positions = new int[2, 2];
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
            positions[0, 0] = i / this.map.size;
            positions[0, 1] = j / this.map.size;

            int k = this.hero.XPos;
            while (k % this.map.size != 0)
            {
                --k;
            }
            int l = this.hero.YPos;
            while (l % this.map.size != 0)
            {
                --l;
            }
            positions[1, 0] = k / this.map.size;
            positions[1, 1] = l / this.map.size;

            return positions;
        }
        public bool IsHeroInRange()
        {
            double vektorX = this.xPos - this.hero.XPos;
            double vektorY = this.yPos - this.hero.YPos;

            if ((vektorX < 31 && vektorX > -31 && vektorY > -31 && vektorY < 31))
            {
                return true;
            }
            return false;
        }
    }
}
