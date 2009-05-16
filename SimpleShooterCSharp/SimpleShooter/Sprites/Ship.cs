using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace SimpleShooter
{
    public class Ship: Sprite
    {
        public double MaxX { get; set; }
        public double MinX { get; set; }
        public Storyboard FadeStoryboard { get; set; }

        public Ship(double width, double height, Point firstPosition)
            : base(width, height, firstPosition)
        {
            FadeStoryboard = new Storyboard();
            Duration durationTimeline = new Duration(TimeSpan.FromMilliseconds(2000));
            DoubleAnimation spawnFade = new DoubleAnimation();

            spawnFade.From = .1;
            spawnFade.To = 1;
            spawnFade.SetValue(Storyboard.TargetNameProperty, SpriteCanvas.Name);
            spawnFade.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Opacity"));
            spawnFade.Duration = durationTimeline;
            FadeStoryboard.Children.Add(spawnFade);
            SpriteCanvas.Resources.Add("SpawnFadeAnimation", FadeStoryboard);
            FadeStoryboard.Begin();
        }

        public override Canvas RenderSpriteCanvas()
        {
            return LoadSpriteCanvas("SimpleShooter.Sprites.Ship.xaml");
        }

        public override void Update(System.TimeSpan elapsedTime)
        {
            //verify that this is a position we can move to
            if (Position.X > MaxX)
            {
                Position = new Point(MaxX, Position.Y);
                Velocity = new Vector(0, 0);
            }
            if (Position.X < MinX)
            {
                Position = new Point(MinX, Position.Y);
                Velocity = new Vector(0, 0);
            }
            base.Update(elapsedTime);
        }

        public Missle Fire()
        {
            Missle missle = new Missle(5, 5, Position);
            missle.Velocity = Vector.CreateVectorFromAngle(0, 250);
            return missle;
        }
    }
}
