using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Snake.Models;

namespace Snake.BusinessLogic
{
    public static class BoardLogic
    {
        public static void UpdateDirection(this Board board, Board.Direction newDir) 
        {
            bool isOppositeDirection = false;
            if ((Board.DirectionMove[newDir].X == 0) && (Board.DirectionMove[board.CurrentDirection].X == 0))
            {
                isOppositeDirection = true;
            }
            if ((Board.DirectionMove[newDir].Y == 0) && (Board.DirectionMove[board.CurrentDirection].Y == 0))
            {
                isOppositeDirection = true;
            }
            if (!(isOppositeDirection))
            {
                board.CurrentDirection = newDir;
            }
        }
        public static void Move(this Board board)
        {
            bool gameOver1, gameOver2 = false;
            board.SnakeCoordinates.RemoveAt(board.SnakeCoordinates.Count - 1);

            int x = Board.DirectionMove[board.CurrentDirection].X;
            int y = Board.DirectionMove[board.CurrentDirection].Y;

            Coordinates newHead = new Coordinates(board.SnakeCoordinates[0].X + x, board.SnakeCoordinates[0].Y + y);

            gameOver1 = (newHead.X < 1) || (newHead.X > board.Width - 2) || (newHead.Y < 1) || (newHead.Y > board.Height - 2);
            foreach (Coordinates pair in board.SnakeCoordinates)
            {
                gameOver2 = (pair.X == newHead.X) && (pair.Y == newHead.Y);
            }

            if (!gameOver1 && !gameOver2)
            {
                board.SnakeCoordinates.Insert(0, newHead);
                board.TurnNumber += 1;
                if (board.HasFood())
                {
                    board.Eat();
                }          
            }
            else
            {
                board.Timer.Enabled = false;
                board.Init();
            }
        }
        
        public static bool HasFood(this Board board)
        {
            bool hasFood = false;
            foreach (Coordinates pair in board.FoodCoordinates)
            {
                if (board.SnakeCoordinates[0] == pair)
                {
                    hasFood = true;
                }
            }
            return hasFood;
        }

        public static void Eat(this Board board)
        {
            Coordinates lastCell = board.SnakeCoordinates[board.SnakeCoordinates.Count - 1];
            Coordinates secondToLast = board.SnakeCoordinates[board.SnakeCoordinates.Count - 2];
            int xDiff = secondToLast.X - lastCell.X;
            int yDiff = secondToLast.Y - lastCell.Y;

            board.SnakeCoordinates.Add(new Coordinates(lastCell.X - xDiff, lastCell.Y - yDiff));
            board.FoodCoordinates.Remove(board.SnakeCoordinates[0]);
            board.GenerateFood();
        }

        public static void GenerateFood(this Board board)
        {
            Random r = new Random();
            bool invalid = true;
            while (invalid)
            {
                invalid = false;
                int x = r.Next(0, board.Width - 1);
                int y = r.Next(0, board.Height - 1);
                Coordinates newFood = new Coordinates(x, y);
                if ((board.SnakeCoordinates.Contains(newFood)) || (board.FoodCoordinates.Contains(newFood)))
                {
                    invalid = true;
                }
                else
                {
                    board.FoodCoordinates.Add(newFood);
                }
            }
        }        
    }
}