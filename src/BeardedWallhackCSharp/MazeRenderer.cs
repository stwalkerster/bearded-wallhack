// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MazeRenderer.cs" company="Simon Walker">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace BeardedWallhackCSharp
{
    using System.Drawing;
    using System.Linq;

    using OpenTK.Graphics.OpenGL;

    /// <summary>
    ///     The maze renderer.
    /// </summary>
    public class MazeRenderer : IMazeRenderer
    {
        #region Fields

        /// <summary>
        /// The maze.
        /// </summary>
        private Maze maze;

        /// <summary>
        /// The room size.
        /// </summary>
        private double roomSize = 1.0;

        #endregion

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
        public Maze Maze
        {
            get
            {
                return this.maze;
            }

            set
            {
                this.maze = value;

                if (value != null)
                {
                    this.roomSize = new[] { 2.0 / this.Maze.Height, 2.0 / this.Maze.Width }.Min();
                }
            }
        }

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

            const double WallWidth = 0.8;
            Color baseFloorColour = Color.LightGray;
            Color wallColour = Color.Black;
            Color exitColour = Color.Green;

            GL.PushMatrix();
            GL.Translate(this.roomSize / 2, -this.roomSize / 2, 0.0);
            GL.Translate(-1, 1, 0.0);

            foreach (Block block in this.Maze)
            {
                GL.PushMatrix();
                var floorColour = block.IsExit ? exitColour : baseFloorColour;

                GL.Translate(this.roomSize * block.PositionX, this.roomSize * -block.PositionY, 0.0);
                GL.Scale(this.roomSize / 2, this.roomSize / 2, 1.0);
                
                // Main block
                GL.Color3(floorColour);

                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex2(WallWidth, WallWidth);
                GL.Vertex2(-WallWidth, WallWidth);
                GL.Vertex2(-WallWidth, -WallWidth);
                GL.Vertex2(WallWidth, -WallWidth);
                GL.End();

                // CORNERS
                GL.Color3(wallColour);
                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex2(1, 1);
                GL.Vertex2(WallWidth, 1);
                GL.Vertex2(WallWidth, WallWidth);
                GL.Vertex2(1, WallWidth);
                GL.End();

                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex2(-1, -1);
                GL.Vertex2(-WallWidth, -1);
                GL.Vertex2(-WallWidth, -WallWidth);
                GL.Vertex2(-1, -WallWidth);
                GL.End();

                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex2(-1, 1);
                GL.Vertex2(-WallWidth, 1);
                GL.Vertex2(-WallWidth, WallWidth);
                GL.Vertex2(-1, WallWidth);
                GL.End();

                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex2(1, -1);
                GL.Vertex2(WallWidth, -1);
                GL.Vertex2(WallWidth, -WallWidth);
                GL.Vertex2(1, -WallWidth);
                GL.End();

                // DOORS
                GL.Color3(block.ExitTop ? floorColour : wallColour);
                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex2(-WallWidth, 1);
                GL.Vertex2(WallWidth, 1);
                GL.Vertex2(WallWidth, WallWidth);
                GL.Vertex2(-WallWidth, WallWidth);
                GL.End();

                GL.Color3(block.ExitRight ? floorColour : wallColour);
                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex2(1, WallWidth);
                GL.Vertex2(WallWidth, WallWidth);
                GL.Vertex2(WallWidth, -WallWidth);
                GL.Vertex2(1, -WallWidth);
                GL.End();

                GL.Color3(block.ExitBottom ? floorColour : wallColour);
                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex2(-WallWidth, -1);
                GL.Vertex2(WallWidth, -1);
                GL.Vertex2(WallWidth, -WallWidth);
                GL.Vertex2(-WallWidth, -WallWidth);
                GL.End();

                GL.Color3(block.ExitLeft ? floorColour : wallColour);
                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex2(-1, WallWidth);
                GL.Vertex2(-WallWidth, WallWidth);
                GL.Vertex2(-WallWidth, -WallWidth);
                GL.Vertex2(-1, -WallWidth);
                GL.End();
                
                GL.PopMatrix();
            }

            GL.PopMatrix();
        }

        #endregion
    }
}