using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using GreenFox;
using System;
using System.Collections.Generic;
using System.Timers;
using Wanderer;

namespace DrawingApplication
{
    public class MainWindow : Window
    {
        Hero hero;
        Map myMap;
        DispatcherTimer timer;
        DispatcherTimer scoreTimer;
        TextBox stats;
        TextBox scoreCounter;
        FoxDraw foxDraw;
        DispatcherTimer heroJumpBan;
        int jumpDelay;
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            var canvas = this.Get<Canvas>("canvas");
            this.foxDraw = new FoxDraw(canvas);
            this.timer = new DispatcherTimer();
            this.scoreTimer = new DispatcherTimer();
            this.heroJumpBan = new DispatcherTimer();
            this.jumpDelay = 0;
            CountScore();
            Check();               
            myMap = new Map(foxDraw);
            hero = new Hero(foxDraw, 50, 50, myMap);
            Level level = new Level(foxDraw, hero, myMap);
            level.CreatePlayer();
            level.StartGame();
            this.KeyDown += KeyPressed;
            this.KeyUp += KeyOff;

            
            this.scoreCounter = new TextBox();
            this.scoreCounter.IsVisible = true;
            this.scoreCounter.Text = $"Current Time: {this.hero.Time}";
            foxDraw.AddText(this.scoreCounter);
            foxDraw.SetPosition(this.scoreCounter, 760, 500);
            this.scoreCounter.FontSize = 23;
            this.scoreCounter.Width = 200;
            this.scoreCounter.Height = 50;

            this.stats = new TextBox();
            this.stats.IsVisible = true;
            foxDraw.AddText(this.stats);
            foxDraw.SetPosition(this.stats, 760, 100);
            this.stats.FontSize = 20;
            this.stats.Width = 200;
            this.stats.Height = 400;

        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void Check()
        {
            this.timer.Interval = TimeSpan.FromMilliseconds(2);
            this.timer.Tick += CheckKeyState;
            this.timer.Start();
        }
        public void CountScore()
        {
            this.scoreTimer.Interval = TimeSpan.FromSeconds(1);
            this.scoreTimer.Tick += AddSecond;
            this.scoreTimer.Start();
        }
        public void AddSecond(object source, EventArgs e)
        {
            foxDraw.RemoveText(this.scoreCounter);
            if (hero.CurrentHealth > 0)
            {
                this.hero.Time++;
                this.scoreCounter.Text = $"Current Time: {this.hero.Time}";
                foxDraw.AddText(this.scoreCounter);
            }
        }
        public void BanJump()
        {
            this.heroJumpBan.Interval = TimeSpan.FromSeconds(1);
            this.heroJumpBan.Tick += CountTheDelay;
            this.heroJumpBan.Start();
        }
        public void CountTheDelay(object source, EventArgs e)
        {
            if (jumpDelay == 0)
            {
                this.jumpDelay++;
            }
            else
            {
                this.heroJumpBan.Stop();
            }
        }
        public void CheckKeyState(object source, EventArgs e)
        {
            foxDraw.RemoveText(this.stats);
            if (this.hero.CurrentHealth > 0)
            {
                this.stats.Text =
                    $"Level: {this.hero.Stage}" +
                    $"\n" +
                    $"\nYour Hero:" +
                    $"\nHealth: {this.hero.CurrentHealth}/{this.hero.MaxHealth}" +
                    $"\nStrikePower {this.hero.StrikePower}" +
                    $"\nScore {this.hero.Score}" +
                    $"\n" +
                    $"\n" +
                    $"\n" +
                    $"\nThe Boss: " +
                    $"\nHelath: {this.hero.Enemies[0].CurrentHealth}/{this.hero.Enemies[0].MaxHealth}" +
                    $"\nStrikePower: {this.hero.Enemies[0].StrikePower}";

                foxDraw.AddText(this.stats);
            }
            int move = 1;
            if (hero.keyPress[Key.Down] == true)
            {
                this.hero.MoveHero(0, move, "down", @"../../../../Assets/heroDownReduced.png");
            }
            if (hero.keyPress[Key.Up] == true)
            {
                this.hero.MoveHero(0, -move, "up", @"../../../../Assets/heroUpReduced.png");
            }
            if (hero.keyPress[Key.Left] == true)
            {
                this.hero.MoveHero(-move, 0, "left", @"../../../../Assets/heroLeftReduced.png");
            }
            if (hero.keyPress[Key.Right] == true)
            {
                this.hero.MoveHero(move, 0, "right", @"../../../../Assets/heroRightReduced.png");
            }
            if (hero.keyPress[Key.Space] == true && !this.heroJumpBan.IsEnabled)
            {
                if (hero.keyPress[Key.Down] == true && !hero.IsWallInFront("down"))
                {
                    BanJump();
                    this.hero.MoveHero(0, 30, "down", @"../../../../Assets/heroDownReduced.png");
                    List<bool> areEnemiesInRange = this.hero.IsAnyEnemyInRange();
                    for (int i = 0; i < areEnemiesInRange.Count; i++)
                    {
                        if (areEnemiesInRange[i] == true)
                        {
                            this.hero.Enemies[i].CurrentHealth -= this.hero.StrikePower;
                        }
                    }
                }
                if (hero.keyPress[Key.Up] == true && !hero.IsWallInFront("up"))
                {
                    this.hero.MoveHero(0, -30, "up", @"../../../../Assets/heroUpReduced.png");
                    BanJump();
                    List<bool> areEnemiesInRange = this.hero.IsAnyEnemyInRange();
                    for (int i = 0; i < areEnemiesInRange.Count; i++)
                    {
                        if (areEnemiesInRange[i] == true)
                        {
                            this.hero.Enemies[i].CurrentHealth -= this.hero.StrikePower;
                        }
                    }
                }
                if (hero.keyPress[Key.Left] == true && !hero.IsWallInFront("left"))
                {
                    this.hero.MoveHero(-30, 0, "left", @"../../../../Assets/heroLeftReduced.png");
                    List<bool> areEnemiesInRange = this.hero.IsAnyEnemyInRange();
                    BanJump();
                    for (int i = 0; i < areEnemiesInRange.Count; i++)
                    {
                        if (areEnemiesInRange[i] == true)
                        {
                            this.hero.Enemies[i].CurrentHealth -= this.hero.StrikePower;
                        }
                    }
                }
                if (hero.keyPress[Key.Right] == true && !hero.IsWallInFront("right"))
                {
                    this.hero.MoveHero(30, 0, "right", @"../../../../Assets/heroRightReduced.png");
                    BanJump();
                    List<bool> areEnemiesInRange = this.hero.IsAnyEnemyInRange();
                    for (int i = 0; i < areEnemiesInRange.Count; i++)
                    {
                        if (areEnemiesInRange[i] == true)
                        {
                            this.hero.Enemies[i].CurrentHealth -= this.hero.StrikePower;
                        }
                    }
                }
                hero.keyPress[Key.Space] = false;
            }
        }
        public void KeyPressed(object sender, Avalonia.Input.KeyEventArgs e)
        {
            hero.keyPress[e.Key] = true;
        }
        public void KeyOff(object sender, Avalonia.Input.KeyEventArgs e)
        {
            hero.keyPress[e.Key] = false;
        }
    }
}