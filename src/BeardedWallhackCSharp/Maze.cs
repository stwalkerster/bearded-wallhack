// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Maze.cs" company="Simon Walker">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BeebMaze
{
    using System;
    using System.Collections.Generic;

    using BeebMaze.Properties;

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
        /// Initializes a new instance of the <see cref="Maze"/> class.
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

            this.generate();
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
            LEFT = 3, 

            /// <summary>
            /// The right.
            /// </summary>
            RIGHT = 1, 

            /// <summary>
            /// The down.
            /// </summary>
            DOWN = 2, 

            /// <summary>
            /// The up.
            /// </summary>
            UP = 0, 
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
        public Block currentBlock { get; private set; }

        /// <summary>
        /// Gets or sets the exit block.
        /// </summary>
        public Block exitBlock { get; set; }

        /// <summary>
        /// Gets a value indicating whether is solved.
        /// </summary>
        public bool isSolved { get; private set; }

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
        /// The <see cref="Block[,]"/>.
        /// </returns>
        [Obsolete]
        public Block[,] exportMaze()
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
        public void importMaze(Block[,] maze)
        {
            this.mazeBlocks = maze;
        }

        /// <summary>
        /// The move.
        /// </summary>
        /// <param name="direction">
        /// The direction.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        public void move(Direction direction)
        {
            switch (direction)
            {
                case Direction.LEFT:
                    if (this.currentBlock.ExitLeft)
                    {
                        this.currentBlock.CurrentState = Block.State.Visited;
                        Block next = this.currentBlock.WallLeft.getOpposite(this.currentBlock);
                        next.CurrentState = Block.State.Current;
                        this.currentBlock = next;
                    }

                    break;
                case Direction.RIGHT:
                    if (this.currentBlock.ExitRight)
                    {
                        this.currentBlock.CurrentState = Block.State.Visited;
                        Block next = this.currentBlock.WallRight.getOpposite(this.currentBlock);
                        next.CurrentState = Block.State.Current;
                        this.currentBlock = next;
                    }

                    break;
                case Direction.DOWN:
                    if (this.currentBlock.ExitBottom)
                    {
                        this.currentBlock.CurrentState = Block.State.Visited;
                        Block next = this.currentBlock.WallBottom.getOpposite(this.currentBlock);
                        next.CurrentState = Block.State.Current;
                        this.currentBlock = next;
                    }

                    break;
                case Direction.UP:
                    if (this.currentBlock.ExitTop)
                    {
                        this.currentBlock.CurrentState = Block.State.Visited;
                        Block next = this.currentBlock.WallTop.getOpposite(this.currentBlock);
                        next.CurrentState = Block.State.Current;
                        this.currentBlock = next;
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
        public void solve()
        {
            bool changed;

            this.isSolved = true;

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
        private void generate()
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

            this.exitBlock = this.mazeBlocks[this.Width - 1, this.Height - 1];
            this.exitBlock.IsExit = true;
            this.exitBlock.CurrentState = Block.State.Exit;

            this.currentBlock = startBlock;
            this.currentBlock.CurrentState = Block.State.Current;
        }

        #endregion
    }
}