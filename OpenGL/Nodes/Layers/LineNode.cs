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

namespace VVVV.Nodes.OpenGL
{
	#region PluginInfo
	[PluginInfo(Name = "Line", Category = "OpenGL", Help = "Draw a Quad", Tags = "")]
	#endregion PluginInfo
	public class LineNode : ILayerSimple
	{
		#region pins
		[Input("Input")]
		IDiffSpread<ISpread<Vector3D>> FPinInInput;

		Spread<Spread<Vector3d>> FPosition = new Spread<Spread<Vector3d>>(0);
		#endregion
		protected override void Update()
		{
			if (FPinInInput.IsChanged)
				return;

			FPosition.SliceCount = FPinInInput.SliceCount;

			int i=0, j;
			foreach (var line in FPinInInput)
			{
				FPosition[i] = new Spread<Vector3d>(line.SliceCount);

				j=0;
				foreach (var vertex in line)
				{
					FPosition[i][j] = UMath.ToGL(vertex);
					j++;
				}
				i++;
			}
		}

		protected override void DrawSlice(int iSlice)
		{
			if (iSlice >= FPosition.SliceCount)
				return;

			GL.Begin(BeginMode.LineStrip);

			foreach (var vertex in FPosition[iSlice])
				GL.Vertex3(vertex);

			GL.End();
		}
	}
}
