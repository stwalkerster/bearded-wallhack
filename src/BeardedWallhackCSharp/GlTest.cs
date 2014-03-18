// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlTest.cs" company="Simon Walker">
//   Simon Walker
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace BeardedWallhackCSharp
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    using OpenTK.Graphics.OpenGL;

    /// <summary>
    ///     The OpenGL test.
    /// </summary>
    public partial class GlTest : Form
    {
        #region Fields

        /// <summary>
        ///     The loaded.
        /// </summary>
        private bool loaded;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initialises a new instance of the <see cref="GlTest" /> class.
        /// </summary>
        public GlTest()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on load.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.loaded = true;

            this.GlControl1Resize(this, EventArgs.Empty);

            this.glControl1.Invalidate();
        }

        /// <summary>
        /// The paint.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void GlControl1Paint(object sender, PaintEventArgs e)
        {
            if (!this.loaded)
            {
                return;
            }

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.ClearColor(Color.BlueViolet);

            this.glControl1.SwapBuffers();
        }

        /// <summary>
        /// The resize.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void GlControl1Resize(object sender, EventArgs e)
        {
            if (this.glControl1.ClientSize.Height == 0)
            {
                this.glControl1.ClientSize = new Size(this.glControl1.ClientSize.Width, 1);
            }

            GL.Viewport(0, 0, this.glControl1.ClientSize.Width, this.glControl1.ClientSize.Height);
        }

        #endregion
    }
}