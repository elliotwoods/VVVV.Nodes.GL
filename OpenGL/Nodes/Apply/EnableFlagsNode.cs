using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V2;
using OpenTK.Graphics.OpenGL;

namespace VVVV.Nodes.OpenGL
{

	#region PluginInfo
	[PluginInfo(Name = "EnableFlags", Category = "OpenGL", Version = "State", Help = "Give 'Enable' states as IState's", Tags = "")]
	#endregion PluginInfo
	public class EnableFlagsNode : IState
	{
		[Input("Flags", DefaultEnumEntry = "DepthEnable")]
		ISpread<EnableCap> FPinInFlags;

		public override void Push()
		{
			GL.PushAttrib(AttribMask.EnableBit);
			for (int i = 0; i < SpreadMax; i++)
				GL.Enable(FPinInFlags[i]);
		}

		public override void Pop()
		{
			GL.PopAttrib();
		}
	}
}
