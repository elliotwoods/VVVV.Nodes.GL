using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using System.Drawing;
using System.Drawing.Imaging;

namespace VVVV.Nodes.OpenGL
{
	abstract class Texture : IDisposable
	{
		public class TextureAttributes
		{
			public TextureAttributes(int Width, int Height, int Depth, PixelInternalFormat InternalFormat, OpenTK.Graphics.OpenGL.PixelFormat PixelFormat, PixelType PixelType)
			{
				this.Width = Width;
				this.Height = Height;
				this.Depth = Depth;
				this.InternalFormat = InternalFormat;
				this.PixelFormat = PixelFormat;
				this.PixelType = PixelType;
			}

			public int Width, Height, Depth;
			public PixelInternalFormat InternalFormat;
			public OpenTK.Graphics.OpenGL.PixelFormat PixelFormat;
			public PixelType PixelType;
		}

		private class TextureInstance : IDisposable
		{
			private IGraphicsContext FContext;
			private TextureAttributes FAttributes;

			public TextureInstance(IGraphicsContext context, TextureAttributes attributes)
			{
				FContext = context;
				FAttributes = attributes;

				FAttributesUpdate = false;
				FContentsUpdate = true;

				if (context != GraphicsContext.CurrentContext)
					ContextRegister.BindContext(context);

				GL.Enable(EnableCap.Texture2D);
				GL.GenTextures(1, out this.FID);
				GL.BindTexture(TextureTarget.Texture2D, this.FID);

				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			}

			public void Dispose()
			{
				if (ContextRegister.BindContext(FContext))
					GL.DeleteTexture(this.FID);
			}

			private int FID;
			public int ID
			{
				get
				{
					return FID;
				}
			}

			/// <summary>
			/// NeedsAttributesUpdate results in this instance being destroyed
			/// and replaced by a new TextureInstance for this context
			/// </summary>
			private bool FAttributesUpdate;
			public bool NeedsAttributesUpdate
			{
				get
				{
					if (FAttributesUpdate)
					{
						FAttributesUpdate = false;
						return true;
					}
					else
						return false;
				}
			}

			private bool FContentsUpdate;
			public bool NeedsContentsUpdate
			{
				get
				{
					if (FContentsUpdate)
					{
						FContentsUpdate = false;
						return true;
					}
					else
						return false;
				}
			}
		}

		/// <summary>
		/// Dictionary of instances of this texture,
		/// e.g. instances of this texture on the different contexts, devices
		/// </summary>
		private Dictionary<IGraphicsContext, TextureInstance> FTextures = new Dictionary<IGraphicsContext,TextureInstance>();

		protected TextureAttributes FAttributes;
		
		public bool Bind()
		{
			if (!IsReady())
				return false;

			IGraphicsContext context = GraphicsContext.CurrentContext;
			CheckExists(context);
			CheckUpdates(context);
			GL.BindTexture(TextureTarget.Texture2D, FTextures[context].ID);

			return true;
		}

		public void Unbind()
		{
			IGraphicsContext context = GraphicsContext.CurrentContext;
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		private void CheckExists(IGraphicsContext context)
		{
			bool create = false;

			if (!FTextures.ContainsKey(context))
				create = true;
			else if (FTextures[context].NeedsAttributesUpdate)
			{
				Destroy(context);
				create = true;
			}

			if (create)
				Create(context);
		}

		private void CheckUpdates(IGraphicsContext context)
		{
			if (FTextures[context].NeedsContentsUpdate)
				Load();
		}

		private void Create(IGraphicsContext context)
		{
			FTextures.Add(context, new TextureInstance(context, FAttributes));
		}

		private void Destroy(IGraphicsContext context)
		{
			FTextures[context].Dispose();
			FTextures.Remove(context);
		}

		public void DestroyAll()
		{
			foreach (KeyValuePair<IGraphicsContext, TextureInstance> tex in FTextures)
				tex.Value.Dispose();

			FTextures.Clear();
		}

		public void Dispose()
		{
			DestroyAll();
		}

		/// <summary>
		/// Override this function with something that loads the texture
		/// onto the graphics card. This is performed once per context.
		/// </summary>
		/// <returns>Loaded successfully</returns>
		abstract public bool Load();

		virtual protected bool IsReady()
		{
			return FAttributes != null;
		}
	}
}
