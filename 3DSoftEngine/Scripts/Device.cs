using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml.Media;
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
            Debug.WriteLine(viewMatrix);

            var projectionMatrix =
                Matrix4x4.CreatePerspective(0.78f, (float) bmp.PixelWidth / bmp.PixelHeight, 0.01f, 1.0f);
            foreach (Mesh mesh in meshes)
            {
                    
                var rotate = Matrix4x4.CreateFromYawPitchRoll(mesh.Rotation.Y, mesh.Rotation.X, mesh.Rotation.Z);
               
                var world = Matrix4x4.CreateTranslation(mesh.Positon.X, mesh.Positon.Y, mesh.Positon.Z);
                var worldMatrix = rotate * world;
                 var transformMatrix =worldMatrix * viewMatrix * projectionMatrix;
                foreach (Vector3 vertex in mesh.Vertices)
                {
                    Vector2 point = Project(vertex, transformMatrix);
                    DrawPoint(point);
                }
            }
        }
        //投影，将世界坐标转换为屏幕坐标
        public Vector2 Project(Vector3 coord, Matrix4x4 trans)
        {
            var point = Vector3.Transform(coord, trans);
            var x = point.X * bmp.PixelWidth + bmp.PixelWidth / 2f;
            var y = point.Y * bmp.PixelHeight + bmp.PixelHeight / 2f;
            return new Vector2(x, y);
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