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

namespace VVVV.Nodes.OpenGL
{
	public abstract class ILayerSimple : ILayer
	{
		#region pins and fields
		[Input("Transform")]
		ISpread<Matrix4x4> FPinInTransform;

		[Input("Color", DefaultColor= new double[]{1, 1, 1, 1})]
		IDiffSpread<RGBAColor> FPinInColor;

		[Input("Texture")]
		IDiffSpread<Texture> FPinInTexture;
		#endregion

		protected abstract void DrawSlice(int iSlice);
		public override void Draw()
		{
			Matrix4d mat;
			Color col;

			for (int i = 0; i < SpreadMax; i++)
			{
				//apply matrix
				GL.PushMatrix();
				mat = UMath.ToGL(FPinInTransform[i]);
				GL.MultMatrix(ref mat);

				//apply color
				col = FPinInColor[i].Color;
				GL.Color4(col);

				//apply texture
				bool textureBound = false;
				if (FPinInTexture[i] != null)
				{
					FPinInTexture[i].Bind();
					textureBound = true;
				}

				//
				//draw the slice
				DrawSlice(i);
				//
				//

				//release texture
				if (textureBound)
					FPinInTexture[i].Unbind();

				//release matrix
				GL.PopMatrix();
			}
		}
	}
}
