// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMazeRenderer.cs" company="Simon Walker">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BeardedWallhackCSharp
{
    using System;

    /// <summary>
    /// The MazeRenderer interface.
    /// </summary>
    public interface IMazeRenderer
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the maze.
        /// </summary>
        Maze Maze { get; set; }

        /// <summary>
        ///     Gets or sets the position.
        /// </summary>
        Turtle Position { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The render.
        /// </summary>
        void Render();

        #endregion

        /// <summary>
        ///     The force redraw required.
        /// </summary>
        event EventHandler ForceRedrawRequired;
    }
}