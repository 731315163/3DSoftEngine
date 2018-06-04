using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace _3DSoftEngine.Scripts
{
    public class Device
    {
        //这里面缓存了颜色
        private byte[] backBuffer;
        
        //就是一张image
        private WriteableBitmap bmp;

        public Device(WriteableBitmap bmp)
        {
            this.bmp = bmp;
            backBuffer = new byte[bmp.PixelWidth * bmp.PixelHeight * 4];
           
        }

        public void Clear(byte r, byte g, byte b, byte a)
        {
            for (int i = 0; i < backBuffer.Length; i += 4)
            {
                backBuffer[i] = b;
                backBuffer[i + 1] = g;
                backBuffer[i + 2] = r;
                backBuffer[i + 3] = a;
            }
        }
        // 更新image
        public void Present()
        {
            using (var stream = bmp.PixelBuffer.AsStream())
            {
                stream.Write(backBuffer,0,backBuffer.Length);
            }
            bmp.Invalidate();
        }


        //根据相机位置，将点的坐标转化为世界坐标
        public void Render(Camera camera, params Mesh[] meshes)
        {
            var viewMatrix = Matrix4x4.CreateLookAt(camera.Position, camera.Target, Vector3.UnitY);
           
            var viewTest = MatrixUnit.CreateViewMatrix(camera.Position, camera.Target, Vector3.UnitY);
           
            var projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(0.9f,(float) bmp.PixelWidth / bmp.PixelHeight,0.01f, 1.0f);
            Matrix4x4 projectionTest =
                MatrixUnit.CreateProjectionMatrix(179f * (float)(Math.PI / 180f), (float) bmp.PixelWidth / bmp.PixelHeight, 0.01f, 1.0f);
            foreach (Mesh mesh in meshes)
            {

                var rotate = Matrix4x4.CreateFromYawPitchRoll(mesh.Rotation.Y, mesh.Rotation.X, mesh.Rotation.Z);
                Debug.WriteLine(rotate);
                var world = Matrix4x4.CreateTranslation(mesh.Positon.X, mesh.Positon.Y, mesh.Positon.Z);
                var worldMatrix = world*rotate;
                var transformMatrix = viewTest * projectionTest;
                
                for (var i = 0; i < mesh.Vertices.Length - 1; i++)
                {
                    var point0 = Project(mesh.Vertices[i], transformMatrix);
                    var point1 = Project(mesh.Vertices[i + 1], transformMatrix);
                    DrawPoint(point0);
                   // DrawLine(point0, point1);
                }
            }
        }
        
        //投影，将世界坐标转换为屏幕坐标
        public Vector2 Project(Vector3 coord, Matrix4x4 trans)
        {
            var point = Vector3.Transform(coord, trans);
           
            point.X = (point.X + 1.0f) / 2;
            point.Y = (point.Y + 1.0f) / 2;
            var x = point.X * bmp.PixelWidth ;
            var y = point.Y * bmp.PixelHeight;
            var res = new Vector2(x, y);
     //       Debug.WriteLine(res);
            return res;
        }
    
       /// 一个递归画线的算法
        public void DrawLine(Vector2 bpoint, Vector2 epoint)
       {
           // Length是 求向量长度的函数
           var dist = (epoint - bpoint).Length();
           // point1和point2代表屏幕坐标，
           // 如果距离小于2像素，推出递归
            if (dist < 2)return;

           Vector2 midpoint = bpoint + (epoint - bpoint) / 2;

           DrawPoint(midpoint);

           DrawLine(bpoint,midpoint);
           DrawLine(midpoint,epoint);

       }
        public void DrawPoint(Vector2 point)
        {
            if (point.X >= 0 && point.Y >= 0 && point.X < bmp.PixelWidth && point.Y < bmp.PixelHeight)
            {
                PutPixel((int)point.X ,(int)point.Y,Color.FromArgb(255,255,255,255));
            }
        }
        public void PutPixel(int x, int y, Color color)
        {
            var intdex = (x + y * bmp.PixelWidth) * 4;
            backBuffer[intdex] = color.B;
            backBuffer[intdex + 1] = color.G;
            backBuffer[intdex + 2] = color.R;
            backBuffer[intdex + 3] = color.A;
        }
    }
}