// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MazeRenderScreen.cs" company="Simon Walker">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace BeardedWallhackCSharp.Render
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    using BeardedWallhackCSharp;

    using BeardedWallhackCSharp.Properties;

    /// <summary>
    ///     The maze render screen.
    /// </summary>
    public abstract partial class MazeRenderScreen : UserControl
    {
        #region Fields

        /// <summary>
        ///     The last known maze.
        /// </summary>
        protected Maze lastKnownMaze;

        /// <summary>
        ///     The maze.
        /// </summary>
        protected Maze maze;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initialises a new instance of the <see cref="MazeRenderScreen" /> class.
        /// </summary>
        protected MazeRenderScreen()
        {
            this.InitializeComponent();
            this.rendererToolStripStatusLabel.Text = string.Format(
                this.rendererToolStripStatusLabel.Tag.ToString(), 
                "Generic");
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The move.
        /// </summary>
        /// <param name="direction">
        /// The direction.
        /// </param>
        public void move(Keys direction)
        {
            if (direction == Keys.Up)
            {
                direction = Keys.W;
            }

            if (direction == Keys.Down)
            {
                direction = Keys.S;
            }

            if (direction == Keys.Left)
            {
                direction = Keys.A;
            }

            if (direction == Keys.Right)
            {
                direction = Keys.D;
            }

            if (direction == Keys.W || direction == Keys.A || direction == Keys.S || direction == Keys.D)
            {
                this.performMove(direction);
            }
        }

        /// <summary>
        /// The render.
        /// </summary>
        /// <param name="pmaze">
        /// The pmaze.
        /// </param>
        public virtual void render(Maze pmaze)
        {
            if (pmaze == null)
            {
                pmaze = this.lastKnownMaze;
            }
            else
            {
                this.lastKnownMaze = pmaze;
            }

            this.maze = pmaze;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The create.
        /// </summary>
        /// <returns>
        ///     The <see cref="MazeRenderScreen" />.
        /// </returns>
        internal static MazeRenderScreen Create()
        {
            return (MazeRenderScreen)Activator.CreateInstance(typeof(Gl2MazeRenderScreen));
        }

        /// <summary>
        /// Draws the cube.
        /// </summary>
        /// <param name="x1">
        /// The x1.
        /// </param>
        /// <param name="y1">
        /// The y1.
        /// </param>
        /// <param name="x2">
        /// The x2.
        /// </param>
        /// <param name="y2">
        /// The y2.
        /// </param>
        /// <param name="is3d">
        /// if set to <c>true</c> [is3d].
        /// </param>
        protected abstract void drawCube(float x1, float y1, float x2, float y2, bool is3d = false);

        /// <summary>
        ///     The draw scene.
        /// </summary>
        protected void drawScene()
        {
            int rows = this.maze.Height, cols = this.maze.Width;

            // x,y
            var xvertices = new float[(2 * cols) + 2, (2 * rows) + 2];
            var yvertices = new float[(2 * cols) + 2, (2 * rows) + 2];

            const float cellWidth = 0.3f;
            const float cellHeight = 0.3f;

            const float wallWidth = 0.1f;
            const float wallHeight = 0.1f;

            float xval = 0;
            for (int x = 0; x < (2 * cols) + 2; x++)
            {
                if (x != 0)
                {
                    xval += x % 2 == 0 ? cellWidth : wallWidth;
                }

                float yval = 0;
                for (int y = 0; y < (2 * rows) + 2; y++)
                {
                    if (y != 0)
                    {
                        yval += y % 2 == 0 ? cellHeight : wallHeight;
                    }

                    xvertices[x, y] = xval;
                    yvertices[x, y] = yval;
                }
            }

            this.positionScene(cols, rows, xvertices, yvertices);

            this.setColour(this.getColour(Settings.Default.ColorWalls));

            for (int x = 0; x <= cols; x++)
            {
                for (int y = 0; y <= rows; y++)
                {
                    this.drawCube(
                        xvertices[x * 2, y * 2], 
                        yvertices[x * 2, y * 2], 
                        xvertices[(x * 2) + 1, (y * 2) + 1], 
                        yvertices[(x * 2) + 1, (y * 2) + 1], 
                        true);
                }
            }

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    Block cell = this.maze[x, y];

                    

                    if (!cell.ExitTop)
                    {
                        this.setColour(this.getColour(Settings.Default.ColorWalls));

                        this.drawCube(
                            xvertices[(x * 2) + 1, y * 2], 
                            yvertices[(x * 2) + 1, y * 2], 
                            xvertices[(x * 2) + 2, (y * 2) + 1], 
                            yvertices[(x * 2) + 2, (y * 2) + 1], 
                            true);
                    }
                    else
                    {
                        this.setColour(this.getColour(this.getDoorColour(cell.WallTop, cell)));
                        this.drawCube(
                            xvertices[(x * 2) + 1, y * 2], 
                            yvertices[(x * 2) + 1, y * 2], 
                            xvertices[(x * 2) + 2, (y * 2) + 1], 
                            yvertices[(x * 2) + 2, (y * 2) + 1]);
                    }

                    if (!cell.ExitLeft)
                    {
                        this.setColour(this.getColour(Settings.Default.ColorWalls));

                        this.drawCube(
                            xvertices[x * 2, (y * 2) + 1], 
                            yvertices[x * 2, (y * 2) + 1], 
                            xvertices[(x * 2) + 1, (y * 2) + 2], 
                            yvertices[(x * 2) + 1, (y * 2) + 2], 
                            true);
                    }
                    else
                    {
                        this.setColour(this.getColour(this.getDoorColour(cell.WallLeft, cell)));
                        this.drawCube(
                            xvertices[x * 2, (y * 2) + 1], 
                            yvertices[x * 2, (y * 2) + 1], 
                            xvertices[(x * 2) + 1, (y * 2) + 2], 
                            yvertices[(x * 2) + 1, (y * 2) + 2]);
                    }

                    if ((x + 1) == cols)
                    {
                        this.setColour(this.getColour(Settings.Default.ColorWalls));

                        this.drawCube(
                            xvertices[(x * 2) + 2, (y * 2) + 1], 
                            yvertices[(x * 2) + 2, (y * 2) + 1], 
                            xvertices[(x * 2) + 3, (y * 2) + 2], 
                            yvertices[(x * 2) + 3, (y * 2) + 2], 
                            true);
                    }

                    if ((y + 1) == rows)
                    {
                        this.setColour(this.getColour(Settings.Default.ColorWalls));

                        this.drawCube(
                            xvertices[(x * 2) + 1, (y * 2) + 2], 
                            yvertices[(x * 2) + 1, (y * 2) + 2], 
                            xvertices[(x * 2) + 2, (y * 2) + 3], 
                            yvertices[(x * 2) + 3, (y * 2) + 3], 
                            true);
                    }

                    

                    #region cells

                    switch (cell.CurrentState)
                    {
                        case Block.State.Current:
                            this.setColour(this.getColour(Settings.Default.ColorCurrentBlock));
                            break;
                        case Block.State.Exit:
                            this.setColour(this.getColour(Settings.Default.ColorExitBlock));
                            break;
                        case Block.State.Unvisited:
                            this.setColour(
                                this.getColour(
                                    cell.Hidden ? Settings.Default.ColorWalls : Settings.Default.ColorUnvisitedBlock));
                            break;
                        case Block.State.Visited:
                            this.setColour(this.getColour(Settings.Default.ColorVisitedBlock));
                            break;
                    }

                    this.drawCube(
                        xvertices[(x * 2) + 1, (y * 2) + 1], 
                        yvertices[(x * 2) + 1, (y * 2) + 1], 
                        xvertices[(x * 2) + 2, (y * 2) + 2], 
                        yvertices[(x * 2) + 2, (y * 2) + 2]);

                    #endregion
                }
            }
        }

        /// <summary>
        /// The get colour.
        /// </summary>
        /// <param name="color">
        /// The color.
        /// </param>
        /// <returns>
        /// The <see cref="float[]"/>.
        /// </returns>
        protected virtual float[] getColour(Color color)
        {
            var colvector = new float[3];

            colvector[0] = (float)color.R / 255;
            colvector[1] = (float)color.G / 255;
            colvector[2] = (float)color.B / 255;

            return colvector;
        }

        /// <summary>
        /// The get door colour.
        /// </summary>
        /// <param name="w">
        /// The w.
        /// </param>
        /// <param name="currentCell">
        /// The current cell.
        /// </param>
        /// <returns>
        /// The <see cref="Color"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        protected Color getDoorColour(Wall w, Block currentCell)
        {
            var doorState = Block.State.Unvisited;

            if (currentCell.Hidden && w.getOpposite(currentCell).Hidden)
            {
                return Settings.Default.ColorWalls;
            }

            if (!Settings.Default.UseFancyDoors)
            {
                return Settings.Default.ColorDoors;
            }

            if (w.getOpposite(currentCell).CurrentState != currentCell.CurrentState)
            {
                // check exit and current states

                // use non-current state for when it's next to each other
                if (currentCell.CurrentState == Block.State.Current)
                {
                    doorState = w.getOpposite(currentCell).CurrentState;
                }

                if (w.getOpposite(currentCell).CurrentState == Block.State.Current)
                {
                    doorState = currentCell.CurrentState;
                }

                if (currentCell.CurrentState == Block.State.Exit)
                {
                    doorState = w.getOpposite(currentCell).CurrentState;
                }

                if (w.getOpposite(currentCell).CurrentState == Block.State.Exit)
                {
                    doorState = currentCell.CurrentState;
                }

                if (!this.maze.isSolved)
                {
                    if (currentCell.CurrentState == Block.State.Exit
                        && w.getOpposite(currentCell).CurrentState == Block.State.Current)
                    {
                        doorState = Block.State.Unvisited;
                    }

                    if (currentCell.CurrentState == Block.State.Current
                        && w.getOpposite(currentCell).CurrentState == Block.State.Exit)
                    {
                        doorState = Block.State.Visited;
                    }
                }
                else
                {
                    if (currentCell.CurrentState == Block.State.Exit
                        && w.getOpposite(currentCell).CurrentState == Block.State.Current)
                    {
                        doorState = Block.State.Current;
                    }

                    if (currentCell.CurrentState == Block.State.Current
                        && w.getOpposite(currentCell).CurrentState == Block.State.Exit)
                    {
                        doorState = Block.State.Current;
                    }
                }
            }
            else
            {
                // same state, mark as same colour
                doorState = currentCell.CurrentState;
            }

            switch (doorState)
            {
                case Block.State.Unvisited:
                    return Settings.Default.ColorDoors;
                case Block.State.Current:
                    return Settings.Default.ColorCurrentBlock;
                case Block.State.Visited:
                    return Settings.Default.ColorVisitedBlock;
                case Block.State.Exit:
                    return Settings.Default.ColorExitBlock;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// The perform move.
        /// </summary>
        /// <param name="direction">
        /// The direction.
        /// </param>
        protected abstract void performMove(Keys direction);

        /// <summary>
        /// The position scene.
        /// </summary>
        /// <param name="cols">
        /// The cols.
        /// </param>
        /// <param name="rows">
        /// The rows.
        /// </param>
        /// <param name="xvertices">
        /// The xvertices.
        /// </param>
        /// <param name="yvertices">
        /// The yvertices.
        /// </param>
        protected abstract void positionScene(int cols, int rows, float[,] xvertices, float[,] yvertices);

        /// <summary>
        /// The set colour.
        /// </summary>
        /// <param name="r">
        /// The r.
        /// </param>
        /// <param name="g">
        /// The g.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        protected abstract void setColour(float r, float g, float b);

        /// <summary>
        /// The set colour.
        /// </summary>
        /// <param name="v">
        /// The v.
        /// </param>
        protected abstract void setColour(float[] v);

        /// <summary>
        /// The maze render screen_ paint.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void MazeRenderScreen_Paint(object sender, PaintEventArgs e)
        {
        }

        #endregion
    }
}