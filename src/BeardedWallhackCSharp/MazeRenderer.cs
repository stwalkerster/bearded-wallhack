// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MazeRenderer.cs" company="Simon Walker">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace BeardedWallhackCSharp
{
    using OpenTK.Graphics.OpenGL;

    /// <summary>
    ///     The maze renderer.
    /// </summary>
    public class MazeRenderer : IMazeRenderer
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initialises a new instance of the <see cref="MazeRenderer"/> class.
        /// </summary>
        /// <param name="maze">
        /// The maze.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        public MazeRenderer(Maze maze, MazePosition position)
        {
            this.Maze = maze;
            this.Position = position;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the maze.
        /// </summary>
        public Maze Maze { get; set; }

        /// <summary>
        ///     Gets or sets the position.
        /// </summary>
        public MazePosition Position { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The render.
        /// </summary>
        public void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex2(0.5, 0.5);
            GL.Vertex2(-0.5, 0.5);
            GL.Vertex2(-0.5, -0.5);
            GL.Vertex2(0.5, -0.5);
            GL.End();
        }

        #endregion
    }
}