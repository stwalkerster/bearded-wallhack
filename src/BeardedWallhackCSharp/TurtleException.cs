﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TurtleException.cs" company="Simon Walker">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BeardedWallhackCSharp
{
    using System;

    /// <summary>
    /// The turtle exception.
    /// </summary>
    internal class TurtleException : Exception
    {
        public bool Success { get; set; }

        #region Constructors and Destructors

        /// <summary>
        /// Initialises a new instance of the <see cref="TurtleException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public TurtleException(string message, bool success = false)
            : base(message)
        {
            this.Success = success;
        }

        #endregion
    }
}