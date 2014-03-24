// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MazeRenderer.cs" company="">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace BeardedWallhackCSharp
{
    using System;
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
        ///     The maze.
        /// </summary>
        private Maze maze;

        /// <summary>
        /// The position.
        /// </summary>
        private Turtle turtle;

        /// <summary>
        ///     The room size.
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
        /// <param name="turtle">
        /// The position.
        /// </param>
        public MazeRenderer(Maze maze, Turtle turtle)
        {
            this.Maze = maze;
            this.Turtle = turtle;
        }

        #endregion

        #region Public Events

        /// <summary>
        ///     The force redraw required.
        /// </summary>
        public event EventHandler ForceRedrawRequired;

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

                EventHandler onEvent = this.ForceRedrawRequired;
                if (onEvent != null)
                {
                    onEvent(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        ///     Gets or sets the position.
        /// </summary>
        public Turtle Turtle
        {
            get
            {
                return this.turtle;
            }

            set
            {
                this.turtle = value;

                EventHandler onEvent = this.ForceRedrawRequired;
                if (onEvent != null)
                {
                    onEvent(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The render.
        /// </summary>
        public void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (this.maze == null)
            {
                return;
            }

            const double CellWallWidth = 0.2;
            const double WallPosition = 1 - CellWallWidth;

            Color baseFloorColour = Color.LightGray;
            Color wallColour = Color.Black;
            Color exitColour = Color.Green;
            Color startColour = Color.SkyBlue;

            GL.PushMatrix();
            GL.Translate(this.roomSize / 2, -this.roomSize / 2, 0.0);
            GL.Translate(-1, 1, 0.0);

            foreach (Block block in this.Maze.MazeBlocks)
            {
                GL.PushMatrix();
                Color floorColour;

                if (!block.InMaze)
                {
                    floorColour = wallColour;
                }
                else if (block.IsExit)
                {
                    floorColour = exitColour;
                }
                else if (block.CurrentState == Block.State.Visited)
                {
                    floorColour = startColour;
                }
                else
                {
                    floorColour = baseFloorColour;
                }


                GL.Translate(this.roomSize * block.PositionX, this.roomSize * -block.PositionY, 0.0);
                GL.Scale(this.roomSize / 2, this.roomSize / 2, 1.0);

                // Main block
                GL.Color3(floorColour);

                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex2(WallPosition, WallPosition);
                GL.Vertex2(-WallPosition, WallPosition);
                GL.Vertex2(-WallPosition, -WallPosition);
                GL.Vertex2(WallPosition, -WallPosition);
                GL.End();

                // CORNERS
                GL.Color3(wallColour);
                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex2(1, 1);
                GL.Vertex2(WallPosition, 1);
                GL.Vertex2(WallPosition, WallPosition);
                GL.Vertex2(1, WallPosition);
                GL.End();

                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex2(-1, -1);
                GL.Vertex2(-WallPosition, -1);
                GL.Vertex2(-WallPosition, -WallPosition);
                GL.Vertex2(-1, -WallPosition);
                GL.End();

                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex2(-1, 1);
                GL.Vertex2(-WallPosition, 1);
                GL.Vertex2(-WallPosition, WallPosition);
                GL.Vertex2(-1, WallPosition);
                GL.End();

                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex2(1, -1);
                GL.Vertex2(WallPosition, -1);
                GL.Vertex2(WallPosition, -WallPosition);
                GL.Vertex2(1, -WallPosition);
                GL.End();

                // DOORS
                GL.Color3(block.ExitTop ? floorColour : wallColour);
                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex2(-WallPosition, 1);
                GL.Vertex2(WallPosition, 1);
                GL.Vertex2(WallPosition, WallPosition);
                GL.Vertex2(-WallPosition, WallPosition);
                GL.End();

                GL.Color3(block.ExitRight ? floorColour : wallColour);
                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex2(1, WallPosition);
                GL.Vertex2(WallPosition, WallPosition);
                GL.Vertex2(WallPosition, -WallPosition);
                GL.Vertex2(1, -WallPosition);
                GL.End();

                GL.Color3(block.ExitBottom ? floorColour : wallColour);
                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex2(-WallPosition, -1);
                GL.Vertex2(WallPosition, -1);
                GL.Vertex2(WallPosition, -WallPosition);
                GL.Vertex2(-WallPosition, -WallPosition);
                GL.End();

                GL.Color3(block.ExitLeft ? floorColour : wallColour);
                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex2(-1, WallPosition);
                GL.Vertex2(-WallPosition, WallPosition);
                GL.Vertex2(-WallPosition, -WallPosition);
                GL.Vertex2(-1, -WallPosition);
                GL.End();

                GL.PopMatrix();
            }

            {
                GL.PushMatrix();

                GL.Translate(
                    this.roomSize * this.Turtle.Block.PositionX,
                    this.roomSize * -this.Turtle.Block.PositionY,
                    0.0);
                GL.Scale(this.roomSize / 2, this.roomSize / 2, 1.0);

                this.RenderTurtle();

                GL.PopMatrix();
            }

            GL.PopMatrix();
        }

        /// <summary>
        /// The render turtle.
        /// </summary>
        private void RenderTurtle()
        {
            GL.Begin(PrimitiveType.Triangles);

            GL.Color3(Color.Blue);
            
            if (this.Turtle.Direction == Maze.Direction.Right)
            {
                GL.Vertex2(-0.3, 0.6);
                GL.Vertex2(-0.3, -0.6);
                GL.Vertex2(0.3, 0);
            }

            if (this.Turtle.Direction == Maze.Direction.Up)
            {
                GL.Vertex2(0, 0.3);
                GL.Vertex2(-0.6, -0.3);
                GL.Vertex2(0.6, -0.3);
            }

            if (this.Turtle.Direction == Maze.Direction.Left)
            {
                GL.Vertex2(0.3, 0.6);
                GL.Vertex2(0.3, -0.6);
                GL.Vertex2(-0.3, 0);
            }

            if (this.Turtle.Direction == Maze.Direction.Down)
            {
                GL.Vertex2(0, -0.3);
                GL.Vertex2(-0.6, 0.3);
                GL.Vertex2(0.6, 0.3);
            }

            GL.End();
        }

        #endregion
    }
}