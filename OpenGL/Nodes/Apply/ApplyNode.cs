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
	[PluginInfo(Name = "Apply", Category = "OpenGL", Version="State", Help = "Apply an IState to input layers", Tags = "")]
	#endregion PluginInfo
	public class ApplyNode : ILayer
	{
		#region fields & pins
		[Input("Layer")]
		ISpread<ILayer> FPinInLayer;

		[Input("State")]
		ISpread<IState> FPinInState;

		[Input("Enabled", DefaultValue=1)]
		ISpread<bool> FPinInEnabled;

		#endregion fields & pins

		public override void Draw()
		{
			for (int i = 0; i < SpreadMax; i++)
			{
				if (FPinInEnabled[i])
					if (FPinInState[i] != null)
						FPinInState[i].Push();

				if (FPinInLayer[i] != null)
					FPinInLayer[i].Draw();

				if (FPinInEnabled[i])
					if (FPinInState[i] != null)
						FPinInState[i].Pop();
			}
		}
	}
}
