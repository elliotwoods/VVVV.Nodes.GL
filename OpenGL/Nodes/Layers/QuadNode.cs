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
using System.Drawing;
using System.Drawing.Imaging;

namespace VVVV.Nodes.OpenGL
{
	#region PluginInfo
	[PluginInfo(Name = "Quad", Category = "OpenGL", Help = "Draw a Quad", Tags = "")]
	#endregion PluginInfo
	public class QuadNode : ILayerSimple
	{
		protected override void Update()
		{

		}

		protected override void DrawSlice(int iSlice)
		{
			//GL.BindTexture(TextureTarget.Texture2D, texture);
			GL.Begin(BeginMode.Quads);

			GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-0.5f, -0.5f);
			GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(0.5f, -0.5f);
			GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(0.5f, 0.5f);
			GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-0.5f, 0.5f);

			GL.End();
		}
	}
}
