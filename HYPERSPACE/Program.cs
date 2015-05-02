using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERSPACE
{
    class Program
    {
        static void Main(string[] args)
        {
            Hyperspace game = new Hyperspace();
            game.PlayGame();
        }
    }
    class Unit
    {
        public int Y
        {
            get;
            set;
        }

        public int X
        {
            get;
            set;
        }

        public ConsoleColor Color
        {
            get;
            set;
        }

        public string Symbol
        {
            get;
            set;
        }

        public bool IsSpaceRift
        {
            get;
            set;
        }

        static List<string> ObstacleList = new List<string>() { "*", "!", ".", ":", ";", ":", "'", "?" };

        static Random randomNumberGenerator = new Random();

        public Unit(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Color = ConsoleColor.Cyan;
            this.Symbol = ObstacleList[randomNumberGenerator.Next(0, ObstacleList.Count)];
        }

        public Unit(int x, int y, ConsoleColor color, string symbol, bool isSpaceRift)
        {
            this.X = x;
            this.Y = y;
            this.Color = color;
            this.Symbol = symbol;
            this.IsSpaceRift = isSpaceRift;
        }

        public void Draw()
        {
            Console.SetCursorPosition(this.X, this.Y);
            Console.ForegroundColor = this.Color;
            Console.WriteLine(this.Symbol);
        }
    }
    class Hyperspace
    {
        public int Score
        {
            get;
            set;
        }

        public int Speed
        {
            get;
            set;
        }

        public List<Unit> ObstacleList
        {
            get;
            set;
        }
        public Unit SpaceShip
        {
            get;
            set;
        }

        public bool Smashed
        {
            get;
            set;
        }

        private Random randomNumberGenerator = new Random();

        public Hyperspace()
        {
            this.Score = 0;
            this.Speed = 0;
            this.ObstacleList = new List<Unit>();
            this.SpaceShip = new Unit((Console.WindowWidth / 2) - 1, (Console.WindowHeight - 1), ConsoleColor.Red, "@", false);
            //Console.BufferHeight = 30;
            Console.WindowHeight = 30;
            //Console.BufferWidth = 60;
            Console.WindowWidth = 60;
        }
        public void PlayGame()
        {
            while (!Smashed)
            {
                int spaceRiftChance = randomNumberGenerator.Next(1, 11);

                if (spaceRiftChance == 10)
                {
                    ObstacleList.Add(new Unit(randomNumberGenerator.Next(0, Console.WindowWidth - 2), 5, ConsoleColor.Green, "%", true));
                }
                else
                {
                    ObstacleList.Add(new Unit(randomNumberGenerator.Next(0, Console.WindowWidth - 2), 5));
                }

                MoveShip();
                MoveObstacles();
                DrawGame();

                if (Speed < 170)
                {
                    Speed++;
                }
                System.Threading.Thread.Sleep(170 - Speed);
            }
        }
        public void MoveShip()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyPressed = Console.ReadKey();

                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }
                if (keyPressed.Key == ConsoleKey.LeftArrow && SpaceShip.X > 0)
                {
                    SpaceShip.X--;
                }
                if (keyPressed.Key == ConsoleKey.RightArrow && SpaceShip.X < (Console.WindowWidth - 2))
                {
                    SpaceShip.X++;
                }
            }
        }

        public void MoveObstacles()
        {
            List<Unit> newObstacleList = new List<Unit>();

            foreach (Unit unit in ObstacleList)
            {
                unit.Y++;
                if (unit.X == SpaceShip.X && unit.Y == SpaceShip.Y && unit.IsSpaceRift == true)
                {
                    Speed -= 50;
                    Smashed = false;
                }

                if (unit.X == SpaceShip.X && unit.Y == SpaceShip.Y && unit.IsSpaceRift == false)
                {
                    Smashed = true;
                }

                if (unit.Y < Console.WindowHeight)
                {
                    newObstacleList.Add(unit);
                }
                else
                {
                    Score++;
                }
            }
            ObstacleList = newObstacleList;
            }
        public void DrawGame()
        {
            Console.Clear();
            SpaceShip.Draw();

            foreach (Unit unit in ObstacleList)
            {
                unit.Draw();
            }
            PrintAtPosition(20, 2, "Score: " + this.Score, ConsoleColor.Green);
            PrintAtPosition(20, 3, "Speed: " + this.Speed, ConsoleColor.Green);
        }
        public void PrintAtPosition(int x, int y, string text, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(text);
        }
    }
}
