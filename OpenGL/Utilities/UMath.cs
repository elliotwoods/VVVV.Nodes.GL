using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.Utils.VMath;
using OpenTK;

namespace VVVV.Nodes.OpenGL.Utilities
{
	class UMath
	{
		public static Matrix4d ToGL(Matrix4x4 matrix)
		{
			Matrix4d value = new Matrix4d(
				matrix.m11, matrix.m12, matrix.m13, matrix.m14,
				matrix.m21, matrix.m22, matrix.m23, matrix.m24,
				matrix.m31, matrix.m32, matrix.m33, matrix.m34,
				matrix.m41, matrix.m42, matrix.m43, matrix.m44
				);
			return value;
		}

		public static Vector3d ToGL(Vector3D v3)
		{
			Vector3d value = new Vector3d(v3.x, v3.y, v3.z);
			return value;
		}
	}
}
