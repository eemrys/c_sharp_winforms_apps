using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Snake.BusinessLogic;

namespace Snake.Models
{
    [Serializable]
    public class Board
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int TurnNumber { get; set; }
        public List<Coordinates> FoodCoordinates { get; set; }
        public List<Coordinates> SnakeCoordinates { get; set; }
        public Direction CurrentDirection { get; set; }
        public System.Timers.Timer Timer { get; set; }
        public enum Direction
        {
            Top,
            Bottom,
            Left,
            Right
        }

        public static Dictionary<Direction, Coordinates> DirectionMove = new Dictionary<Direction, Coordinates>
            {
                {Direction.Top, new Coordinates(0, -1) },
                {Direction.Bottom, new Coordinates(0, 1) },
                {Direction.Left, new Coordinates(-1, 0) },
                {Direction.Right, new Coordinates(1, 0) }
             };

        public Board()
        {
            Width = 20;
            Height = 20;
            Timer = new System.Timers.Timer(4000);
            Timer.Elapsed += Timer_Elapsed;
            Init();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Move();
        }

        public void Init()
        {
            CurrentDirection = Direction.Top;
            TurnNumber = 0;
            SnakeCoordinates = new List<Coordinates>
                {
                    new Coordinates(Width / 2, Height / 2),
                    new Coordinates(Width / 2, Height / 2 + 1)
                };
            FoodCoordinates = new List<Coordinates>();
            this.GenerateFood();
            this.GenerateFood();
            Timer.Enabled = true;
        }
    }
}
