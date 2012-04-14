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
	[PluginInfo(Name = "Cons", Category = "OpenGL", Help = "Cons Layers (like Group in DirectX 9)", Tags = "")]
	#endregion PluginInfo
	public class ConsNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input")]
		ISpread<ILayer> FPinInInput1;

		[Input("Input 2")]
		ISpread<ILayer> FPinInInput2;

		[Input("Enabled", IsSingle=true, DefaultValue=1, Visibility=PinVisibility.OnlyInspector)]
		ISpread<bool> FPinInEnabled;

		[Output("Output")]
		ISpread<ILayer> FPinOutOutput;

		#endregion fields & pins

		public void Evaluate(int SpreadMax)
		{
			FPinOutOutput.SliceCount = FPinInInput1.SliceCount + FPinInInput2.SliceCount;

			int i = 0;

			foreach (var layer in FPinInInput1)
				FPinOutOutput[i++] = layer;

			foreach (var layer in FPinInInput2)
				FPinOutOutput[i++] = layer;
		}
	}
}
