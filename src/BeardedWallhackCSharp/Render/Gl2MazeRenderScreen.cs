// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Gl2MazeRenderScreen.cs" company="Simon Walker">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace BeardedWallhackCSharp.Render
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    using BeardedWallhackCSharp;

    using Tao.OpenGl;

    /// <summary>
    ///     The gl 2 maze render screen.
    /// </summary>
    public partial class Gl2MazeRenderScreen : MazeRenderScreen
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Gl2MazeRenderScreen"/> class. 
        /// </summary>
        public Gl2MazeRenderScreen()
        {
            this.InitializeComponent();
            this.rendererToolStripStatusLabel.Text = string.Format(
                this.rendererToolStripStatusLabel.Tag.ToString(), 
                "2D OpenGL");
            Gl.glMatrixMode(Gl.GL_MODELVIEW);

            this.simpleOpenGlControl1.Paint += this.simpleOpenGlControl1_Paint;
            this.simpleOpenGlControl1.PreviewKeyDown += Program.app.form1KeyDown;
        }

        /// <summary>
        /// Finalises an instance of the <see cref="Gl2MazeRenderScreen"/> class. 
        /// </summary>
        ~Gl2MazeRenderScreen()
        {
            this.simpleOpenGlControl1.DestroyContexts();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The render.
        /// </summary>
        /// <param name="pmaze">
        /// The pmaze.
        /// </param>
        public override void render(Maze pmaze)
        {
            base.render(pmaze);
            this.simpleOpenGlControl1.Invalidate();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The draw cube.
        /// </summary>
        /// <param name="x1">
        /// The x 1.
        /// </param>
        /// <param name="y1">
        /// The y 1.
        /// </param>
        /// <param name="x2">
        /// The x 2.
        /// </param>
        /// <param name="y2">
        /// The y 2.
        /// </param>
        /// <param name="is3d">
        /// The is 3 d.
        /// </param>
        protected override void drawCube(float x1, float y1, float x2, float y2, bool is3d = false)
        {
            Gl.glBegin(Gl.GL_POLYGON);
            Gl.glVertex3f(x1, y1, 0f);
            Gl.glVertex3f(x2, y1, 0f);
            Gl.glVertex3f(x2, y2, 0f);
            Gl.glVertex3f(x1, y2, 0f);

            Gl.glEnd();
        }

        /// <summary>
        ///     The draw gridlines.
        /// </summary>
        protected void drawGridlines()
        {
            Gl.glColor3f(1, 0, 0);
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex3f(-10, 0, 0);
            Gl.glVertex3f(10, 0, 0);
            Gl.glEnd();
            Gl.glColor3f(0, 1, 0);
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex3f(0, -10, 0);
            Gl.glVertex3f(0, 10, 0);
            Gl.glEnd();
            Gl.glColor3f(0, 0, 1);
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex3f(0, 0, -10);
            Gl.glVertex3f(0, 0, 10);
            Gl.glEnd();
        }

        /// <summary>
        /// The perform move.
        /// </summary>
        /// <param name="direction">
        /// The direction.
        /// </param>
        protected override void performMove(Keys direction)
        {
            switch (direction)
            {
                case Keys.W:
                    this.maze.move(Maze.Direction.UP);
                    break;
                case Keys.A:
                    this.maze.move(Maze.Direction.LEFT);
                    break;
                case Keys.S:
                    this.maze.move(Maze.Direction.DOWN);
                    break;
                case Keys.D:
                    this.maze.move(Maze.Direction.RIGHT);
                    break;
            }
        }

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
        protected override void positionScene(int cols, int rows, float[,] xvertices, float[,] yvertices)
        {
            float maxX = xvertices[(2 * cols) + 1, (2 * rows) + 1];
            float maxY = yvertices[(2 * cols) + 1, (2 * rows) + 1];

            float scaleX = 2 / maxX;
            float scaleY = 2 / maxY;

            Gl.glTranslatef(-1, 1, 0);
            Gl.glScalef(scaleX, -scaleY, 1);
        }

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
        protected override void setColour(float r, float g, float b)
        {
            Gl.glColor3f(r, g, b);
        }

        /// <summary>
        /// The set colour.
        /// </summary>
        /// <param name="v">
        /// The v.
        /// </param>
        protected override void setColour(float[] v)
        {
            Gl.glColor3fv(v);
        }

        /// <summary>
        /// The simple open gl control 1_ size changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected virtual void simpleOpenGlControl1_SizeChanged(object sender, EventArgs e)
        {
            Gl.glViewport(0, 0, this.simpleOpenGlControl1.Size.Width, this.simpleOpenGlControl1.Size.Height);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
        }

        /// <summary>
        /// Handles the Paint event of the simpleOpenGlControl1 control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.
        /// </param>
        private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
        {
            if (this.maze == null)
            {
                this.maze = this.lastKnownMaze;
            }

            if (this.maze == null)
            {
                return;
            }

            Gl.glLoadIdentity();

            // background - use unvisitedblock
            float[] clearCol = this.getColour(Color.Magenta);
            Gl.glClearColor(clearCol[0], clearCol[1], clearCol[2], 1f);

            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            Gl.glPushMatrix();
            {
                this.drawScene();
            }

            Gl.glPopMatrix();
        }

        #endregion
    }
}