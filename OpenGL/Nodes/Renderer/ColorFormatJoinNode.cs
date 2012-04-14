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
	[PluginInfo(Name = "ColorFormat", Category = "OpenGL", Help = "Create a ColorFormat based on bits per pixel", Tags = "")]
	#endregion PluginInfo
	public class ColorFormatAllJoinNode : IPluginEvaluate
	{
		#region pins
		[Input("Bits Per Pixel", DefaultValue=32, MinValue=1, MaxValue=32)]
		IDiffSpread<int> FPinInBPP;

		[Output("Output")]
		ISpread<ColorFormat> FPinOutOutput;
		#endregion
		
		bool FFirstRun = true;
		public void Evaluate(int SpreadMax)
		{
			if (FPinInBPP.IsChanged || FFirstRun)
			{
				FFirstRun = false;

				FPinOutOutput.SliceCount = SpreadMax;

				for (int i=0; i<SpreadMax; i++)
				{
					FPinOutOutput[i] = new ColorFormat(FPinInBPP[i]);
				}
			}
		}
	}

	#region PluginInfo
	[PluginInfo(Name = "ColorFormat", Category = "OpenGL", Version = "RGBA", Help = "Create a ColorFormat based on bits per pixel per channel", Tags = "")]
	#endregion PluginInfo
	public class ColorFormatIndividualJoinNode : IPluginEvaluate
	{
		#region pins
		[Input("Red", DefaultValue = 32, MinValue = 1, MaxValue = 32, DimensionNames = new string[] { "bits" })]
		IDiffSpread<int> FPinInRed;
		[Input("Green", DefaultValue = 32, MinValue = 1, MaxValue = 32, DimensionNames = new string[] { "bits" })]
		IDiffSpread<int> FPinInGreen;
		[Input("Blue", DefaultValue = 32, MinValue = 1, MaxValue = 32, DimensionNames = new string[] { "bits" })]
		IDiffSpread<int> FPinInBlue;
		[Input("Alpha", DefaultValue = 32, MinValue = 1, MaxValue = 32, DimensionNames = new string[] { "bits" })]
		IDiffSpread<int> FPinInAlpha;

		[Output("Output")]
		ISpread<ColorFormat> FPinOutOutput;
		#endregion

		bool FFirstRun = true;
		public void Evaluate(int SpreadMax)
		{
			if (FPinInRed.IsChanged || FPinInGreen.IsChanged || FPinInBlue.IsChanged || FPinInAlpha.IsChanged || FFirstRun)
			{
				FFirstRun = false;

				FPinOutOutput.SliceCount = SpreadMax;

				for (int i = 0; i < SpreadMax; i++)
				{
					FPinOutOutput[i] = new ColorFormat(FPinInRed[i], FPinInGreen[i], FPinInBlue[i], FPinInAlpha[i]);
				}
			}
		}
	}
}
