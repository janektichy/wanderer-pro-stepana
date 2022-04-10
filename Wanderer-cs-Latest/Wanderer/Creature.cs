using Avalonia.Controls;
using GreenFox;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wanderer
{
    abstract class Creature
    {
        protected Map map;
        public Avalonia.Controls.Image image;
        protected FoxDraw foxDraw;
        protected int xPos;
        protected int yPos;
        protected int currentHealth;
        protected int maxHealth;
        protected int strikePower;
        protected TextBox stats;
        protected bool isAlive;
        public Creature(FoxDraw foxdraw, int x, int y, Map map)
        {
            this.map = map;
            this.foxDraw = foxdraw;
            this.image = new Image();
            this.xPos = x;
            this.yPos = y;
            this.isAlive = true;

        }
        public bool IsAlive { get { return isAlive; } set { isAlive = value; } }
        public int XPos { get { return xPos; } set { xPos = value; } }
        public int YPos { get { return yPos; } set { yPos = value; } }
        public int CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }
        public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
        public int StrikePower { get { return strikePower; } set { strikePower = value; } }
        public bool IsCreatureOnEdgeOfTheBox(string direction)
        {
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
                    if (this.yPos > j)
                    {
                        return false;
                    }
                    break;
                case "down":
                    if (this.yPos < j + 10)
                    {
                        return false;
                    }
                    break;
                case "left":
                    if (this.xPos > i)
                    { 
                        return false;
                    }
                    break;
                case "right":
                    if (this.xPos < i + 10)
                    {
                        return false;
                    }
                    break;
            }
            return true;
        }
        public bool IsWallInFront(string direction)
        {
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
            int mapPosX = i / this.map.size;
            int mapPosY = j / this.map.size;
            switch (direction)
            {
                case "up":

                    if ((this.map.mapControler[mapPosX, mapPosY - 1] == 1) || (this.map.mapControler[mapPosX + 1, mapPosY - 1] == 1 && this.xPos - 10 > i))
                    {
                        return true;
                    }
                    break;
                case "down":
                    if ((this.map.mapControler[mapPosX, mapPosY + 1] == 1) || (this.map.mapControler[mapPosX + 1, mapPosY + 1] == 1 && this.xPos - 10 > i))
                    {
                        return true;
                    }
                    break;
                case "left":
                    if ((this.map.mapControler[mapPosX - 1, mapPosY] == 1) || (this.map.mapControler[mapPosX - 1, mapPosY + 1] == 1 && this.yPos - 10 > j))
                    {
                        return true;
                    }
                    break;
                case "right":
                    if ((this.map.mapControler[mapPosX + 1, mapPosY] == 1) || (this.map.mapControler[mapPosX + 1, mapPosY + 1] == 1 && this.yPos - 10 > j))
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }
    }
}
