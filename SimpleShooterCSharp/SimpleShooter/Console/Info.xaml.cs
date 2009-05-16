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
    public partial class Info : UserControl
    {
        private string _gameInfo;
        public string GameInfo
        {
            get { return _gameInfo; }
            set
            {
                _gameInfo = value;
                txtInfo.Text = GameInfo;
            }
        }


        public Info()
        {
            InitializeComponent();
        }
    }
}
