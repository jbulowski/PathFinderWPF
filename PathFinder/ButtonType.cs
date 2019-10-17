namespace PathFinderWPF
{
    public enum ButtonType
    {
        /// <summary>
        /// This cell is free
        /// </summary>
        Walkable,

        /// <summary>
        /// This cell is a start cell
        /// </summary>
        Start,

        /// <summary>
        /// This cell is an obsticle i.e. not walkable
        /// </summary>
        Obsticle,

        /// <summary>
        /// This cell is a end cell
        /// </summary>
        End
    }
}