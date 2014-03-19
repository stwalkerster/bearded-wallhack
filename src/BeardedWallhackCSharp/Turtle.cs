// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MazePosition.cs" company="Simon Walker">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace BeardedWallhackCSharp
{
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
            
        }

        public void TurnLeft()
        {
          //  this.Direction = (this.Direction + 1);
        }

        public void TurnRight()
        {
         //   this.Direction = (this.Direction - 1);
        }
    }
}