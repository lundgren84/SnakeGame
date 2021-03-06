﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_Logic
{
    public enum Direction
    {
        Up,Down,Left,Right,Stop
    }
   public class Settings
    {
       public static Direction direction { get; set; }
        public static int Width { get; set; }
        public static int Height { get; set; }
        public static int points { get; set; }
        public static int score { get; set; }
        public static bool GameOver { get; set; }
       public  Settings()
        {
            direction = Direction.Left;
            points = 100;
            score = 0;
            Width = 15;
            Height = 15;
            GameOver = false;
        }
    }
}
