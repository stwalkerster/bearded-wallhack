// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Maze.cs" company="Simon Walker">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace BeardedWallhackCSharp
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     The maze.
    /// </summary>
    [Serializable]
    public class Maze
    {
        #region Fields

        /// <summary>
        ///     The maze blocks.
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
            this.Width = width;
            this.Height = height;

            this.Generate();
        }

        /// <summary>
        ///     Initialises a new instance of the <see cref="Maze" /> class.
        /// </summary>
        public Maze()
        {
        }

        #endregion

        #region Enums

        /// <summary>
        ///     The direction.
        /// </summary>
        public enum Direction
        {
            /// <summary>
            ///     The left.
            /// </summary>
            Left = 3, 

            /// <summary>
            ///     The right.
            /// </summary>
            Right = 1, 

            /// <summary>
            ///     The down.
            /// </summary>
            Down = 2, 

            /// <summary>
            ///     The up.
            /// </summary>
            Up = 0, 
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the current block.
        /// </summary>
        public Block CurrentBlock { get; private set; }

        /// <summary>
        ///     Gets or sets the exit block.
        /// </summary>
        public Block ExitBlock { get; set; }

        /// <summary>
        ///     Gets the height.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        ///     Gets the maze blocks.
        /// </summary>
        public Block[,] MazeBlocks
        {
            get
            {
                return this.mazeBlocks;
            }
        }

        /// <summary>
        ///     Gets the width.
        /// </summary>
        public int Width { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        ///     The generate.
        /// </summary>
        private void Generate()
        {
            this.mazeBlocks = new Block[this.Width, this.Height];

            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    this.MazeBlocks[x, y] = new Block();

                    this.MazeBlocks[x, y].PositionX = x;
                    this.MazeBlocks[x, y].PositionY = y;

                    Wall w;

                    if (x != 0)
                    {
                        w = new Wall(this.MazeBlocks[x, y], this.MazeBlocks[x - 1, y], true);
                        this.MazeBlocks[x, y].WallLeft = w;
                        this.MazeBlocks[x - 1, y].WallRight = w;
                    }

                    if (y != 0)
                    {
                        w = new Wall(this.MazeBlocks[x, y], this.MazeBlocks[x, y - 1], true);
                        this.MazeBlocks[x, y].WallTop = w;
                        this.MazeBlocks[x, y - 1].WallBottom = w;
                    }
                }
            }

            var walls = new List<KeyValuePair<Wall, Block>>();
            Block startBlock = this.MazeBlocks[0, 0];

            startBlock.CurrentState = Block.State.Visited;
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

                Block newBlock = walls[wallId].Key.GetOpposite(walls[wallId].Value);

                if (!newBlock.InMaze)
                {
                    // Make the wall a passage and mark the cell on the opposite side as part of the maze.
                    walls[wallId].Key.Present = false;
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

            this.ExitBlock = this.MazeBlocks[this.Width - 1, this.Height - 1];
            this.ExitBlock.IsExit = true;

            this.CurrentBlock = startBlock;
        }

        #endregion
    }
}