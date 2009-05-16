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
    public partial class Score : UserControl
    {
        private int _gameScore;
        public int GameScore
        { 
            get { return _gameScore;}
            set
            {
                _gameScore = value;
                txtScore.Text = "Skor : "+GameScore.ToString();
            }
        }

        public Score()
        {
            InitializeComponent();
        }
    }
}
