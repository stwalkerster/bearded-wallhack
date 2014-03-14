// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Wall.cs" company="Simon Walker">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BeardedWallhackCSharp
{
    /// <summary>
    /// The wall.
    /// </summary>
    public class Wall
    {
        #region Fields

        /// <summary>
        /// The a.
        /// </summary>
        public Block a;

        /// <summary>
        /// The b.
        /// </summary>
        public Block b;

        /// <summary>
        /// The present.
        /// </summary>
        public bool present = true;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Wall"/> class.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <param name="present">
        /// The present.
        /// </param>
        public Wall(Block a, Block b, bool present)
        {
            this.a = a;
            this.b = b;
            this.present = present;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get opposite.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <returns>
        /// The <see cref="Block"/>.
        /// </returns>
        public Block getOpposite(Block x)
        {
            return x == this.a ? this.b : this.a;
        }

        #endregion
    }
}