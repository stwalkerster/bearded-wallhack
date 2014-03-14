// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MazePosition.cs" company="Simon Walker">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BeardedWallhackCSharp
{
    /// <summary>
    /// The maze position.
    /// </summary>
    public class MazePosition
    {
        #region Public Properties

        /// <summary>
        /// Gets the block.
        /// </summary>
        public Block Block { get; private set; }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        public Maze.Direction Direction { get; set; }

        #endregion
    }
}