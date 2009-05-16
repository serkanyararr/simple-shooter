using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SimpleShooter.Sprites;

namespace SimpleShooter
{
    public partial class Page : UserControl
    {
        private KeyHandler keyHandler;
        private GameLoop gameLoop;
        private Ship PlayerShip { get; set; }
        List<Alien> aliens;
        List<Alien> aliensRemove;
        List<Alien> alienShooters;
        List<Bomb> bombs;
        List<Bomb> bombsRemove;
        List<Missle> missles;
        List<Missle> misslesRemove;
        List<WaveData> waves;

        int spritesCount;
        int alienCount = 5;
        int shotsAtOnce = 1;
        int shotsThisPass = 1;
        double corridorHeight;
        double corridorPad = 15;
        double alienSpeed = 100;
        bool alternate = true;
        double enemyShootMilliseonds = 3000;
        double spawnFadeInMilliseconds = 3000;

        public GameState status;

        bool shoot = false;

        DateTime nextShot = DateTime.Now;

        public Page()
        {
            InitializeComponent();
            keyHandler = new KeyHandler(this);
            GenerateStarField(350);
            InitializeGame();
        }

        void InitializeGame()
        {
            gameLoop = new GameLoop(this);
            gameLoop.Update += new GameLoop.UpdateHandler(gameLoop_Update);

            PlayerShip = new Ship(30, 30, new Point(100, 360));
            PlayerShip.MaxX = 470;
            PlayerShip.MinX = 10;
            gameRoot.Children.Add(PlayerShip.SpriteCanvas);

            corridorHeight = ((gameRoot.Height - 100) / alienCount) - 15;

            waves = new List<WaveData>();
            waves.Add(new WaveData(10, 3000, 2, 1));
            waves.Add(new WaveData(11, 2000, 4, 5));
            waves.Add(new WaveData(12, 1000, 6, 1));
            waves.Add(new WaveData(15, 1000, 10, 1));
            waves.Add(new WaveData(500, 1000, 11, 5));
            waves.Add(new WaveData(20, 500, 12, 1));
            waves.Add(new WaveData(20, 500, 13, 2));
            waves.Add(new WaveData(30, 500, 20, 3));
            waves.Add(new WaveData(50, 300, 25, 3));
            waves.Add(new WaveData(100, 300, 30, 5));
            waves.Add(new WaveData(200, 300, 40, 5));
            waves.Add(new WaveData(500, 300, 50, 5));

            aliens = new List<Alien>();
            aliensRemove = new List<Alien>();
            alienShooters = new List<Alien>();
            bombs = new List<Bomb>();
            bombsRemove = new List<Bomb>();
            missles = new List<Missle>();
            misslesRemove = new List<Missle>();

            StartWave();

            ctlLives.Lives = 3;

            gameLoop.Start();
            status = GameState.Running;
        }

        void gameLoop_Update(TimeSpan elapsed)
        {
            //clear the current Vector so the sprite is not moving unless a keys is pressed
            PlayerShip.Velocity = new Vector(0, 0);
            if (keyHandler.IsKeyPressed(Key.Left))
            {
                PlayerShip.Velocity = Vector.CreateVectorFromAngle(270, 125);
            }
            if (keyHandler.IsKeyPressed(Key.Right))
            {
                PlayerShip.Velocity = Vector.CreateVectorFromAngle(90, 125);
            }
            if (keyHandler.IsKeyPressed(Key.Space))
            {
                switch (status)
                {
                    case GameState.Ready:
                        break;
                    case GameState.Running:
                        EntityFired(PlayerShip);
                        break;
                    case GameState.Paused:
                        break;
                    case GameState.BetweenWaves:
                        status = GameState.Running;
                        ctlInfo.GameInfo = "";
                        StartWave();
                        break;
                    case GameState.GameOver:
                        break;
                    default:
                        break;
                }
            }
            PlayerShip.Update(elapsed);

            BombLoop(elapsed);
            MissleLoop(elapsed);
            AlienLoop(elapsed);

            foreach (Alien alien in aliensRemove)
            {
                aliens.Remove(alien);
                gameRoot.Children.Remove(alien.SpriteCanvas);
                AlienShot(alien);
            }
            aliensRemove.Clear();

            foreach (Missle missle in misslesRemove)
            {
                missles.Remove(missle);
                gameRoot.Children.Remove(missle.SpriteCanvas);
            }
            misslesRemove.Clear();

            if (nextShot <= DateTime.Now)
            {
                nextShot = DateTime.Now.AddMilliseconds(enemyShootMilliseonds).AddMilliseconds(elapsed.Milliseconds * -1);

                shotsThisPass = shotsAtOnce;
                if (shotsThisPass > aliens.Count)
                {
                    shotsThisPass = aliens.Count;
                }

                if (aliens.Count > 0)
                {
                    foreach (Alien alien in aliens)
                    {
                        alienShooters.Add(alien);
                    }
                }

                while (alienShooters.Count > shotsThisPass)
                {
                    alienShooters.RemoveAt(GetRandInt(0, alienShooters.Count - 1));
                }

                foreach (Alien alien in alienShooters)
                {
                    EntityFired(alien);
                }

                alienShooters.Clear();
            }
        }

