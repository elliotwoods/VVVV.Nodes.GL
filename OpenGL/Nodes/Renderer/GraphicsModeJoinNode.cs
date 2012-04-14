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
using OpenTK.Graphics;

namespace VVVV.Nodes.OpenGL
{
	#region PluginInfo
	[PluginInfo(Name = "GraphicsMode", Category = "OpenGL", Version = "Join", Help = "Change GraphicsMode of a renderer. Note that many input values are wrapped to closest useful value (e.g. 8, 16, 24, 32)", Tags = "")]
	#endregion PluginInfo
	public class GraphicsModeNode : IPluginEvaluate
	{
		#region pins
		[Input("Color Format")]
		IDiffSpread<ColorFormat> FPinInColorFormat;

		[Input("Depth-buffer Depth", DefaultValue = 24, MinValue = 8, MaxValue = 32)]
		IDiffSpread<int> FPinInDepthBufferDepth;

		[Input("Stencil-buffer Depth", DefaultValue = 8, MinValue = 8, MaxValue = 32)]
		IDiffSpread<int> FPinInStencilBufferDepth;

		[Input("MSAA", DefaultValue = 4, MinValue = 1, MaxValue = 32)]
		IDiffSpread<int> FPinInMSAA;

		[Input("Accumulator Format")]
		IDiffSpread<ColorFormat> FPinInAccumulatorFormat;

		[Input("Buffers", DefaultValue=2, MinValue=1, MaxValue=3)]
		IDiffSpread<int> FPinInBuffers;

		[Input("Stereo")]
		IDiffSpread<bool> FPinInStereo;

		[Output("Output")]
		ISpread<GraphicsMode> FPinOutOutput;
		#endregion
		
		bool FFirstRun = true;
		public void Evaluate(int SpreadMax)
		{
			if (InputChanged() || FFirstRun)
			{
				FFirstRun = false;

				FPinOutOutput.SliceCount = SpreadMax;
				ColorFormat FColorFormat;
				ColorFormat FAccumulatorFormat;

				for (int i=0; i<SpreadMax; i++)
				{
					if (FPinInColorFormat[i] == null)
						FColorFormat = new ColorFormat(32);
					else
						FColorFormat = FPinInColorFormat[i];

					if (FPinInAccumulatorFormat[i] == null)
						FAccumulatorFormat = new ColorFormat(0);
					else
						FAccumulatorFormat = FPinInAccumulatorFormat[i];

					FPinOutOutput[i] = new GraphicsMode(FColorFormat, FPinInDepthBufferDepth[i], FPinInStencilBufferDepth[i], FPinInMSAA[i], FAccumulatorFormat, FPinInBuffers[i], FPinInStereo[i]);
				}
			}
		}

		bool InputChanged()
		{
			return FPinInColorFormat.IsChanged || FPinInDepthBufferDepth.IsChanged || FPinInStencilBufferDepth.IsChanged || FPinInMSAA.IsChanged || FPinInAccumulatorFormat.IsChanged || FPinInBuffers.IsChanged || FPinInStereo.IsChanged;
		}
	}
}
