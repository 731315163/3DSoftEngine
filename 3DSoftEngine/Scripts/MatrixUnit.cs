using System;
using System.Numerics;

namespace _3DSoftEngine.Scripts
{
    public static class MatrixUnit
    {
      
        public static Matrix4x4 CreateProjectionMatrix(float fov, float aspect, float zn, float zf)
        {
            Matrix4x4 proj = new Matrix4x4
            {

                M11 = (float) (1f / (Math.Tan(fov * 0.5f) / aspect)),
                M22 = (float) (1f / Math.Tan(fov * 0.5f)),
                M33 = zf / (zn - zf),
                M34 = (zn * zf) / (zn - zf),
                M43 = -1.0f
            };
            return proj;
        }

        public static Matrix4x4 CreateViewMatrix(Vector3 cameraPosition,Vector3 cameraTarget , Vector3 cameraUp)
        {
            Matrix4x4 res = new Matrix4x4();
            
            Vector3 Z = Vector3.Normalize(cameraPosition - cameraTarget);
            
            Vector3 X =Vector3.Normalize( Vector3.Cross(cameraUp,Z));
            
            Vector3 Y =Vector3.Cross(Z,X);

            res.M11 = X.X;
            res.M12 = X.Y;
            res.M13 = X.Z;
            res.M14 = - cameraPosition.X * (X.X + X.Y + X.Z);

            res.M21 = Y.X;
            res.M22 = Y.Y;
            res.M23 = Y.Z;
            res.M24 = -cameraPosition.Y * (Y.X + Y.Y + Y.Z);

            res.M31 = Z.X;
            res.M32 = Z.Y;
            res.M33 = Z.Z;
            res.M34 = -cameraPosition.Z * (Z.X + Z.Y + Z.Z);

            res.M41 = 0;
            res.M42 = 0;
            res.M43 = 0;
            res.M44 = 1;
            return res;
        }

        public static Matrix4x4 CreateRotateMatrix(float xradian, float yradian, float zradian)
        {
            Matrix4x4 x = new Matrix4x4(), y = new Matrix4x4(), z = new Matrix4x4();
            
            float xsin =(float) Math.Sin(xradian);
            float xcos = (float) Math.Cos(xradian);
            x.M11 = 1;
            x.M22 = xcos;
            x.M23 = -xsin;
            x.M32 = xsin;
            x.M33 = xcos;
            x.M44 = 1;

            float ysin = (float) Math.Sin(yradian);
            float ycos = (float) Math.Cos(yradian);

            y.M11 = ycos;
            y.M13 = ysin;
            y.M22 = 1;
            y.M31 = -ysin;
            y.M33 = ycos;
            y.M44 = 1;

            float zsin = (float) Math.Sin(zradian);
            float zcos = (float) Math.Cos(zradian);

            z.M11 = zcos;
            z.M12 = -zsin;
            z.M21 = zsin;
            z.M22 = zcos;
            z.M33 = 1;
            z.M44 = 1;

            Matrix4x4 temp =  Matrix4x4.Multiply(x, y);
            return Matrix4x4.Multiply(temp, z);
        }
        
    }
}
