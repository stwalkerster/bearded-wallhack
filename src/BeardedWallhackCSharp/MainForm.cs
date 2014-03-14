// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="Simon Walker">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BeebMaze
{
    using System;
    using System.Threading;
    using System.Windows.Forms;

    using BeebMaze.Properties;
    using BeebMaze.Render;

    /// <summary>
    /// The main form.
    /// </summary>
    public partial class MainForm : Form
    {
        #region Fields

        /// <summary>
        /// The _maze lock.
        /// </summary>
        private readonly object _mazeLock = new object();

        // private Block _exitBlock;
        // private int _height;
        // private Block[,] _maze;

        /// <summary>
        /// The _maze panel.
        /// </summary>
        private MazeRenderScreen _mazePanel;

        /// <summary>
        /// The _real maze.
        /// </summary>
        private Maze _realMaze;

        /// <summary>
        /// The _regeneration thread.
        /// </summary>
        private Thread _regenerationThread;

        #endregion

        // private int _width;
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();
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
        private event EventHandler generationComplete;

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
        public void form1KeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            this._mazePanel.move(e.KeyCode);

            if (this._realMaze.currentBlock == this._realMaze.exitBlock)
            {
                this._realMaze.solve();
            }
        }

        /// <summary>
        /// The perform render.
        /// </summary>
        public void performRender()
        {
            if (this._mazePanel == null || (this._mazePanel.GetType() != typeof(Gl2MazeRenderScreen)))
            {
                this.panel1.Controls.Clear();
                this._mazePanel = MazeRenderScreen.Create();
                this._mazePanel.Dock = DockStyle.Fill;
                this.panel1.Controls.Add(this._mazePanel);
            }

            lock (this._mazeLock)
            {
                this._mazePanel.render(this._realMaze);
            }
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
        private void form1FormClosing(object sender, FormClosingEventArgs e)
        {
            this._regenerationThread.Abort();
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
        private void form1GenerationComplete(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                EventHandler eh = this.form1GenerationComplete;
                this.Invoke(eh, sender, e);
                return;
            }

            this.panel1.Controls.Add(this._mazePanel);

            this.panel1.Visible = true;
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
        private void form1Load(object sender, EventArgs e)
        {
            this.generationComplete += this.form1GenerationComplete;
            this.generateMaze(Settings.Default.MazeSize);
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
        private void form1Resize(object sender, EventArgs e)
        {
            this.performRender();
        }

        /// <summary>
        /// The generate maze.
        /// </summary>
        /// <param name="resolution">
        /// The resolution.
        /// </param>
        private void generateMaze(int resolution)
        {
            this.panel1.Visible = false;
            this.panel1.Controls.Clear();

            this.updateData("Initialising thread...", -1);

            Thread.Sleep(100);

            var tsd = new ThreadStartData
                          {
                              height = this.panel1.Width / resolution, 
                              width = this.panel1.Height / resolution, 
                          };

            this._regenerationThread = new Thread(this.regenerationThread_DoWork) { Priority = ThreadPriority.Lowest };
            this._regenerationThread.Start(tsd);
        }

        /// <summary>
        /// The regeneration thread_ do work.
        /// </summary>
        /// <param name="startData">
        /// The start data.
        /// </param>
        private void regenerationThread_DoWork(object startData)
        {
            var data = (ThreadStartData)startData;

            int width = data.width;
            int height = data.height;

            this.updateData("Generating maze", 0);

            var realMaze = new Maze(width, height);

            this.updateData("Rendering maze", 0);
            lock (this._mazeLock)
            {
                this._realMaze = realMaze;
            }

            this.Invoke(new Action(this.performRender));

            this.updateData("Drawing completed maze", -1);

            this.generationComplete(this, new EventArgs());
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
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.generateMaze(Settings.Default.MazeSize);
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
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this._realMaze.solve();
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
        private void updateData(string message = "", int percent = -2)
        {
            if (this.InvokeRequired)
            {
                // Thread.Sleep(10);
                UpdateDataDelegate udd = this.updateData;
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
            /// The height.
            /// </summary>
            public int height;

            /// <summary>
            /// The width.
            /// </summary>
            public int width;

            #endregion
        }
    }
}