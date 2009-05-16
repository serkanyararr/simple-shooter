using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SimpleShooter
{
    public class Alien : Sprite
    {
        public double fireRateMilliseconds = 2000;
        public double fireVelocity = 250;
        public double wayPointMin;
        public double wayPointMax;
        public double speed = 100;
        public bool spawnWait;
        public DateTime spawnComplete;
        public double MaxX { get; set; }
        public double MinX { get; set; }

        public Alien(double width, double height, Point firstPosition)
            : base(width, height, firstPosition)
        {

        }

        public void CheckDirection()
        {
            if (Position.X > wayPointMax)
            {
                Velocity = Vector.CreateVectorFromAngle(270, speed);
            }
            if (Position.X < wayPointMin)
            {
                Velocity = Vector.CreateVectorFromAngle(90, speed);
            }
        }

        public override Canvas RenderSpriteCanvas()
        {
            return LoadSpriteCanvas("SimpleShooter.Sprites.Alien.xaml");
        }

        public override void Update(TimeSpan elapsedTime)
        {
            CheckDirection();
            base.Update(elapsedTime);
        }

        public Bomb Fire()
        {
            Bomb bomb = new Bomb(5,5,Position);
            bomb.Velocity = Vector.CreateVectorFromAngle(180, fireVelocity);
            return bomb;
        }
    }
}