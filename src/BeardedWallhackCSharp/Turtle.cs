// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MazePosition.cs" company="Simon Walker">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace BeardedWallhackCSharp
{
    using System;

    /// <summary>
    ///     The maze position.
    /// </summary>
    public class Turtle
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Turtle"/> class.
        /// </summary>
        /// <param name="block">
        /// The block.
        /// </param>
        /// <param name="direction">
        /// The direction.
        /// </param>
        public Turtle(Block block, Maze.Direction direction)
        {
            this.Block = block;
            this.Direction = direction;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the block.
        /// </summary>
        public Block Block { get; private set; }

        /// <summary>
        ///     Gets the direction.
        /// </summary>
        public Maze.Direction Direction { get; private set; }

        #endregion

        public void GoForward()
        {
            this.GoForward(1);
        }

        public void GoForward(int amount)
        {
            for (int i = amount - 1; i >= 0; i--)
            {
                var wall = this.GetWall(this.Direction);

                if (wall != null && !wall.Present)
                {
                    this.Block = wall.GetOpposite(this.Block);
                }
                else
                {
                    return;
                }
            }
        }

        public void TurnLeft()
        {
            this.Direction = (Maze.Direction)(((int)this.Direction + 1) % 4);
        }

        public void TurnRight()
        {
            this.Direction = (Maze.Direction)(((int)this.Direction - 1 + 4) % 4);
        }

        public bool CanSeeWall()
        {
            var wall = this.GetWall(this.Direction);

            return wall.Present;
        }

        /// <summary>
        /// The get wall.
        /// </summary>
        /// <param name="direction">
        /// The direction.
        /// </param>
        /// <returns>
        /// The <see cref="Wall"/>.
        /// </returns>
        private Wall GetWall(Maze.Direction direction)
        {
            switch (direction)
            {
                case Maze.Direction.Down:
                    return Block.WallBottom;
                case Maze.Direction.Left:
                    return Block.WallLeft;
                case Maze.Direction.Right:
                    return Block.WallRight;
                case Maze.Direction.Up:
                    return Block.WallTop;
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}