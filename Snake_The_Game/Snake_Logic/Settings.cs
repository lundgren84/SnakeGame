using System;
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
        public static int wide { get; set; }
        public int height { get; set; }
        public static int points { get; set; }
        public static int score { get; set; }
       public  Settings()
        {
            direction = Direction.Stop;
            points = 100;
            score = 0;
        }
    }
}
