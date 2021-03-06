﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Milky.OsuPlayer.Common;
using Milky.OsuPlayer.Common.Configuration;
using Milky.OsuPlayer.Common.Data;
using Milky.OsuPlayer.Common.Player;
using Milky.OsuPlayer.Media.Audio;
using Milky.OsuPlayer.ViewModels;
using Milky.WpfApi;

namespace Milky.OsuPlayer.Control
{
    public class VolumeControlVm : ViewModelBase
    {
        private PlayerViewModel _player = PlayerViewModel.Current;
        public PlayerViewModel Player
        {
            get => _player;
            set
            {
                _player = value;
                OnPropertyChanged();
            }
        }
    }
    /// <summary>
    /// VolumeControl.xaml 的交互逻辑
    /// </summary>
    public partial class VolumeControl : UserControl
    {
        public int HitsoundOffset
        {
            get => (int)GetValue(HitsoundOffsetProperty);
            set => SetValue(HitsoundOffsetProperty, value);
        }

        public static readonly DependencyProperty HitsoundOffsetProperty =
            DependencyProperty.Register(
                "HitsoundOffset",
                typeof(int),
                typeof(VolumeControl),
                new PropertyMetadata(0, OffsetChanged)
            );

        private static void OffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is VolumeControl ctrl && ComponentPlayer.Current != null)
            {
                ctrl.Offset.Value = ComponentPlayer.Current.HitsoundOffset;
            }
        }

        private readonly AppDbOperator _appDbOperator = new AppDbOperator();

        public VolumeControl()
        {
            InitializeComponent();
        }

        private void MasterVolume_DragComplete(object sender, DragCompletedEventArgs e)
        {
            AppSettings.SaveDefault();
        }

        private void MusicVolume_DragComplete(object sender, DragCompletedEventArgs e)
        {
            AppSettings.SaveDefault();
        }

        private void HitsoundVolume_DragComplete(object sender, DragCompletedEventArgs e)
        {
            AppSettings.SaveDefault();
        }

        private void SampleVolume_DragComplete(object sender, DragCompletedEventArgs e)
        {
            AppSettings.SaveDefault();
        }

        private void Balance_DragComplete(object sender, DragCompletedEventArgs e)
        {
            AppSettings.SaveDefault();
        }

        private void Offset_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (ComponentPlayer.Current == null)
                return;
            ComponentPlayer.Current.HitsoundOffset = (int)Offset.Value;
        }

        private void Offset_DragComplete(object sender, DragCompletedEventArgs e)
        {
            _appDbOperator.UpdateMap(Services.Get<PlayerList>().CurrentInfo.Identity,
                ComponentPlayer.Current.HitsoundOffset);
        }
    }
}
