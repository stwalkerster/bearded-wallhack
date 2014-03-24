// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Block.cs" company="Simon Walker">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace BeardedWallhackCSharp
{
    using System;

    /// <summary>
    ///     The block.
    /// </summary>
    [Serializable]
    public class Block
    {
        #region Fields

        /// <summary>
        ///     The _current state.
        /// </summary>
        private State currentState = State.Unvisited;

        #endregion

        #region Enums

        /// <summary>
        ///     The state.
        /// </summary>
        public enum State
        {
            /// <summary>
            ///     The unvisited.
            /// </summary>
            Unvisited, 

            /// <summary>
            ///     The current.
            /// </summary>
            Current, 

            /// <summary>
            ///     The visited.
            /// </summary>
            Visited
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the current state.
        /// </summary>
        public State CurrentState
        {
            get
            {
                return this.currentState;
            }

            set
            {
                this.currentState = value;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether exit bottom.
        /// </summary>
        public bool ExitBottom
        {
            get
            {
                if (this.WallBottom == null)
                {
                    return false;
                }

                return !this.WallBottom.Present;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether exit left.
        /// </summary>
        public bool ExitLeft
        {
            get
            {
                if (this.WallLeft == null)
                {
                    return false;
                }

                return !this.WallLeft.Present;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether exit right.
        /// </summary>
        public bool ExitRight
        {
            get
            {
                if (this.WallRight == null)
                {
                    return false;
                }

                return !this.WallRight.Present;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether exit top.
        /// </summary>
        public bool ExitTop
        {
            get
            {
                if (this.WallTop == null)
                {
                    return false;
                }

                return !this.WallTop.Present;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether in maze.
        /// </summary>
        public bool InMaze { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this block is the maze exit.
        /// </summary>
        public bool IsExit { get; set; }

        /// <summary>
        ///     Gets or sets the position x.
        /// </summary>
        public int PositionX { get; set; }

        /// <summary>
        ///     Gets or sets the position y.
        /// </summary>
        public int PositionY { get; set; }

        /// <summary>
        ///     Gets or sets the wall bottom.
        /// </summary>
        public Wall WallBottom { get; set; }

        /// <summary>
        ///     Gets or sets the wall left.
        /// </summary>
        public Wall WallLeft { get; set; }

        /// <summary>
        ///     Gets or sets the wall right.
        /// </summary>
        public Wall WallRight { get; set; }

        /// <summary>
        ///     Gets or sets the wall top.
        /// </summary>
        public Wall WallTop { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The count effective walls.
        /// </summary>
        /// <returns>
        ///     The <see cref="int" />.
        /// </returns>
        public int CountEffectiveWalls()
        {
            int count = 0;
            if (this.WallTop != null)
            {
                if (this.WallTop.Present)
                {
                    count++;
                }
                else if (!this.WallTop.GetOpposite(this).InMaze)
                {
                    count++;
                }
            }
            else
            {
                count++;
            }

            if (this.WallBottom != null)
            {
                if (this.WallBottom.Present)
                {
                    count++;
                }
                else if (!this.WallBottom.GetOpposite(this).InMaze)
                {
                    count++;
                }
            }
            else
            {
                count++;
            }

            if (this.WallLeft != null)
            {
                if (this.WallLeft.Present)
                {
                    count++;
                }
                else if (!this.WallLeft.GetOpposite(this).InMaze)
                {
                    count++;
                }
            }
            else
            {
                count++;
            }

            if (this.WallRight != null)
            {
                if (this.WallRight.Present)
                {
                    count++;
                }
                else if (!this.WallRight.GetOpposite(this).InMaze)
                {
                    count++;
                }
            }
            else
            {
                count++;
            }

            return count;
        }

        #endregion
    }
}