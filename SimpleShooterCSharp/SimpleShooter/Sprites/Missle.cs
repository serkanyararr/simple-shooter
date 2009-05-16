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
    public class Missle: Sprite
    {
        public double MaxX { get; set; }
        public double MinX { get; set; }

        public Missle(double width, double height, Point firstPosition)
            : base(width, height, firstPosition)
        {
            ((Storyboard)SpriteCanvas.FindName("Storyboard1")).Begin();
        }

        public override Canvas RenderSpriteCanvas()
        {
            return LoadSpriteCanvas("SimpleShooter.Sprites.Missle.xaml");
        }

        public override void Update(System.TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
        }
    }
}