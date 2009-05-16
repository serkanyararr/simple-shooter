using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace SimpleShooter
{
    public abstract class Sprite
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public Vector Velocity { get; set; }
        public Canvas SpriteCanvas { get; set; }
        public double CollisionRadius;
        private Point _position;
        public Point Position
        {
            get
            {
                return _position;
            } 
            set
            {
                _position = value;
                SpriteCanvas.SetValue(Canvas.TopProperty, _position.Y - (Height/2));
                SpriteCanvas.SetValue(Canvas.LeftProperty, _position.X - (Width/2));
            }
        }

        public Sprite(Double initialWidth, Double initialHeight, Point initialPosition)
        {
            Width = initialWidth;
            Height = initialHeight;
            
            SpriteCanvas = RenderSpriteCanvas();

            SpriteCanvas.Width = Width;
            SpriteCanvas.Height = Height;
            //NOTE: because the setter for Position uses both Height and Width, it is important this comes after they are set.
            Position = initialPosition;
            CollisionRadius = Width*.48;
        }

        public abstract Canvas RenderSpriteCanvas();

        public Canvas LoadSpriteCanvas(string xamlPath)
        {
            System.IO.Stream s = this.GetType().Assembly.GetManifestResourceStream(xamlPath);
            return (Canvas)XamlReader.Load(new System.IO.StreamReader(s).ReadToEnd());
        }

        public virtual void Update(TimeSpan elapsedTime)
        {
            Position = (Position + Velocity * elapsedTime.TotalSeconds);
        }

        public static bool Collides(Sprite s1, Sprite s2)
        {
            Vector v = new Vector((s1.Position.X) - (s2.Position.X), (s1.Position.Y) - (s2.Position.Y));
            if (s1.CollisionRadius + s2.CollisionRadius > v.Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
