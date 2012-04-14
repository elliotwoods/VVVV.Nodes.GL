using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace VVVV.Nodes.OpenGL
{
	class ContextRegister
	{
		public class ContextAsset
		{
			public ContextAsset(IWindowInfo Window)
			{
				this.Window = Window;
			}

			public IWindowInfo Window;
		}

		public static Dictionary<IGraphicsContext, ContextAsset> Contexts = new Dictionary<IGraphicsContext,ContextAsset>();

		public static void Add(GLControl control)
		{
			IGraphicsContext context = control.Context;
			IWindowInfo window = control.WindowInfo;

			Contexts.Add(context, new ContextAsset(window));
		}

		public static void Add(GameWindow gameWindow)
		{
			IGraphicsContext context = gameWindow.Context;
			IWindowInfo window = gameWindow.WindowInfo;

			Contexts.Add(context, new ContextAsset(window));
		}

		public static void Remove(GLControl control)
		{
			IGraphicsContext context = control.Context;
			Remove(context);
		}

		public static void Remove(GameWindow gameWindow)
		{
			IGraphicsContext context = gameWindow.Context;
			Remove(context);
		}

		public static void Remove(IGraphicsContext context)
		{
			if (context != null)
				Contexts.Remove(context);
		}

		public static bool BindContext(IGraphicsContext context)
		{
			if (!context.IsDisposed)
			{
				context.MakeCurrent(Contexts[context].Window);
				return true;
			} else 	{
				return false;
			}
		}
	}
}
