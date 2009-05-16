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
    public partial class WaveInfo : UserControl
    {
        private string _waveInfo;
        public string WaveInfoText
        {
            get { return _waveInfo; }
            set
            {
                _waveInfo = value;
                txtWaveInfo.Text = WaveInfoText;
            }
        }

        public WaveInfo()
        {
            InitializeComponent();
        }
    }
}
