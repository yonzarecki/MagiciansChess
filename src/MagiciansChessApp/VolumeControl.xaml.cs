using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace HockeyApp
{
    public sealed partial class VolumeControl : UserControl
    {
        private bool finishedInit = false;

        public VolumeControl()
        {
            this.InitializeComponent();
            SoundMuted = false;
            //BackgroundVolume = 5;
        }


        private static MediaElement BackgroundSoundPlayer
        {
            get
            {
                var rootGrid = VisualTreeHelper.GetChild(Window.Current.Content, 0);
                return VisualTreeHelper.GetChild(rootGrid, 0) as MediaElement;
            }
        }

        private double BackgroundVolume
        {
      
            get { return BackgroundSoundPlayer.Volume; }
            set { BackgroundSoundPlayer.Volume = value;  }
        }

        private double volumeKeeper = 0;

        public void Mute(Button btn)
        {
            btn.Style = (Application.Current as App).MuteStyle;
            volumeKeeper = BackgroundVolume;
            BackgroundVolume = 0;

        }

        public void UnMute(Button btn)
        {
            btn.Style = (Application.Current as App).UnMuteStyle;
            BackgroundVolume = volumeKeeper;
        }

        private bool SoundMuted { get; set; }

        private void Btn_mute_OnClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
        
            if (SoundMuted)
            {
                UnMute(btn); 
            }

            else
            {
                Mute(btn);
            }

            SoundMuted = !SoundMuted;
        }

        private void VolumeSlider_OnValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (finishedInit)
            {
                BackgroundVolume = e.NewValue;
            }

            if (!finishedInit)
                finishedInit = true;
        }

        
    }
}