        void EntityFired(Sprite shooter)
        {
            Debug.WriteLine(shooter);
            switch (shooter.ToString())
            {
                case "SimpleShooter.Ship":
                    if (missles.Count == 0)
                    {
                        Missle missle = ((Ship)shooter).Fire();
                        missles.Add(missle);
                        gameRoot.Children.Add(missle.SpriteCanvas);

                    }
                    break;
                case "SimpleShooter.Alien":
                    Bomb bomb = ((Alien)shooter).Fire();
                    bombs.Add(bomb);
                    gameRoot.Children.Add(bomb.SpriteCanvas);
                    break;
                default:
                    break;
            }
        }

        public void AlienShot(Alien alien)
        {
            if (waves.Count > 0)
            {
                if (waves[0].EnemyCount > 0)
                {
                    NextFromWave(alien.Position.Y - (corridorPad + alien.SpriteCanvas.Height));
                }
                else
                {
                    if (aliens.Count == 0)
                    {
                        //wave over
                        waves.RemoveAt(0);
                        if (waves.Count > 0)
                        {
                            ctlInfo.GameInfo = string.Format("{0}{1}{2}", "İşgal Engellendi", Environment.NewLine, "(Devam etmek için ateş edin)");
                            status = GameState.BetweenWaves;
                        }
                        else
                        {
                            GameOver();
                        }
                    }
                }
            }
            else
            {

                if (aliens.Count < 1)
                {
                    GameOver();
                }
            }

        }

        void BombLoop(TimeSpan elapsed)
        {
            foreach (Bomb bomb in bombs)
            {
                bomb.Update(elapsed);
                if (bomb.Position.Y > gameRoot.Height)
                {
                    gameRoot.Children.Remove(bomb.SpriteCanvas);
                    bombsRemove.Add(bomb);
                }
                else
                {
                    if (Sprite.Collides(PlayerShip, bomb))
                    {
                        ctlLives.Lives -= 1;

                        if (ctlLives.Lives < 1)
                        {
                            GameOver();
                        }
                        else
                        {
                            bombsRemove.Add(bomb);
                            PlayerShip.FadeStoryboard.Begin();
                        }

                    }
                }
            }
            foreach (Bomb bomb in bombsRemove)
            {
                bombs.Remove(bomb);
                gameRoot.Children.Remove(bomb.SpriteCanvas);
            }
            bombsRemove.Clear();
        }

        void MissleLoop(TimeSpan elapsed)
        {
            foreach (Missle missle in missles)
            {
                missle.Update(elapsed);
                if (missle.Position.Y < 0)
                {
                    gameRoot.Children.Remove(missle.SpriteCanvas);
                    misslesRemove.Add(missle);
                }

            }

            foreach (Missle missle in misslesRemove)
            {
                missles.Remove(missle);
                gameRoot.Children.Remove(missle.SpriteCanvas);
            }
            misslesRemove.Clear();

        }

        void AlienLoop(TimeSpan elapsed)
        {
            foreach (Alien alien in aliens)
            {
                alien.Update(elapsed);
                alien.CheckDirection();

                if (alien.spawnWait)
                {
                    if (alien.spawnComplete <= DateTime.Now)
                    {
                        alien.spawnWait = false;
                    }
                }

                if (!alien.spawnWait)
                {
                    foreach (Missle missle in missles)
                    {
                        if (Sprite.Collides(alien, missle))
                        {
                            aliensRemove.Add(alien);
                            misslesRemove.Add(missle);
                            ctlScore.GameScore += 10;
                        }
                    }
                }
            }
        }

