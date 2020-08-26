using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Snake.Models;
using Snake.BusinessLogic;

namespace Snake.DataAccessLayer
{
    public interface ISnakeRepository
    {
        Board Get();
        void UpdateDir(Board.Direction newDir);
    }

    public class SnakeRepository : ISnakeRepository
    {
        private Board _board = new Board();

        public Board Get()
        {
            return _board;
        }

        public void UpdateDir(Board.Direction newDir)
        {
            _board.UpdateDirection(newDir);
        }
    }
}
