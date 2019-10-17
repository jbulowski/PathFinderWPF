namespace PathFinderWPF
{
    public class Cell
    {

        /// <summary>
        /// Holds the column of the row
        /// </summary>
        public int column;

        /// <summary>
        /// Holds the row of the row
        /// </summary>
        public int row;

        /// <summary>
        /// Holds the ButtonType e.g. Start, End, Wall
        /// </summary>
        public ButtonType type
        {
            get; set;
        }

        /// <summary>
        /// Holds distance from start Cell
        /// </summary>
        public int gCost;

        /// <summary>
        /// Holds distance from end Cell
        /// </summary>
        public int hCost;

        /// <summary>
        /// Holds the sum of gCost and hCost
        /// </summary>
        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        /// <summary>
        /// Checks if a cell is walkable
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool IsCellWalkable
        {
            get
            {
                return (type == ButtonType.Walkable || type == ButtonType.End || type != ButtonType.Obsticle) ? true : false;
            }
        }

        /// <summary>
        /// Holds parent Cell so an path list can be created
        /// </summary>
        public Cell parent;

        public Cell(int column, int row, ButtonType type)
        {
            this.column= column;
            this.row = row;
            this.type= type;
        }
    }
}