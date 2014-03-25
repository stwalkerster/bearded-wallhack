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
        #region Fields

        /// <summary>
        ///     The fuel.
        /// </summary>
        private int fuel = 100000;

        #endregion

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
        ///     Gets or sets the direction.
        /// </summary>
        public Maze.Direction Direction { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The can see wall.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool CanSeeWall()
        {
            Wall wall = this.GetWall(this.Direction);
            return wall == null || wall.Present;
        }

        /// <summary>
        ///     The go forward.
        /// </summary>
        /// <exception cref="TurtleException">
        ///     if I hit a wall
        /// </exception>
        public void GoForward()
        {
            Wall wall = this.GetWall(this.Direction);

            if (wall != null && !wall.Present)
            {
                if (this.fuel <= 0)
                {
                    throw new TurtleException("I'm too tired to go any further!");
                }

                this.Block = wall.GetOpposite(this.Block);
                this.Block.CurrentState = Block.State.Visited;

                if (this.Block.IsExit)
                {
                    throw new TurtleException("NOM NOM NOM!!", true);
                }

                this.fuel--;
            }
            else
            {
                Console.WriteLine("Position: {0}, {1}", this.Block.PositionX, this.Block.PositionY);
                throw new TurtleException("I hit a wall!");
            }
        }

        /// <summary>
        ///     The turn left.
        /// </summary>
        public void TurnLeft()
        {
            if (this.fuel <= 0)
            {
                throw new TurtleException("I'm too tired to go any further!");
            }

            this.fuel--;

            this.Direction = (Maze.Direction)(((int)this.Direction - 1 + 4) % 4);
        }

        /// <summary>
        ///     The turn right.
        /// </summary>
        public void TurnRight()
        {
            if (this.fuel <= 0)
            {
                throw new TurtleException("I'm too tired to go any further!");
            }

            this.fuel--;

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