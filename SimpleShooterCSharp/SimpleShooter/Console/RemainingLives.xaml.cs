using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SimpleShooter
{
    public partial class RemainingLives : UserControl
    {
        private int _lives  ;
        public int Lives
        {
            get { return _lives; }
            set
            {
                _lives = value;
                string livesString = string.Empty;
                for (int i = 0; i < _lives -1; i++)
                {
                    livesString = string.Format("{0}{1}", livesString, "");
                }
                txtRemainingLives.Text = livesString;
            }
        }

        public RemainingLives()
        {
            InitializeComponent();
        }
    }
}
