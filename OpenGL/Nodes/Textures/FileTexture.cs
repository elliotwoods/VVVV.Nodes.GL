using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V2;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace VVVV.Nodes.OpenGL
{
	class BitmapTextureInstance : Texture, IDisposable
	{
		public BitmapTextureInstance(Bitmap Bitmap)
		{
			FBitmap = Bitmap;
			this.FAttributes = new TextureAttributes(Bitmap.Width, Bitmap.Height, PixelInternalFormat.Rgba, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte);
		}
	
		public Bitmap FBitmap;

		public override bool Load()
		{
			if (FBitmap == null)
				return false;
			
			BitmapData data = FBitmap.LockBits(new System.Drawing.Rectangle(0, 0, FBitmap.Width, FBitmap.Height),
				ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
				OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

			FBitmap.UnlockBits(data);
			return true;
		}

		void IDisposable.Dispose()
		{
			FBitmap.Dispose();
			base.Dispose();
		}
	}

	#region PluginInfo
	[PluginInfo(Name = "FileTexture", Category = "OpenGL", Help = "Load a file into a texture", Tags = "")]
	#endregion PluginInfo
	public class FileTexture : IPluginEvaluate, IDisposable
	{
		[Input("Filename", StringType = StringType.Filename)]
		IDiffSpread<string> FPinInFilename;

		[Input("Reload", IsBang = true)]
		ISpread<bool> FPinInReload;

		[Output("Texture")]
		ISpread<Texture> FPinOutOutput;

		[Output("String")]
		ISpread<string> FPinOutStatus;

		public void Evaluate(int SpreadMax)
		{
			if (FPinInFilename.IsChanged)
			{
				FPinOutOutput.SliceCount = SpreadMax;
				FPinOutStatus.SliceCount = SpreadMax;

				for (int i = 0; i < SpreadMax; i++)
					LoadSlice(i);
			}

			for (int i = 0; i < SpreadMax; i++)
			{
				if (FPinInReload[i])
					LoadSlice(i);
			}
		}

		private void LoadSlice(int i)
		{
			if (!File.Exists(FPinInFilename[i]))
			{
				FPinOutStatus[i] = "File not found";
				return;
			}
			try
			{
				Bitmap bmp = new Bitmap(FPinInFilename[i]);
				FPinOutOutput[i] = new BitmapTextureInstance(bmp);
				FPinOutStatus[i] = "OK";
			}
			catch (Exception e)
			{
				FPinOutStatus[i] = e.Message;
			}
		}

		private void ClearOutput()
		{
			foreach(Texture tex in FPinOutOutput)
			{
				if (tex != null)
					tex.Dispose();
			}
		}

		public void Dispose()
		{
			ClearOutput();
		}
	}
}
