﻿using Microsoft.Win32;
using MusicPlayer.CommandPattern;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TagLib;

namespace MusicPlayer
{
    

    public sealed class Player
    {
        public static MediaPlayer mplayer;
        public List<Song> queue { get; set; }
        public int currentSongPointer { get; set; }
        int slot;
        CommandInvoker cmdControl;

        private Player()
        {
            mplayer = new MediaPlayer();
            queue = new List<Song>();
            currentSongPointer = -1;
            cmdControl = new CommandInvoker();
            slot = 0;               //slot to be used when setting commands
        }

        private static Player instance = null;

        public static Player Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Player();
                }
                return instance;
            }
        }

        public void skipBack()
        {
            if (currentSongPointer <= 0)
            {
                //do nothing
            }
            else
            {
                currentSongPointer--;
                cmdControl.playbtnPushed(currentSongPointer);
            }
        }

        public void pause()
        {
            if(currentSongPointer > -1)
            {
                cmdControl.pausebtnPushed(currentSongPointer);
            }
            
        }

        public void play()
        {
            if(currentSongPointer > -1)
            {
                cmdControl.playbtnPushed(currentSongPointer);
            }
            
        }

        public void skipForward()
        {
            if (currentSongPointer >= queue.Count || currentSongPointer <= -1)
            {
                //do nothing
            }
            else
            {
                currentSongPointer++;
                cmdControl.playbtnPushed(currentSongPointer);
            }
        }



        public void import()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Multiselect = true;
            openFileDialog1.ShowDialog();

            String[] s = openFileDialog1.FileNames;

            for (int i = 0; i < s.Length; i++)
            {
                Uri uri = new Uri(s[i], UriKind.Absolute);
                mplayer.Open(uri);
                File file = File.Create(uri.OriginalString);
                queue.Add(new Song(uri, file.Tag.Title, file.Tag.Album, file.Tag.JoinedPerformers, (int)file.Tag.Year));
                currentSongPointer++;
                cmdControl.setCommand(slot, new PlayCommand((Song)queue[currentSongPointer]), new PauseCommand((Song)queue[currentSongPointer]));
                slot++;
            }
        }
    }
}
