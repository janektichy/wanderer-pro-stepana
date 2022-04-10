using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using GreenFox;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using static GreenFox.FoxDraw;



namespace Wanderer
{
    class Level
    {
        protected Hero hero;
        protected Map map;
        protected Boss boss;
        protected FoxDraw foxDraw;
        protected Random random;
        protected int skeletonCount;
        protected List<Skeleton> skeletonList;
        protected DispatcherTimer statusCheck;
        protected int Difficulty;
        protected string playerName;
        protected string LBpath;
        public Level(FoxDraw foxDraw, Hero hero, Map map)
        {
            this.foxDraw = foxDraw;
            this.map = map;
            this.hero = hero;
            this.random = new Random();
            this.boss = new Boss(foxDraw, 50, 50, this.map, this.hero);
            this.skeletonCount = 2;
            this.skeletonList = new List<Skeleton>();
            this.statusCheck = new DispatcherTimer();
            this.Difficulty = 100;
            this.LBpath = @"LeaderBoard.txt";
            this.playerName = "";
        }
        public void AddScoreToLeaderBoard()
        {
            string[] fileLines = File.ReadAllLines(LBpath);
            Dictionary<string, int> splited = new Dictionary<string, int>();
            for (int i = 0; i < fileLines.Length; i++)
            {
                string[] scoreAndName = fileLines[i].Split(";");
                splited.Add(scoreAndName[0], int.Parse(scoreAndName[1])); 
            }
            List<int> sorted = new List<int>();
            sorted.Add(hero.Score);
            foreach (KeyValuePair<string, int> score in splited)
            {
                if (score.Value < sorted[sorted.Count - 1])
                {
                    sorted.Add(score.Value);
                }
                else
                {
                    int listCount = sorted.Count;
                    for (int i = 0; i < listCount; i++)
                    {
                        if (score.Value >= sorted[i])
                        {
                            sorted.Insert(i, score.Value);
                        }
                    }
                }
            }
            Dictionary<string, int> sortedDic = new Dictionary<string, int>();
            for (int i = 0; i < sorted.Count; i++)
            {
                if (sorted[i] == hero.Score && !sortedDic.ContainsKey(this.playerName))
                {
                    sortedDic.Add(this.playerName, this.hero.Score);
                }
                foreach (KeyValuePair<string, int> score in splited)
                {
                    if (sorted[i] == score.Value && !sortedDic.ContainsKey(score.Key))
                    {
                        sortedDic.Add(score.Key, score.Value);
                    }
                }
            }
            using (StreamWriter writer = new StreamWriter(LBpath))
            {
                foreach (KeyValuePair<string, int> score in sortedDic)
                {
                    writer.WriteLine($"{score.Key};{score.Value}");
                }
            }
        }
        public void ShowLeaderBoard()
        {
            string[] fileLines = File.ReadAllLines(LBpath);
            string[,] topFivePlayers;
            string textTopPlayers = "";
            StringWriter writer = new StringWriter();
            writer.WriteLine("Top Scorers:");
            if (fileLines.Length < 5)
            {
                topFivePlayers = new string[fileLines.Length, 2];
            }
            else
            {
            topFivePlayers = new string[5, 2];
            }
            for (int i = 0; i < topFivePlayers.GetLength(0); i++)
            {
                string[] splited = fileLines[i].Split(';');
                topFivePlayers[i,0] = splited[0];
                topFivePlayers[i, 1] = splited[1];
                writer.WriteLine($"Player: { topFivePlayers[i, 0]}     Score: { topFivePlayers[i, 1]}");
            }
            textTopPlayers = writer.ToString();
            TextBox showStats = new TextBox();


            showStats.Text = textTopPlayers;
            showStats.FontSize = 25;
            showStats.Width = 500;
            showStats.Height = topFivePlayers.GetLength(0) * 48;
            foxDraw.AddText(showStats);
            foxDraw.SetPosition(showStats,150, 405);

        }
        public void CreatePlayer()
        {
            Console.WriteLine("Pick your player name. It may be anything but you can not change it atferwards");
            this.playerName = Console.ReadLine();
            bool isFree = false;
            string[] fileLines = File.ReadAllLines(LBpath);
            do
            {
                if (this.playerName.Length == 0)
                {
                    Console.WriteLine("You have to give me something, common man, one letter atleast");
                    this.playerName = Console.ReadLine();
                }
                for (int i = 0; i < fileLines.Length; i++)
                {
                    if (!fileLines[i].Contains(this.playerName))
                    {
                        isFree = true;
                    }
                }
                while (!isFree)
                {
                    Console.WriteLine("This name is already taken, pick another one please");
                    this.playerName = Console.ReadLine();
                    for (int i = 0; i < fileLines.Length; i++)
                    {
                        if (!fileLines[i].Contains(this.playerName))
                        {
                            isFree = true;
                        }
                    }
                }

            } while (this.playerName.Length == 0);
            
            Console.WriteLine("Great, enjoy the game!" +
                "\nPress enter when you are ready, there is no pausing :)");
            Console.ReadLine();
        }
        public void StartGame()
        {
            foxDraw.RemoveImage(hero.image);
            map.ClearMap();
            map.MakeRandomMap();
            foxDraw.AddImage(hero.image, 50, 50);
            foxDraw.RemoveImage(this.boss.image);
            hero.XPos = 50;
            hero.YPos = 50;
            if (hero.Stage == 0)
            {
                GenerateBoss();
                GenerateSkeleton();
                GenerateSkeleton();
                hero.Stage++;
            }
            else
            {
                NextLevel();
            }
            for (int i = 0; i < hero.Enemies.Count; i++)
            {
                hero.Enemies[i].MoveOrAttack();
            }
            CheckStatus();
            
        }
        public void NextLevel()
        {
            hero.Score += (this.Difficulty - hero.Time) + hero.CurrentHealth/2;
            hero.Time = 0;
            this.Difficulty += 25;
            hero.Stage++;
            RespawnEnemies();
            if (hero.Stage % 3 == 0)
            {
                GenerateSkeleton();
                hero.Enemies[hero.Enemies.Count - 1].MaxHealth = hero.Enemies[2].MaxHealth;
                hero.Enemies[hero.Enemies.Count - 1].StrikePower = hero.Enemies[2].StrikePower;
                hero.Enemies[hero.Enemies.Count - 1].MoveInterval = hero.Enemies[2].MoveInterval;
                hero.Enemies[hero.Enemies.Count - 1].AttackInterval = hero.Enemies[2].AttackInterval;
                this.skeletonCount++;
            }
            if (hero.Stage > 1)
            {
                hero.MaxHealth += 10;
                hero.CurrentHealth = hero.MaxHealth;
                hero.StrikePower += 2;
                for (int i = 1; i < hero.Enemies.Count; i++)
                {
                    hero.Enemies[i].MaxHealth += 5;
                    hero.Enemies[i].CurrentHealth = hero.Enemies[i].MaxHealth;
                    hero.Enemies[i].StrikePower += 1;
                    hero.Enemies[i].MoveInterval -= 2;
                    hero.Enemies[i].AttackInterval -= 2;
                }
                hero.Enemies[0].MaxHealth += 15;
                hero.Enemies[0].CurrentHealth = hero.Enemies[0].MaxHealth;
                hero.Enemies[0].StrikePower += 3;
                hero.Enemies[0].MoveInterval -= 2;
                hero.Enemies[0].AttackInterval -= 2;
            }
        }
        public void CheckStatus()
        {
            this.statusCheck.Interval = TimeSpan.FromMilliseconds(100);
            this.statusCheck.Tick += StatusCheckTicker;
            this.statusCheck.Start();
        }
        public void StatusCheckTicker(object source, EventArgs e)
        {
            if (this.hero.CurrentHealth <= 0)
            {
                GameOver();
                this.statusCheck.Stop();
            }
            int countAliveEnemies = 0;
            foreach  (BadGuy enemy in hero.Enemies)
            {
                if (enemy.IsAlive)
                {
                    countAliveEnemies++;
                }
            }
            if (countAliveEnemies == 0)
            {
                this.statusCheck.Stop();
                StartGame();
            }
        }
        public void ClearBoard()
        {
            foreach (Tile tile in this.map.tileList)
            {
                foxDraw.RemoveImage(tile.image);
            }
            foxDraw.RemoveImage(hero.image);
            foreach (BadGuy creature in hero.Enemies)
            {
                foxDraw.RemoveImage(creature.image);
            }
        }
        public void GameOver()
        {
            ClearBoard();
            TextBox gameOver = new TextBox();
            gameOver.Text = $" Game Over" +
                $"\n Your Score:" +
                $"\n-----{hero.Score}------";
            gameOver.FontSize = 80;
            gameOver.Width = 500;
            gameOver.Height = 350;
            foxDraw.AddText(gameOver);
            foxDraw.SetPosition(gameOver, 150, 50);
            AddScoreToLeaderBoard();
            ShowLeaderBoard();
        }
        public void GenerateBoss()
        {
            bool isBossMade = false;
            do
            {
                int randomX = random.Next(9, 13) * this.map.size;
                int randomY = random.Next(9, 13) * this.map.size;
                if (this.map.mapControler[randomX / this.map.size, randomY / this.map.size] != 1)
                {
                    foxDraw.AddImage(this.boss.image, randomX, randomY);
                    this.boss.XPos = randomX;
                    this.boss.YPos = randomY;
                    isBossMade = true;
                    this.hero.Enemies.Add(this.boss);
                }
            } while (!isBossMade);
        }
        public void GenerateSkeleton()
        {
            bool isSkeletonMade = false;
            do
            {
                int randomX = random.Next(4, 11) * this.map.size;
                int randomY = random.Next(4, 11) * this.map.size;
                if (this.map.mapControler[randomX / this.map.size, randomY / this.map.size] != 1)
                {
                    Skeleton skeleton = new Skeleton(foxDraw, randomX, randomY, map, hero);
                    skeleton.XPos = randomX;
                    skeleton.YPos = randomY;
                    isSkeletonMade = true;
                    this.hero.Enemies.Add(skeleton);
                }
            } while (!isSkeletonMade);
        }
        public void RespawnEnemies()
         {
            for (int i = 0; i < hero.Enemies.Count; i++)
            {
                bool isBadGuyRespawned = false;
                do
                {
                    int randomX = random.Next(4, 13) * this.map.size;
                    int randomY = random.Next(4, 13) * this.map.size;
                    if (this.map.mapControler[randomX / this.map.size, randomY / this.map.size] != 1)
                    {
                        foxDraw.AddImage(hero.Enemies[i].image, randomX, randomY);
                        hero.Enemies[i].XPos = randomX;
                        hero.Enemies[i].YPos = randomY;
                        //foxDraw.SetPosition(hero.Enemies[i].image, randomX, randomY);
                        hero.Enemies[i].IsAlive = true;
                        isBadGuyRespawned = true;
                    }
                } while (!isBadGuyRespawned);
            }      
        }
    }
}
