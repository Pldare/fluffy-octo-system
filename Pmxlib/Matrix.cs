namespace PmxLib
{
	public struct Matrix
	{
		public float M11;

		public float M12;

		public float M13;

		public float M14;

		public float M21;

		public float M22;

		public float M23;

		public float M24;

		public float M31;

		public float M32;

		public float M33;

		public float M34;

		public float M41;

		public float M42;

		public float M43;

		public float M44;

		public static Matrix Identity
		{
			get
			{
				Matrix result = default(Matrix);
				result.M11 = 1f;
				result.M22 = 1f;
				result.M33 = 1f;
				result.M44 = 1f;
				return result;
			}
		}

		public float[] ToArray()
		{
			return new float[16]
			{
				this.M11,
				this.M12,
				this.M13,
				this.M14,
				this.M21,
				this.M22,
				this.M23,
				this.M24,
				this.M31,
				this.M32,
				this.M33,
				this.M34,
				this.M41,
				this.M42,
				this.M43,
				this.M44
			};
		}
		public static Matrix RotationYawPitchRoll(float yaw, float pitch, float roll)
		{
			Matrix result = default(Matrix);
			Quaternion quaternion = default(Quaternion);
			quaternion=Quaternion.RotationYawPitchRoll(yaw, pitch, roll);
			Matrix.RotationQuaternion(ref quaternion, out result);
			return result;
		}
		public static void RotationQuaternion(ref Quaternion rotation, out Matrix result)
		{
			double num = (double)rotation.X;
			double num2 = num;
			float num3 = (float)(num2 * num2);
			double num4 = (double)rotation.Y;
			double num5 = num4;
			float num6 = (float)(num5 * num5);
			double num7 = (double)rotation.Z;
			double num8 = num7;
			float num9 = (float)(num8 * num8);
			float num10 = (float)((double)rotation.Y * (double)rotation.X);
			float num11 = (float)((double)rotation.W * (double)rotation.Z);
			float num12 = (float)((double)rotation.Z * (double)rotation.X);
			float num13 = (float)((double)rotation.W * (double)rotation.Y);
			float num14 = (float)((double)rotation.Z * (double)rotation.Y);
			float num15 = (float)((double)rotation.W * (double)rotation.X);
			result.M11 = (float)(1.0 - ((double)num9 + (double)num6) * 2.0);
			result.M12 = (float)(((double)num11 + (double)num10) * 2.0);
			result.M13 = (float)(((double)num12 - (double)num13) * 2.0);
			result.M14 = 0f;
			result.M21 = (float)(((double)num10 - (double)num11) * 2.0);
			result.M22 = (float)(1.0 - ((double)num9 + (double)num3) * 2.0);
			result.M23 = (float)(((double)num15 + (double)num14) * 2.0);
			result.M24 = 0f;
			result.M31 = (float)(((double)num13 + (double)num12) * 2.0);
			result.M32 = (float)(((double)num14 - (double)num15) * 2.0);
			result.M33 = (float)(1.0 - ((double)num6 + (double)num3) * 2.0);
			result.M34 = 0f;
			result.M41 = 0f;
			result.M42 = 0f;
			result.M43 = 0f;
			result.M44 = 1f;
		}
	}
}
