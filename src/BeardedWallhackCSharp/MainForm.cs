// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="Simon Walker">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BeardedWallhackCSharp
{
    using System;
    using System.Threading;
    using System.Windows.Forms;

    using BeardedWallhackCSharp.Properties;

    /// <summary>
    /// The main form.
    /// </summary>
    public partial class MainForm : Form
    {
        #region Fields

        /// <summary>
        /// The _maze lock.
        /// </summary>
        private readonly object mazeLock = new object();

        /// <summary>
        /// The _real maze.
        /// </summary>
        private Maze realMaze;

        /// <summary>
        /// The maze renderer.
        /// </summary>
        private IMazeRenderer mazeRenderer;

        /// <summary>
        /// The _regeneration thread.
        /// </summary>
        private Thread regenerationThread;

        #endregion

        // private int _width;
        #region Constructors and Destructors

        /// <summary>
        /// Initialises a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();
            this.mazeRenderer = new MazeRenderer(null, null);
        }

        #endregion

        #region Delegates

        /// <summary>
        /// The update data delegate.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="percent">
        /// The percent.
        /// </param>
        private delegate void UpdateDataDelegate(string message, int percent);

        #endregion

        // private Block currentBlock { get; set; }
        #region Events

        /// <summary>
        /// The generation complete.
        /// </summary>
        private event EventHandler GenerationComplete;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The form 1 key down.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        public void Form1KeyDown(object sender, PreviewKeyDownEventArgs e)
        {
        }

        /// <summary>
        /// The perform render.
        /// </summary>
        public void PerformRender()
        {
            this.glControl1.Invalidate(); 
        }

        #endregion

        #region Methods

        /// <summary>
        /// The form 1 form closing.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Form1FormClosing(object sender, FormClosingEventArgs e)
        {
            this.regenerationThread.Abort();
        }

        /// <summary>
        /// The form 1 generation complete.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Form1GenerationComplete(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                EventHandler eh = this.Form1GenerationComplete;
                this.Invoke(eh, sender, e);
                return;
            }

            this.panel1.Visible = true;

            this.mazeRenderer.Maze = this.realMaze;
            this.mazeRenderer.Position = new MazePosition(this.realMaze[0, 0], Maze.Direction.Down);
            this.glControl1.Invalidate();
        }

        /// <summary>
        /// The form 1 load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Form1Load(object sender, EventArgs e)
        {
            this.GenerationComplete += this.Form1GenerationComplete;
            this.GenerateMaze(Settings.Default.MazeSize);

            this.glControl1.MakeCurrent();
        }

        /// <summary>
        /// The form 1 resize.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Form1Resize(object sender, EventArgs e)
        {
            this.glControl1.Invalidate();
        }

        /// <summary>
        /// The generate maze.
        /// </summary>
        /// <param name="resolution">
        /// The resolution.
        /// </param>
        private void GenerateMaze(int resolution)
        {
            this.panel1.Visible = false;
            this.panel1.Controls.Clear();

            this.UpdateData("Initialising thread...", -1);

            Thread.Sleep(100);

            var tsd = new ThreadStartData
                          {
                              Height = this.panel1.Width / resolution, 
                              Width = this.panel1.Height / resolution, 
                          };

            this.regenerationThread = new Thread(this.RegenerationThreadDoWork) { Priority = ThreadPriority.Lowest };
            this.regenerationThread.Start(tsd);
        }

        /// <summary>
        /// The regeneration thread_ do work.
        /// </summary>
        /// <param name="startData">
        /// The start data.
        /// </param>
        private void RegenerationThreadDoWork(object startData)
        {
            var data = (ThreadStartData)startData;

            int width = data.Width;
            int height = data.Height;

            this.UpdateData("Generating maze", 0);

            var maze = new Maze(width, height);

            this.UpdateData("Rendering maze", 0);
            lock (this.mazeLock)
            {
                this.realMaze = maze;
            }

            this.Invoke(new Action(this.PerformRender));

            this.UpdateData("Drawing completed maze", -1);

            this.GenerationComplete(this, new EventArgs());
        }

        /// <summary>
        /// The tool strip button 1_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ToolStripButton1Click(object sender, EventArgs e)
        {
            this.GenerateMaze(Settings.Default.MazeSize);
        }

        /// <summary>
        /// The tool strip button 3_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ToolStripButton3Click(object sender, EventArgs e)
        {
           
        }

        /// <summary>
        /// The update data.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="percent">
        /// The percent.
        /// </param>
        private void UpdateData(string message = "", int percent = -2)
        {
            if (this.InvokeRequired)
            {
                // Thread.Sleep(10);
                UpdateDataDelegate udd = this.UpdateData;
                this.Invoke(udd, message, percent);
                return;
            }

            if (message != string.Empty)
            {
                this.label2.Text = message;
            }

            if (percent <= 100 && percent >= 0)
            {
                this.progressBar1.Style = ProgressBarStyle.Continuous;
                this.progressBar1.Value = percent;
            }

            if (percent == -1)
            {
                this.progressBar1.Style = ProgressBarStyle.Marquee;
            }
        }

        #endregion

        /// <summary>
        /// The thread start data.
        /// </summary>
        private struct ThreadStartData
        {
            #region Fields

            /// <summary>
            /// The Height.
            /// </summary>
            public int Height;

            /// <summary>
            /// The Width.
            /// </summary>
            public int Width;

            #endregion
        }

        /// <summary>
        /// Repaint the OpenGL control
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void GlControl1Paint(object sender, PaintEventArgs e)
        {
            this.mazeRenderer.Render();

            glControl1.SwapBuffers();
        }
    }
}