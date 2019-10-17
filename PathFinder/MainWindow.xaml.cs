using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PathFinderWPF
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Holds the grid object
        /// </summary>
        private Grid grid;

        /// <summary>
        /// Holds amounts of attempts to find a path
        /// </summary>
        private int findPathAttempt;

        /// <summary>
        /// List of UI Buttons
        /// </summary>
        public List<Button> buttons;

        public MainWindow()
        {
            InitializeComponent();

            findPathAttempt = 0;

            var startCell = new Cell(2, 2, ButtonType.Start);
            var endCell = new Cell(17, 17, ButtonType.Start);

            grid = new Grid(20, 20, startCell, endCell);

            buttons = Container.Children.Cast<Button>().ToList();

            foreach (Button button in buttons)
            {
                // Using full namespace, because it collides
                // with the PathFinderWPF.Grid class
                var column = System.Windows.Controls.Grid.GetColumn(button);
                var row = System.Windows.Controls.Grid.GetRow(button);

                if (row == grid.startCell.row && column == grid.startCell.column)
                    button.Content = "Start";

                if (row == grid.endCell.row && column == grid.endCell.column)
                    button.Content = "End";
            }
        }

        /// <summary>
        /// Handles a button click event
        /// </summary>
        /// <param name="sender">The button that was clicked</param>
        /// <param name="e">The events of the click</param>
        private void Button(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            // Using full namespace, because it collides
            // with the PathFinderWPF.Grid class
            var column = System.Windows.Controls.Grid.GetColumn(button);
            var row = System.Windows.Controls.Grid.GetRow(button);

            if (!grid.isReservedCell(column, row))
            {
                var clickedCell = grid.cells[row, column];

                if (clickedCell.type != ButtonType.Obsticle)
                {
                    clickedCell.type = ButtonType.Obsticle;
                    button.Background = Brushes.Black;
                    button.Content = "";
                } else
                {
                    clickedCell.type = ButtonType.Walkable;
                    button.Background = Brushes.LightGray;
                    button.Content = "";
                }
            }
        }

        /// <summary>
        /// Finds path from start cell to end cell
        /// </summary>
        /// <param name="sender">The button that was clicked</param>
        /// <param name="e">The events of the click</param>
        private void FindPath(object sender, RoutedEventArgs e)
        {

            findPathAttempt++;

            Cell startCell = grid.cells[grid.startCell.row, grid.startCell.column];
            Cell endCell = grid.cells[grid.endCell.row, grid.endCell.column];

            var openSet = new List<Cell>();
            var closedSet = new HashSet<Cell>();

            openSet.Add(startCell);

            while (openSet.Count > 0)
            {

                Cell currentCell = openSet[0];
                
                for (var i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentCell.fCost || openSet[i].fCost == currentCell.fCost && openSet[i].hCost < currentCell.hCost)
                        currentCell = openSet[i];
                }

                openSet.Remove(currentCell);
                closedSet.Add(currentCell);

                // path found
                if (currentCell == endCell || currentCell.type == ButtonType.End)
                {
                    grid.RetracePath(startCell, endCell);

                    foreach (Cell cell in grid.cells)
                    {
                        if (grid.path != null)
                        {
                            if (grid.path.Contains(cell))
                            {

                                // Removing end cell from path
                                // so it doesnt get animated
                                grid.path.Remove(endCell);
                                
                                foreach (Button button in buttons)
                                {

                                    // Using full namespace, because it collides
                                    // with the PathFinderWPF.Grid class
                                    var row = System.Windows.Controls.Grid.GetRow(button);
                                    var column = System.Windows.Controls.Grid.GetColumn(button);

                                    foreach (Cell pathCell in grid.path)
                                    {
                                        if (pathCell.row == row && pathCell.column == column)
                                        {
                                            button.Background = Brushes.Red;
                                            button.Content = findPathAttempt;

                                        }
                                    }
                                }
                            }
                        }
                    }

                    return;
                }

                foreach (var neighbour in grid.NeighboursOf(currentCell))
                {
                    if (!neighbour.IsCellWalkable || closedSet.Contains(neighbour))
                        continue;

                    // int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                    int newCostToNeighbour = currentCell.gCost + grid.getDistanceBetweenCells(currentCell, neighbour);

                    // if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        // we need to calculate the new fCost
                        // so we set gCost and hCost
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = grid.getDistanceBetweenCells(neighbour, endCell);
                        neighbour.parent = currentCell;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }

            }
        }

        /// <summary>
        /// Resets the UI 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reset(object sender, RoutedEventArgs e)
        {
            findPathAttempt = 0;

            // Iterate over UI buttons

            foreach (Button button in buttons)
            {
                // Using full namespace, because it collides
                // with the PathFinderWPF.Grid class
                var row = System.Windows.Controls.Grid.GetRow(button);
                var column = System.Windows.Controls.Grid.GetColumn(button);
                if (!grid.isReservedCell(row, column))
                {
                        button.Content = "";
                        button.Background = Brushes.LightGray;
                }
            }

            // Iterate over grid cells
            foreach (Cell cell in grid.cells)
            {
                if (cell != grid.startCell && cell != grid.endCell)
                {
                    cell.type = ButtonType.Walkable;
                }
            }

            FindPathButton.Content = "Find Path";
            ResetButton.Content = "Reset";
        }
    }
}