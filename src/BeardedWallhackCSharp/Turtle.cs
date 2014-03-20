// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Turtle.cs" company="Simon Walker">
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
        ///     Gets or sets the block.
        /// </summary>
        public Block Block { get; set; }

        /// <summary>
        ///     Gets the direction.
        /// </summary>
        public Maze.Direction Direction { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The can see wall.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CanSeeWall()
        {
            var wall = this.GetWall(this.Direction);
            return wall == null || wall.Present;
        }

        /// <summary>
        /// The can see wall.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CanSeeWallOnLeft()
        {
            var wall = this.GetWall((Maze.Direction)(((int)this.Direction - 1 + 4) % 4));
            return wall == null || wall.Present;
        }

        /// <summary>
        /// The can see wall.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CanSeeWallOnRight()
        {
            var wall = this.GetWall((Maze.Direction)(((int)this.Direction + 1) % 4));
            return wall == null || wall.Present;
        }

        /// <summary>
        /// The go forward.
        /// </summary>
        /// <exception cref="TurtleException">
        /// if I hit a wall
        /// </exception>
        public void GoForward()
        {
            Wall wall = this.GetWall(this.Direction);

            if (wall != null && !wall.Present)
            {
                this.Block = wall.GetOpposite(this.Block);
                this.Block.CurrentState = Block.State.Visited;

                if (this.Block.IsExit)
                {
                    throw new TurtleException("NOM NOM NOM!!");
                }
            }
            else
            {
                throw new TurtleException("I hit a wall!");
            }
        }

        /// <summary>
        /// The turn left.
        /// </summary>
        public void TurnLeft()
        {
            this.Direction = (Maze.Direction)(((int)this.Direction - 1 + 4) % 4);
        }

        /// <summary>
        /// The turn right.
        /// </summary>
        public void TurnRight()
        {
            this.Direction = (Maze.Direction)(((int)this.Direction + 1) % 4);
        }

        #endregion

        #region Methods

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
                    return this.Block.WallBottom;
                case Maze.Direction.Left:
                    return this.Block.WallLeft;
                case Maze.Direction.Right:
                    return this.Block.WallRight;
                case Maze.Direction.Up:
                    return this.Block.WallTop;
            }

            throw new ArgumentOutOfRangeException();
        }

        #endregion
    }
}