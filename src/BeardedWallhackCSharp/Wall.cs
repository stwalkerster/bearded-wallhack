// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Wall.cs" company="Simon Walker">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace BeardedWallhackCSharp
{
    using System;

    /// <summary>
    ///     The wall.
    /// </summary>
    [Serializable]
    public class Wall
    {
        #region Fields

        /// <summary>
        /// The present.
        /// </summary>
        private bool present = true;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Wall"/> class.
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
            this.A = a;
            this.B = b;
            this.present = present;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the a.
        /// </summary>
        public Block A { get; set; }

        /// <summary>
        /// Gets or sets the b.
        /// </summary>
        public Block B { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether present.
        /// </summary>
        public bool Present
        {
            get
            {
                return this.present;
            }

            set
            {
                this.present = value;
            }
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
        public Block GetOpposite(Block x)
        {
            return x == this.A ? this.B : this.A;
        }

        #endregion
    }
}