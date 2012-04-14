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
	public abstract class IState : IPluginEvaluate
	{
		#region fields & pins
		[Output("State")]
		ISpread<IState> FPinOutState;

		[Import]
		ILogger FLogger;

		#endregion fields & pins

		[ImportingConstructor]
		public IState()
		{

		}

		protected int SpreadMax = 0;

		public abstract void Push();
		public abstract void Pop();

		private bool FFirstRun = true;
		public void Evaluate(int SpreadMax)
		{
			if (FFirstRun)
			{
				FPinOutState[0] = this;
				FFirstRun = false;
			}

			this.SpreadMax = SpreadMax;
		}
	}
}