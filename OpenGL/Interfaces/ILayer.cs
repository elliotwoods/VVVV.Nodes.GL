#region usings
using System;
using System.ComponentModel.Composition;
using System.Drawing;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Core.Logging;

using System.Collections.Generic;

#endregion usings

namespace VVVV.Nodes.OpenGL
{
	public abstract class ILayer : IPluginEvaluate
	{
		#region fields & pins
		[Output("Layer")]
		ISpread<ILayer> FPinOutLayer;

		[Import]
		ILogger FLogger;

		#endregion fields & pins

		[ImportingConstructor]
		public ILayer()
		{

		}

		protected int SpreadMax = 0;

		/// <summary>
		/// This update call is performed once per frame
		/// </summary>
		/// <param name="SpreadMax"></param>
		protected virtual void Update() { }

		/// <summary>
		/// This draw call is performed once per device.
		/// </summary>
		public abstract void Draw();

		private bool FFirstRun = true;
		public void Evaluate(int SpreadMax)
		{
			this.SpreadMax = SpreadMax;

			if (FFirstRun)
			{
				FPinOutLayer[0] = this;
				FFirstRun = false;
			}

			Update();
		}
	}
}