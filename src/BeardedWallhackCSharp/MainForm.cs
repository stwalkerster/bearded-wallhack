// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="Simon Walker">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace BeardedWallhackCSharp
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading;
    using System.Windows.Forms;

    using OpenTK.Graphics.OpenGL;

    using SharpLua;
    using SharpLua.LuaTypes;
    using SharpLua.Parser;

    /// <summary>
    ///     The main form.
    /// </summary>
    public partial class MainForm : Form
    {
        #region Fields

        /// <summary>
        ///     The level text.
        /// </summary>
        private readonly IDictionary<int, string> levelText = new Dictionary<int, string>
                                                                  {
                                                                      {
                                                                          0,
                                                                          "How about having a go on your own?"
                                                                      },
                                                                      {
                                                                          1,
                                                                          "Hi there! I'm Tim the Turtle!\n\n"
                                                                          + "I'm a bit hungry, so maybe you could tell me how to get to some food?\n\n"
                                                                          + "Type instructions in the box below, and click 'Run'. I'll teach you step by step what instructions you can use.\n\n"
                                                                          + "Start by typing 'goForward()', which will make me go forward one square."
                                                                      },
                                                                      {
                                                                          2,
                                                                          "Thanks for that!\n\n"
                                                                          + "This next one is a bit more of a challenge - you can either repeat the goForward() instruction, or you can try using something called a loop.\n\n"
                                                                          + "I'll always stop when I find food, so you can just use a loop that continues forever. We're going to use a 'while' loop.\n\n"
                                                                          + "Start by using 'while true do', and finsh with 'end'. Anything you put between those two will be repeated.\n\n"
                                                                          + "(The 'true' bit is where we put something called a condition. If the condition equals true, the loop will be repeated another time. We'll use this later!)"
                                                                      },
                                                                      {
                                                                          3,
                                                                          "Oooh, looks like I'm going to have to go around some corners for this one!\n\n"
                                                                          + "You can use turnLeft() and turnRight() to make me point another way.\n\n"
                                                                          + "Try not to let me hit a wall, because that hurts!"
                                                                      },
                                                                      {
                                                                          4,
                                                                          "Yikes!\n\n"
                                                                          + "Looks like you need to use a few loops! I have one last command, canSeeWall(). This will equal true if there is a wall directly in front of me, that I would hit if I went forward. You may want to use this as the condition in a loop.\n\n"
                                                                          + "You can put a loop inside another loop if you want to!\n\n"
                                                                          + "You can create special boxes, or variables, to hold info too. Just use 'local x', and that will create a variable called x, which you can put stuff into by saying 'x = true' for example. You can also use 'if' statements to run commands if something is true. Start with 'if true then' and end with 'end' - just like the loops!\n\n"
                                                                          + "I've given you a hint to start with to make it a bit easier."
                                                                      },
                                                                      {
                                                                          5,
                                                                          "Hmm... this looks like we could be writing the same instructions again and again.\n\n"
                                                                          + "It's much better for me if you group a set of instructions together for me. You can then make your own instructions by creating groups of instructions. We call these functions.\n\n"
                                                                          + "You can create a function by writing 'function myFunction()', where the 'myFunction' bit is it's name. My four instructions [goForward(), turnLeft(), turnRight(), and canSeeWall()] are all functions that I already know!\n\n"
                                                                          + "Try writing a function to go from the top to the bottom, then another from the bottom to the top. You can use these to make your loop much smaller and understandable!"
                                                                      },
                                                                      {
                                                                          6,
                                                                          "These sequences of commands are called programs, and the rules about how it's build form what's known as a 'Programming Language'.\n\n"
                                                                          + "All of my instructions are written in a Programming Language called Lua. You can find out more online - ask your parents to help you find more information.\n\n"
                                                                          + "Real mazes have a lot of dead-ends, so you'll need to use some cunning tricks to get me to my food this time.\n\n"
                                                                          + "Try thinking up some small, neat solution to solve these mazes."
                                                                      }
                                                                  };

        /// <summary>
        /// The level code.
        /// </summary>
        private readonly IDictionary<int, string> levelCode = new Dictionary<int, string>
                                                                  {
                                                                      {
                                                                          4,
                                                                          "-- Hi! Lines starting with two dashes like this will be ignored.\n-- It's called a comment. We use them to help others with the instructions.\n\n-- Let's remember which line I'm on.\nlocal onTop = true\n\nwhile not canSeeWall() do\n\tgoForward()\nend\n\nif onTop then\n\t-- We're on the top, and have hit a wall.\n\t-- Might I suggest moving to the bottom?\n\t\n\tonTop = false\nelse\n\t-- this is the \"else\" part of an if.\n\t-- It is run when the condition is false.\n\t\n\t-- We're on the bottom, and have hit a wall.\n\t-- Might I suggest moving to the top here?\nend\n\n-- You'll need to finish this off."
                                                                      }
                                                                  };

        /// <summary>
        ///     The maze lock.
        /// </summary>
        private readonly object mazeLock = new object();

        /// <summary>
        ///     The maze renderer.
        /// </summary>
        private readonly IMazeRenderer mazeRenderer;

        /// <summary>
        ///     The lua table.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", 
            Justification = "This is correct.")]
        private LuaTable luaTable;

        /// <summary>
        ///     The real maze.
        /// </summary>
        private Maze realMaze;

        /// <summary>
        ///     The regeneration thread.
        /// </summary>
        private Thread regenerationThread;

        /// <summary>
        ///     The turtle.
        /// </summary>
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

        #region Public Properties

        /// <summary>
        ///     Gets or sets the current level.
        /// </summary>
        public int CurrentLevel { get; set; }

        /// <summary>
        ///     Gets or sets the real maze.
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
        /// <param name="resolution">
        /// The resolution.
        /// </param>
        private void GenerateMaze(int resolution)
        {
            this.regenerationThread = new Thread(this.RegenerationThreadDoWork) { Priority = ThreadPriority.Lowest };
            this.regenerationThread.Start(resolution);
        }

        /// <summary>
        ///     The get lua table.
        /// </summary>
        /// <returns>
        ///     The <see cref="LuaTable" />.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", 
            Justification = "Correct spelling.")]
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
        ///     The load level.
        /// </summary>
        private void LoadLevel()
        {
            Stream manifestResourceStream =
                Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("BeardedWallhackCSharp.LevelData.Level" + this.CurrentLevel + ".dat");

            if (manifestResourceStream != null)
            {
                string mazeData = new StreamReader(manifestResourceStream).ReadToEnd();

                byte[] bytes = Convert.FromBase64String(mazeData);
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

            string text;
            if (!this.levelText.TryGetValue(this.CurrentLevel, out text))
            {
                text = this.levelText[0];
            }

            this.label1.Text = text;

            string code;
            if (!this.levelCode.TryGetValue(this.CurrentLevel, out code))
            {
                code = string.Empty;
            }

            this.codeEditor.Text = code;
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
                this.RealMaze = maze;
            }

            this.GenerationComplete(this, new EventArgs());
        }

        /// <summary>
        ///     The reset maze state.
        /// </summary>
        private void ResetMazeState()
        {
            this.turtle.Direction = Maze.Direction.Down;

            this.turtle.Block = this.RealMaze.MazeBlocks[0, 0];
            foreach (Block b in this.RealMaze.MazeBlocks)
            {
                b.CurrentState = Block.State.Unvisited;
            }

            this.turtle.Block.CurrentState = Block.State.Visited;
            this.turtle.EnergyLeft = Turtle.StartEnergy;
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

                if (ex.Success)
                {
                    this.toolStripButton2.Enabled = true;
                }
            }
            catch (ParserException ex)
            {
                MessageBox.Show(string.Format("Whoops! Looks like I don't quite understand what you mean!\n\n{0}", ex.Message));
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Whoops! Something went wrong, but I'm not sure what.\n\n{0}", ex.Message));
            }

            this.glControl1.Invalidate();
        }

        /// <summary>
        ///     The save maze data.
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
            this.CurrentLevel++;
            this.LoadLevel();
        }

        #endregion
    }
}