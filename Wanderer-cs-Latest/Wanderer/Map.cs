using System;
using System.Collections.Generic;
using System.Text;
using GreenFox;
using DrawingApplication;
using Avalonia;

namespace Wanderer
{
    class Map
    {
        public FoxDraw foxDraw;
        public int size;
        public int[,] mapControler;
        public int mapSide;
        public List<Tile> tileList;
        public Map(FoxDraw foxDraw)
        {
            this.foxDraw = foxDraw;
            this.size = 50;
            this.mapSide = 15;
            this.mapControler = new int[this.mapSide, this.mapSide];
            this.tileList = new List<Tile>();
        }
        public void ClearMap()
        {
            for (int i = 0; i < this.mapSide; i++)
            {
                for (int j = 0; j < this.mapSide; j++)
                {
                    this.mapControler[i, j] = 0;
                }
            }
        }
        public void MakeRandomMap()
        {
            FillBorder();
            int zeroCount = 0;
            do
            {
                zeroCount = 0;
                Random random = new Random();
                int randomX = random.Next(1, this.mapSide-1) * this.size;
                int randomY = random.Next(1, this.mapSide-1) * this.size;
                if (CountWallAround(randomX, randomY) <= 1)
                {
                    MakeRandomObjectWall(randomX, randomY);
                }
               
                for (int i = 0; i < this.mapSide; i++)
                {
                    for (int j = 0; j < this.mapSide; j++)
                    {
                        if (this.mapControler[i, j] == 0)
                        {
                            zeroCount++;
                        }
                    }
                }
            } while (zeroCount > 0);
            
        }
        public void MakeRandomObjectWall(int currentX, int currentY)
        {
            Random random = new Random();
            int length = random.Next(22, 30);
            
            int count = 0;
            
            while (count < length)
            {
                int y = currentY / this.size;
                int x = currentX / this.size;
                bool isThereSpace = (this.mapControler[x, y] != 1);
                if (isThereSpace && CountWallAround(currentX, currentY) <= 2)
                {
                    int halfChance = random.Next(0, 2);
                    if (halfChance == 0)
                    {
                        int negOrPos = random.Next(0, 2);
                        if (negOrPos == 0 && x > 0)
                        {
                            PlaceWall(currentX, currentY);
                            currentX -= this.size;
                            count++;
                        }
                        else if (x < this.mapSide - 1)
                        {
                            PlaceWall(currentX, currentY);
                            currentX += this.size;
                            count++;
                        }
                            
                    }
                    else
                    {
                        int negOrPos = random.Next(0, 2);
                        if (negOrPos == 0 && y > 0)
                        {
                            PlaceWall(currentX, currentY);
                            currentY -= this.size;
                            count++;
                        }
                        else if (y < this.mapSide - 1)
                        {
                            PlaceWall(currentX, currentY);
                            currentY += this.size;
                            count++;
                        }   
                    }
                }
                else
                {
                    break;
                }
            }
        }
        public void PlaceFloorAround(int currentX, int currentY)
        {
            int y = currentY / this.size;
            int x = currentX / this.size;

            List<int[]> allPositions = new List<int[]>();
            allPositions.Add(new int[] { x + 1, y});
            allPositions.Add(new int[] { x - 1, y });
            allPositions.Add(new int[] { x, y + 1});
            allPositions.Add(new int[] { x, y - 1});
            allPositions.Add(new int[] { x + 1, y + 1 });
            allPositions.Add(new int[] { x + 1, y - 1 });
            allPositions.Add(new int[] { x - 1, y + 1 });
            allPositions.Add(new int[] { x - 1, y - 1 });

            List<int[]> allPositions2 = new List<int[]>();
            allPositions2.Add(new int[] { currentX + this.size, currentY });
            allPositions2.Add(new int[] { currentX - this.size, currentY });
            allPositions2.Add(new int[] { currentX, currentY + this.size });
            allPositions2.Add(new int[] { currentX, currentY - this.size });
            allPositions2.Add(new int[] { currentX + this.size, currentY + this.size });
            allPositions2.Add(new int[] { currentX + this.size, currentY - this.size });
            allPositions2.Add(new int[] { currentX - this.size, currentY + this.size });
            allPositions2.Add(new int[] { currentX - this.size, currentY - this.size });

            for (int i = 0; i < allPositions.Count; i++)
            {
                int xPos = allPositions[i][0];
                int yPos = allPositions[i][1];

                if ((this.mapControler[xPos, yPos] == 0) && (xPos > 0 && xPos < this.mapSide - 1 && yPos > 0 && yPos < this.mapSide - 1))
                {
                    Floor tile = new Floor(this.foxDraw, allPositions2[i][0], allPositions2[i][1]);
                    this.tileList.Add(tile);
                    this.mapControler[xPos, yPos] = 2;
                }
            }
        }
        public void FillBorder()
        {
            for (int i = 0; i < this.mapSide; i++)
            {
                for (int j = 0; j < this.mapSide; j++)
                {
                    if (i == this.mapSide - 2 || i == 1 || j == this.mapSide - 2 || j == 1)
                    {
                        this.mapControler[i, j] = 2;
                        Floor tile = new Floor(this.foxDraw, i * this.size, j * this.size);
                        this.tileList.Add(tile);
                    }
                    if (i == this.mapSide - 1 || i == 0 || j == this.mapSide - 1 || j == 0)
                    {
                        this.mapControler[i, j] = 1;
                        Wall tile = new Wall(this.foxDraw, i * this.size, j * this.size);
                        this.tileList.Add(tile);
                    }
                }
            }
        }
        public void PrintMapStatus(Map map)
        {
            for (int line = 0; line < this.mapSide; line++)
            {
                for (int row = 0; row < this.mapSide; row++)
                {
                    Console.Write(this.mapControler[row, line] + " ");
                }
                Console.WriteLine();
            }
        }
        public void PlaceWall(int currentX, int currentY)
        {
            Wall wallTile = new Wall(this.foxDraw, currentX, currentY);
            this.tileList.Add(wallTile);
            this.mapControler[currentX/this.size, currentY/this.size] = 1;
            PlaceFloorAround(currentX, currentY);
        }
        public int CountWallAround(int currentX, int currentY)
        {
            int x = currentX / this.size;
            int y = currentY / this.size;
            List<int[]> allPositions = new List<int[]>();
            allPositions.Add(new int[] { x + 1, y });
            allPositions.Add(new int[] { x - 1, y });
            allPositions.Add(new int[] { x, y + 1 });
            allPositions.Add(new int[] { x, y - 1 });
            allPositions.Add(new int[] { x + 1, y + 1 });
            allPositions.Add(new int[] { x + 1, y - 1 });
            allPositions.Add(new int[] { x - 1, y + 1 });
            allPositions.Add(new int[] { x - 1, y - 1 });

            int wallsAroundCount = 0;
            if (x > 0 && x < this.mapSide - 1 && y > 0 && y < this.mapSide - 1)
            {
                foreach (var position in allPositions)
                {
                    if (this.mapControler[position[0], position[1]] == 1)
                    {
                        wallsAroundCount++;
                    }
                }
            }
            return wallsAroundCount;
        }
    }
}
