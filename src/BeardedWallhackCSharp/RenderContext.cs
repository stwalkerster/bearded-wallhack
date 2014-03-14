// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderContext.cs" company="Simon Walker">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace BeardedWallhackCSharp
{
    /// <summary>
    ///     The render context.
    /// </summary>
    public class RenderContext
    {
        #region Public Properties

        /// <summary>
        /// Gets the maze.
        /// </summary>
        public Maze Maze { get; private set; }

        /// <summary>
        /// Gets the position.
        /// </summary>
        public MazePosition Position { get; private set; }

        #endregion
    }
}