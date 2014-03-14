// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Maze.cs" company="Simon Walker">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BeardedWallhackCSharp
{
    using System;
    using System.Collections.Generic;

    using BeardedWallhackCSharp.Properties;

    /// <summary>
    /// The maze.
    /// </summary>
    public class Maze
    {
        #region Fields

        /// <summary>
        /// The maze blocks.
        /// </summary>
        private Block[,] mazeBlocks;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Maze"/> class.
        /// </summary>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        public Maze(int width, int height)
        {
            // swapped to fix something 
            // TODO: locate cause of need for this
            this.Height = width;
            this.Width = height;

            this.Generate();
        }

        #endregion

        #region Enums

        /// <summary>
        /// The direction.
        /// </summary>
        public enum Direction
        {
            /// <summary>
            /// The left.
            /// </summary>
            Left = 3, 

            /// <summary>
            /// The right.
            /// </summary>
            Right = 1, 

            /// <summary>
            /// The down.
            /// </summary>
            Down = 2, 

            /// <summary>
            /// The up.
            /// </summary>
            Up = 0, 
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the height.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the width.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the current block.
        /// </summary>
        public Block CurrentBlock { get; private set; }

        /// <summary>
        /// Gets or sets the exit block.
        /// </summary>
        public Block ExitBlock { get; set; }

        /// <summary>
        /// Gets a value indicating whether is solved.
        /// </summary>
        public bool IsSolved { get; private set; }

        #endregion

        #region Public Indexers

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <returns>
        /// The <see cref="Block"/>.
        /// </returns>
        public Block this[int x, int y]
        {
            get
            {
                return this.mazeBlocks[x, y];
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The export maze.
        /// </summary>
        /// <returns>
        /// The <see cref="Block"/>.
        /// </returns>
        [Obsolete]
        public Block[,] ExportMaze()
        {
            return this.mazeBlocks;
        }

        /// <summary>
        /// The import maze.
        /// </summary>
        /// <param name="maze">
        /// The maze.
        /// </param>
        [Obsolete]
        public void ImportMaze(Block[,] maze)
        {
            this.mazeBlocks = maze;
        }

        /// <summary>
        /// The move.
        /// </summary>
        /// <param name="direction">
        /// The direction.
        /// </param>
        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    if (this.CurrentBlock.ExitLeft)
                    {
                        this.CurrentBlock.CurrentState = Block.State.Visited;
                        Block next = this.CurrentBlock.WallLeft.getOpposite(this.CurrentBlock);
                        next.CurrentState = Block.State.Current;
                        this.CurrentBlock = next;
                    }

                    break;
                case Direction.Right:
                    if (this.CurrentBlock.ExitRight)
                    {
                        this.CurrentBlock.CurrentState = Block.State.Visited;
                        Block next = this.CurrentBlock.WallRight.getOpposite(this.CurrentBlock);
                        next.CurrentState = Block.State.Current;
                        this.CurrentBlock = next;
                    }

                    break;
                case Direction.Down:
                    if (this.CurrentBlock.ExitBottom)
                    {
                        this.CurrentBlock.CurrentState = Block.State.Visited;
                        Block next = this.CurrentBlock.WallBottom.getOpposite(this.CurrentBlock);
                        next.CurrentState = Block.State.Current;
                        this.CurrentBlock = next;
                    }

                    break;
                case Direction.Up:
                    if (this.CurrentBlock.ExitTop)
                    {
                        this.CurrentBlock.CurrentState = Block.State.Visited;
                        Block next = this.CurrentBlock.WallTop.getOpposite(this.CurrentBlock);
                        next.CurrentState = Block.State.Current;
                        this.CurrentBlock = next;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }

            Program.app.performRender();
        }

        /// <summary>
        /// The solve.
        /// </summary>
        public void Solve()
        {
            bool changed;

            this.IsSolved = true;

            do
            {
                changed = false;
                for (int x = 0; x < this.Width; x++)
                {
                    for (int y = 0; y < this.Height; y++)
                    {
                        Block block = this.mazeBlocks[x, y];

                        if (!block.InMaze)
                        {
                            continue;
                        }

                        if (block.CountEffectiveWalls() != 3)
                        {
                            continue;
                        }

                        block.InMaze = false;
                        block.Hidden = false;
                        changed = true;
                    }
                }
            }
            while (changed);

            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    if (this.mazeBlocks[x, y].InMaze)
                    {
                        this.mazeBlocks[x, y].CurrentState = Block.State.Current;
                    }
                }
            }

            Program.app.performRender();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The generate.
        /// </summary>
        private void Generate()
        {
            this.mazeBlocks = new Block[this.Width, this.Height];

            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    this.mazeBlocks[x, y] = new Block();
                    Wall w;

                    if (x != 0)
                    {
                        w = new Wall(this.mazeBlocks[x, y], this.mazeBlocks[x - 1, y], true);
                        this.mazeBlocks[x, y].WallLeft = w;
                        this.mazeBlocks[x - 1, y].WallRight = w;
                    }

                    if (y != 0)
                    {
                        w = new Wall(this.mazeBlocks[x, y], this.mazeBlocks[x, y - 1], true);
                        this.mazeBlocks[x, y].WallTop = w;
                        this.mazeBlocks[x, y - 1].WallBottom = w;
                    }

                    this.mazeBlocks[x, y].Hidden = !Settings.Default.RevealMaze;
                }
            }

            var walls = new List<KeyValuePair<Wall, Block>>();
            Block startBlock = this.mazeBlocks[0, 0];

            startBlock.IsExit = true;
            startBlock.InMaze = true;

            if (startBlock.WallTop != null)
            {
                walls.Add(new KeyValuePair<Wall, Block>(startBlock.WallTop, startBlock));
            }

            if (startBlock.WallRight != null)
            {
                walls.Add(new KeyValuePair<Wall, Block>(startBlock.WallRight, startBlock));
            }

            if (startBlock.WallBottom != null)
            {
                walls.Add(new KeyValuePair<Wall, Block>(startBlock.WallBottom, startBlock));
            }

            if (startBlock.WallLeft != null)
            {
                walls.Add(new KeyValuePair<Wall, Block>(startBlock.WallLeft, startBlock));
            }

            var rnd = new Random();

            // While there are walls in the list:
            int maxWalls = 400;
            while (walls.Count > 0)
            {
                maxWalls = walls.Count > maxWalls ? walls.Count : maxWalls;

                // Pick a random wall from the list. If the cell on the opposite side isn't in the maze yet:
                int wallId = rnd.Next(0, walls.Count);

                Block newBlock = walls[wallId].Key.getOpposite(walls[wallId].Value);

                if (!newBlock.InMaze)
                {
                    // Make the wall a passage and mark the cell on the opposite side as part of the maze.
                    walls[wallId].Key.present = false;
                    newBlock.InMaze = true;

                    // Add the neighboring walls of the cell to the wall list.
                    if (newBlock.WallLeft != null)
                    {
                        walls.Add(new KeyValuePair<Wall, Block>(newBlock.WallLeft, newBlock));
                    }

                    if (newBlock.WallRight != null)
                    {
                        walls.Add(new KeyValuePair<Wall, Block>(newBlock.WallRight, newBlock));
                    }

                    if (newBlock.WallTop != null)
                    {
                        walls.Add(new KeyValuePair<Wall, Block>(newBlock.WallTop, newBlock));
                    }

                    if (newBlock.WallBottom != null)
                    {
                        walls.Add(new KeyValuePair<Wall, Block>(newBlock.WallBottom, newBlock));
                    }
                }

                walls.RemoveAt(wallId);
            }

            this.ExitBlock = this.mazeBlocks[this.Width - 1, this.Height - 1];
            this.ExitBlock.IsExit = true;
            this.ExitBlock.CurrentState = Block.State.Exit;

            this.CurrentBlock = startBlock;
            this.CurrentBlock.CurrentState = Block.State.Current;
        }

        #endregion
    }
}