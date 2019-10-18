using System;
using System.Collections.Generic;

namespace PathFinderWPF
{
    public class Grid
    {

        /// <summary>
        /// Holds the amount of columns
        /// </summary>
        public int column_length;

        /// <summary>
        ///  Holds the amount of rows
        /// </summary>
        public int row_length;

        /// <summary>
        /// Represents the start cell
        /// </summary>
        public Cell startCell;

        /// <summary>
        /// Represents the end cell
        /// </summary>
        public Cell endCell;

        /// <summary>
        /// Represents the grid
        /// </summary>
        public Cell[,] cells;

        /// <summary>
        /// Creates the grid representation
        /// </summary>
        /// <param name="row_length">The amount of rows</param>
        /// <param name="column_length">The amount of columns</param>
        /// <param name="startCell">The starting cell</param>
        /// <param name="endCell">The ending cell</param>
        public Grid(int row_length, int column_length, Cell startCell, Cell endCell)
        {

            this.row_length = row_length;
            this.column_length = column_length;

            this.startCell = startCell;
            this.endCell = endCell;

            cells = new Cell[row_length, column_length];

            for (var row = 0; row < row_length; row++)
            {
                for (var column = 0; column < column_length; column++)
                {
                    // Set type to walkable to every cell
                    var type = ButtonType.Walkable;

                    // Set type to start when cell is a start cell
                    if (column == startCell.column && row == startCell.row)
                        type = ButtonType.Start;

                    // Set type to start when cell is a end cell
                    if (column == endCell.column && row == endCell.row)
                        type = ButtonType.End;

                    cells[row, column] = new Cell(column, row, type);
                }
            }
        }

        /// <summary>
        /// Checks if a cell is reserved
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool isReservedCell(int column, int row)
        {
            // Check if clicked cell is a start cell
            if (startCell.column == column && startCell.row == row) return true;

            // Check if clicked cell is a end cell
            if (endCell.column == column && endCell.row == row) return true;

            return false;
        }

        /// <summary>
        /// Get all possible neighbours of a cell
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public List<Cell> NeighboursOf(Cell cell)
        {
            var neighbours = new List<Cell>();

            for (var row = -1; row <= 1; row++)
            {
                for (var column = -1; column <= 1; column++)
                {
                    if (row == 0 && column == 0) continue;

                    var checkRow = cell.row + row;
                    var checkColumn = cell.column + column;

                    if (checkRow >= 0 && checkRow < row_length && checkColumn >= 0 && checkColumn < column_length)
                        neighbours.Add(cells[checkRow, checkColumn]);
                }
            }

            return neighbours;
        }

        /// <summary>
        /// Calculates the distance between cells
        /// 14 points for a diagonal move
        /// 10 points for a directional move
        /// </summary>
        /// <param name="cellA"></param>
        /// <param name="cellB"></param>
        /// <returns></returns>
        public int getDistanceBetweenCells(Cell cellA, Cell cellB)
        {
            int distanceRow = Math.Abs(cellA.row - cellB.row);
            int distanceColumn = Math.Abs(cellA.column - cellB.column);

            if (distanceRow > distanceColumn)
                return 14 * distanceColumn + 10 * (distanceRow - distanceColumn);

            return 14 * distanceRow + 10 * (distanceColumn - distanceRow);
        }

        /// <summary>
        /// Build a path consisting of a cells that own a parent cell
        /// These parent cells build up the path
        /// </summary>
        /// <param name="startCell"></param>
        /// <param name="endCell"></param>
        public List<Cell> RetracePath(Cell startCell, Cell endCell)
        {
            var path = new List<Cell>();
            var currentCell = endCell;

            while (currentCell != startCell)
            {
                path.Add(currentCell);
                currentCell = currentCell.parent;
            }

            path.Reverse();
            return path;
        }
    }
}