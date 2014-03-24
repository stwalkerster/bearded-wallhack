// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace BeardedWallhackCSharp
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading;
    using System.Windows.Forms;

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

        /// <summary>
        /// The turtle.
        /// </summary>
        private Turtle turtle;

        /// <summary>
        /// The lua table.
        /// </summary>
        private LuaTable luaTable;

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

        /// <summary>
        ///     The _real maze.
        /// </summary>
        public Maze RealMaze
        {
            get
            {
                return this.realMaze;
            }

            set
            {
                this.realMaze = value;
                if (this.mazeRenderer != null)
                {
                    this.mazeRenderer.Maze = value;
                }
            }
        }

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

            this.mazeRenderer.Maze = this.RealMaze;
            this.turtle = new Turtle(this.RealMaze.MazeBlocks[0, 0], Maze.Direction.Down);
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

            this.glControl1.MakeCurrent();

            // trigger a resize event
            this.FormOnResize(sender, e);

            this.CurrentLevel = 1;
            this.LoadLevel();
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
        private void GenerateMaze(int resolution)
        {
            this.regenerationThread = new Thread(this.RegenerationThreadDoWork) { Priority = ThreadPriority.Lowest };
            this.regenerationThread.Start(resolution);
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
        /// The save maze data.
        /// </summary>
        private void SaveMazeData()
        {
            string mazeData;

            lock (this.mazeLock)
            {
                var bf = new BinaryFormatter();
                var s = new MemoryStream();
                bf.Serialize(s, this.realMaze);
                s.Position = 0;
                var bytes = new byte[s.Length];
                s.Read(bytes, 0, bytes.Length);
                
                mazeData = Convert.ToBase64String(bytes);
            }

            Clipboard.SetText(mazeData);
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
                this.RealMaze = maze;
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
            this.ResetMazeState();

            try
            {
                LuaRuntime.Run(this.codeEditor.Text, this.GetLuaTable());
            }
            catch (TurtleException ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.glControl1.Invalidate();
        }

        /// <summary>
        /// The get lua table.
        /// </summary>
        /// <returns>
        /// The <see cref="LuaTable"/>.
        /// </returns>
        private LuaTable GetLuaTable()
        {
            if (this.luaTable != null)
            {
                return this.luaTable;
            }

            this.luaTable = LuaRuntime.CreateGlobalEnviroment();

            this.luaTable.Register(
                "turnLeft",
                delegate
                    {
                        this.turtle.TurnLeft();
                        return null;
                    });

            this.luaTable.Register(
                "turnRight",
                delegate
                    {
                        this.turtle.TurnRight();
                        return null;
                    });

            this.luaTable.Register(
                "goForward",
                delegate
                    {
                        this.turtle.GoForward();
                        return null;
                    });

            this.luaTable.Register("canSeeWall", delegate { return ObjectToLua.ToLuaValue(this.turtle.CanSeeWall()); });
            return this.luaTable;
        }

        /// <summary>
        /// The reset maze state.
        /// </summary>
        private void ResetMazeState()
        {
            this.turtle.Direction = Maze.Direction.Down;
            
            this.turtle.Block = this.RealMaze.MazeBlocks[0, 0];
            foreach (var b in this.RealMaze.MazeBlocks)
            {
                b.CurrentState = Block.State.Unvisited;
            }

            this.turtle.Block.CurrentState = Block.State.Visited;
        }

        #endregion

        /// <summary>
        /// The tool strip button 1 click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ToolStripButton1Click(object sender, EventArgs e)
        {
            this.SaveMazeData();
        }

        /// <summary>
        /// The load level data
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ToolStripButton2Click(object sender, EventArgs e)
        {
            this.LoadLevel();
            this.CurrentLevel++;
        }

        private void LoadLevel()
        {
            var manifestResourceStream =
                Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("BeardedWallhackCSharp.LevelData.Level" + this.CurrentLevel + ".dat");

            if (manifestResourceStream != null)
            {
                var mazeData = new StreamReader(manifestResourceStream).ReadToEnd();

                var bytes = Convert.FromBase64String(mazeData);
                var bf = new BinaryFormatter();
                var s = new MemoryStream(bytes) { Position = 0 };
                var maze = (Maze)bf.Deserialize(s);

                lock (this.mazeLock)
                {
                    this.RealMaze = maze;
                }

                this.GenerationComplete(this, EventArgs.Empty);
            }
            else
            {
                this.GenerateMaze(this.CurrentLevel);
            }
        }

        /// <summary>
        /// Gets or sets the current level.
        /// </summary>
        public int CurrentLevel { get; set; }
    }
}