        void GenerateStarField(int numberOfStars)
        {

            for (int i = 0; i < numberOfStars; i++)
            {

                Ellipse star = new Ellipse();
                double size = GetRandInt(10, 800) * .01;
                star.Width = size;
                star.Height = size;
                star.Opacity = GetRandInt(1, 5) * .1;
                star.Fill = new SolidColorBrush(Colors.White);
                int x = GetRandInt(0, (int)Math.Round(gameRoot.Height, 0));
                int y = GetRandInt(0, (int)Math.Round(gameRoot.Width, 0));
                star.SetValue(Canvas.TopProperty, (double)x);
                star.SetValue(Canvas.LeftProperty, (double)y);
                gameRoot.Children.Add(star);
            }
        }

        public int GetRandInt(int min, int max)
        {
            Byte[] rndBytes = new Byte[10];
            RNGCryptoServiceProvider rndC = new RNGCryptoServiceProvider();
            rndC.GetBytes(rndBytes);
            int seed = BitConverter.ToInt32(rndBytes, 0);
            Random rand = new Random(seed);
            return rand.Next(min, max);
        }

        public void StartWave()
        {
            if (waves.Count > 0)
            {

                Alien alien = new Alien(30, 30, new Point(0,0));
                for (int i = 0; i < waves[0].enemiesAtOnce; i++)
                {
                    int row = i;
                    while (row > 5)
                    {
                        row -= 5;
                    }
                    AddAlien(corridorHeight * row, 0);
                    waves[0].EnemyCount -= 1;
                    shotsAtOnce = waves[0].fireAtOnce;
                    enemyShootMilliseonds = waves[0].fireRateMilliseconds;
                }
            }

        }
        
        public void NextFromWave(double startY)
        {
            if (waves.Count > 0)
            {
                Alien alien = new Alien(30, 30, new Point(0, 0));
                AddAlien(startY, spawnFadeInMilliseconds);
                waves[0].EnemyCount -= 1;
                if (waves[0].EnemyCount == 0)
                {
                    //then that was the last one for that wave
                    waves[0].waveEmpty = true;
                }
            }
        }

        public void AddAlien(double startY, double fadeInMilliseconds)
        {
            Alien alien = new Alien(30,30, new Point(0,0));
            spritesCount += 1;
            alien.MinX = 1;
            alien.MaxX = gameRoot.Width - 1;
            alien.speed = alienSpeed;
            alien.wayPointMax = gameRoot.Width - (alien.SpriteCanvas.Width / 2) - 10;
            alien.wayPointMin = (alien.SpriteCanvas.Width / 2) + 10;

            alien.Position = new Point(GetRandInt((int)Math.Round(alien.wayPointMin, 0), (int)Math.Round(alien.wayPointMax, 0)), startY + corridorPad + alien.SpriteCanvas.Height);

            if (alternate)
            {
                alien.Velocity = Vector.CreateVectorFromAngle(90, alienSpeed);
                alternate = false;
            }
            else
            {
                alien.Velocity = Vector.CreateVectorFromAngle(270, alienSpeed);
                alternate = true;
            }

            if (fadeInMilliseconds > 0)
            {
                alien.spawnWait = true;
                alien.spawnComplete = DateTime.Now.AddMilliseconds(fadeInMilliseconds);
                TimeSpan tsDuration = TimeSpan.FromMilliseconds(fadeInMilliseconds);
                Duration durationTimeline = new Duration(tsDuration);
                DoubleAnimation spawnFade = new DoubleAnimation();
                Storyboard storyBoard = new Storyboard();

                spawnFade.From = .1;
                spawnFade.To = 1;
                spawnFade.SetValue(Storyboard.TargetNameProperty, ((Canvas)alien.SpriteCanvas).Name);
                spawnFade.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Opacity"));
                spawnFade.Duration = durationTimeline;
                storyBoard.Children.Add(spawnFade);
                ((Canvas)alien.SpriteCanvas).Resources.Add("SpawnFadeAnimation", storyBoard);
                storyBoard.Begin();
            }

            aliens.Add(alien);
            gameRoot.Children.Add(alien.SpriteCanvas);

        }

        public void GameOver()
        {
            ctlInfo.GameInfo = string.Format("OYUN BİTTİ");
            status = GameState.GameOver;
            gameLoop.Stop();
        }
     
        public enum GameState
        {
            Ready = 0,
            Running = 1,
            Paused = 2,
            BetweenWaves = 3,
            GameOver = 4
        }
    }
}
