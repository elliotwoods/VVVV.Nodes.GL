using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VMath;
using VVVV.Utils.VColor;
using OpenTK.Graphics.OpenGL;
using VVVV.Nodes.OpenGL.Utilities;
using OpenTK;
using VVVV.Core.Logging;
using System.ComponentModel.Composition;
using OpenTK.Graphics;
using OpenTK.Input;
using System.Threading;

namespace VVVV.Nodes.OpenGL
{
	class RendererWindow : GameWindow, IDisposable
	{
		private IGraphicsContext FContext;

		public RendererWindow()
			: base(800, 600, GraphicsMode.Default, "Renderer (OpenGL)")
		{
			VSync = VSyncMode.Off;
			FRunning = true;

			this.RenderFrame += new EventHandler<FrameEventArgs>(RendererWindow_RenderFrame);
			this.Closed += new EventHandler<EventArgs>(RendererWindow_Closed);
		}

		void RendererWindow_RenderFrame(object sender, FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();

			GL.Begin(BeginMode.Triangles);

			GL.Color3(1.0f, 1.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, 4.0f);
			GL.Color3(1.0f, 0.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 4.0f);
			GL.Color3(0.2f, 0.9f, 1.0f); GL.Vertex3(0.0f, 1.0f, 4.0f);

			GL.End();

			SwapBuffers();
		}

		void RendererWindow_Closed(object sender, EventArgs e)
		{
			ContextRegister.Remove(FContext);
		}

		bool FRunning;

		/// <summary>Load resources here.</summary>
		/// <param name="e">Not used.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
			GL.Enable(EnableCap.DepthTest);
			
			FContext = this.Context;
			ContextRegister.Add(this);

			ContextRegister.BindContext(FContext);
		}

		/// <summary>
		/// Called when your window is resized. Set your viewport here. It is also
		/// a good place to set up your projection matrix (which probably changes
		/// along when the aspect ratio of your window).
		/// </summary>
		/// <param name="e">Not used.</param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			ContextRegister.BindContext(this.Context);
			GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
		}

		/// <summary>
		/// Called when it is time to setup the next frame. Add you game logic here.
		/// </summary>
		/// <param name="e">Contains timing information for framerate independent logic.</param>
		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame(e);
			base.Title = base.RenderFrequency.ToString();
			if (!FRunning)
				Exit();
		}
	}

	#region PluginInfo
	[PluginInfo(Name = "Renderer", Category = "OpenGL", Version = "Fast", Help = "Render external to VVVV window", Tags = "", AutoEvaluate=true)]
	#endregion PluginInfo
	public class RendererFastNode : IPluginEvaluate, IDisposable
	{
		#region fields & pins
		[Input("Input")]
		ISpread<ILayer> FPinInLayer;

		[Input("View")]
		ISpread<Matrix4x4> FPinInView;

		[Input("Projection")]
		ISpread<Matrix4x4> FPinInProjection;

		[Import()]
		ILogger FLogger;

		Thread FThread = null;
		RendererWindow FWindow = null;
		#endregion fields & pins

		public void Evaluate(int SpreadMax)
		{
			if (FThread == null)
			{
				FThread = new Thread(Run);
				FThread.Start();
			}
		}

		void Run()
		{
			FWindow = new RendererWindow();
			FWindow.Run(30.0d);
		}

		public void Dispose()
		{
			if (FWindow != null)
				FWindow.Exit();
		}
	}
}
