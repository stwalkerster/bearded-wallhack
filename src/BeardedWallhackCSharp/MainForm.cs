// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace BeardedWallhackCSharp
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading;
    using System.Windows.Forms;

    using BeardedWallhackCSharp.Properties;

    using OpenTK.Graphics.OpenGL;

    using SharpLua;
    using SharpLua.LuaTypes;

    /// <summary>
    ///     The main form.
    /// </summary>
    public partial class MainForm : Form
    {
        #region Fields

        /// <summary>
        ///     The _maze lock.
        /// </summary>
        private readonly object mazeLock = new object();

        /// <summary>
        ///     The maze renderer.
        /// </summary>
        private readonly IMazeRenderer mazeRenderer;

        /// <summary>
        ///     The _real maze.
        /// </summary>
        private Maze realMaze;

        /// <summary>
        ///     The _regeneration thread.
        /// </summary>
        private Thread regenerationThread;

        private Turtle turtle;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initialises a new instance of the <see cref="MainForm" /> class.
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();
            this.mazeRenderer = new MazeRenderer(null, null);
            this.mazeRenderer.ForceRedrawRequired += this.MazeRendererOnForceRedrawRequired;
        }

        #endregion

        #region Events

        /// <summary>
        ///     The generation complete.
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
        public void FormOnKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
        }

        /// <summary>
        ///     The perform render.
        /// </summary>
        public void PerformRender()
        {
            this.glControl1.Invalidate();
        }

        #endregion

        #region Methods

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

            this.mazeRenderer.Maze = this.realMaze;
            this.turtle = new Turtle(this.realMaze[0, 0], Maze.Direction.Down);
            this.mazeRenderer.Turtle = this.turtle;
            this.glControl1.Invalidate();
        }

        /// <summary>
        /// The form 1 form closing.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void FormOnFormClosing(object sender, FormClosingEventArgs e)
        {
            this.regenerationThread.Abort();
            this.mazeRenderer.ForceRedrawRequired -= this.MazeRendererOnForceRedrawRequired;
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
        private void FormOnLoad(object sender, EventArgs e)
        {
            this.GenerationComplete += this.Form1GenerationComplete;
            this.GenerateMaze(Settings.Default.MazeSize);

            this.glControl1.MakeCurrent();

            // trigger a resize event
            this.FormOnResize(sender, e);
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
        private void FormOnResize(object sender, EventArgs e)
        {
            GL.Viewport(this.glControl1.Size);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            Size s = this.glControl1.Size;

            if (s.Width > s.Height)
            {
                GL.Ortho(-s.Width / (double)s.Height, s.Width / (double)s.Height, -1, 1, -1, 1);
            }
            else
            {
                GL.Ortho(-1, 1, -s.Height / (double)s.Width, s.Height / (double)s.Width, -1, 1);
            }

            GL.MatrixMode(MatrixMode.Modelview);
        }

        /// <summary>
        /// The generate maze.
        /// </summary>
        /// <param name="resolution">
        /// The resolution.
        /// </param>
        private void GenerateMaze(int resolution)
        {
            Thread.Sleep(100);

            int tsd = new[] { this.glControl1.Width, this.glControl1.Height }.Min() / resolution;

            this.regenerationThread = new Thread(this.RegenerationThreadDoWork) { Priority = ThreadPriority.Lowest };
            this.regenerationThread.Start(tsd);
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

            this.glControl1.SwapBuffers();
        }

        /// <summary>
        /// The maze renderer on force redraw required.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void MazeRendererOnForceRedrawRequired(object sender, EventArgs e)
        {
            this.glControl1.Invalidate();
        }

        /// <summary>
        /// The regeneration thread_ do work.
        /// </summary>
        /// <param name="startData">
        /// The start data.
        /// </param>
        private void RegenerationThreadDoWork(object startData)
        {
            var data = (int)startData;

            var maze = new Maze(data, data);

            lock (this.mazeLock)
            {
                this.realMaze = maze;
            }

            this.GenerationComplete(this, new EventArgs());
        }

        /// <summary>
        /// The run button click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void RunButtonClick(object sender, EventArgs e)
        {
            LuaTable luaInterface = LuaRuntime.CreateGlobalEnviroment();

            luaInterface.Register(
                "turnLeft",
                delegate
                    {
                        this.turtle.TurnLeft();
                        return null;
                    });            
            
            luaInterface.Register(
                "turnRight",
                delegate
                    {
                        this.turtle.TurnRight();
                        return null;
                    });            
            
            luaInterface.Register(
                "goForward",
                delegate
                    {
                        this.turtle.GoForward();
                        return null;
                    });

            LuaRuntime.Run(this.scintilla1.Text, luaInterface);

            this.glControl1.Invalidate();
        }

        #endregion
    }
}