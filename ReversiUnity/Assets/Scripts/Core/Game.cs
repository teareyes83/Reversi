using System;
using System.Linq;
using System.Text;

namespace Core
{
    public class Game
    {
        public const char EmptyCellChar = '.';
        public const char BlackPieceChar = 'B';
        public const char WhitePieceChar = 'W';
        public const char PossibleMoveChar = '0';
        readonly static int[,] directions = new int[,]
        {
            {-1, -1},
            {-1,  0},
            {-1, +1},
            { 0, +1},
            {+1, +1},
            {+1,  0},
            {+1, -1},
            { 0, -1},
        };

        char[,] grid;

        public Game(string initSquareString)
        {
            Initialize(initSquareString.ToLinedString());
        }

        public Game(string[] initLinedSquareStrings)
        {
            Initialize(initLinedSquareStrings);
        }

        public Game(char[,] grid)
        {
            this.grid = grid;
        }

        void Initialize(string[] initLinedSquareStrings)
        {
            if (initLinedSquareStrings.Length < 1)
            {
                return;
            }

            if (initLinedSquareStrings[0].Length < 1)
            {
                return;
            }

            grid = new char[initLinedSquareStrings.Length, initLinedSquareStrings[0].Length];
            for (var row = 0; row < initLinedSquareStrings.Length; ++row)
            {
                var rowString = initLinedSquareStrings[row];
                for (int column = 0; column < rowString.Length; ++column)
                {
                    grid[row, column] = rowString[column];
                }
            }
        }

        public char[,] GeneratePossibleMove(char piece)
        {
            var rowLength = grid.GetLength(0);
            var columnLength = grid.GetLength(1);

            for (int row = 0; row < rowLength; ++row)
            {
                for (int column = 0; column < columnLength; ++column)
                {
                    for (int directionIndex = 0; directionIndex < directions.GetLength(0); ++directionIndex)
                    {
                        var directionRow = directions[directionIndex, 0];
                        var directionColunm = directions[directionIndex, 1];
                        GeneratePossibleMove(row, column, directionRow, directionColunm, piece);
                    }
                }
            }

            return grid;
        }


        void GeneratePossibleMove(int row, int column, int directionRow, int directionColumn, char disc)
        {
            var character = grid[row, column];
            if(character != disc)
            {
                return;
            }

            var nextRow = row + directionRow;
            var nextColumn = column + directionColumn;
            var hasOppositeNeighbour = false;
            while (true)
            {
                if (!IsInTheGrid(nextRow, nextColumn))
                {
                    break;
                }

                var nextCell = grid[nextRow, nextColumn];
                if (IsOpposite(disc, nextCell))
                {
                    hasOppositeNeighbour = true;
                    nextRow = nextRow + directionRow;
                    nextColumn = nextColumn + directionColumn;
                    continue;
                }

                if (hasOppositeNeighbour && IsEmpty(nextCell))
                {
                    grid[nextRow, nextColumn] = PossibleMoveChar;

                }
                break;
            }
        }

        bool IsOpposite(char disc, char character)
        {
            if(disc == character)
            {
                return false;
            }

            return !IsEmpty(character);
        }

        bool IsInTheGrid(int row, int col)
        {
            return row >= 0 && col >= 0 && row < grid.GetLength(0) && col < grid.GetLength(1);
        }

        public bool IsEmpty(char character)
        {
            return character != BlackPieceChar && character != WhitePieceChar;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (var row = 0; row < grid.GetLength(0); ++row)
            {
                for (int column = 0; column < grid.GetLength(1); ++column)
                {
                    stringBuilder.Append(grid[row, column]);
                }

                if(row < grid.GetLength(0) - 1)
                {
                    stringBuilder.AppendLine();
                }
            }

            return stringBuilder.ToString();
        }
    }
}
