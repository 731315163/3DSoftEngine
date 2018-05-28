using System.Numerics;

namespace _3DSoftEngine.Scripts
{
    public class Mesh
    {
        //模型上点的坐标
        public Vector3[] Vertices;
        
        //模型的世界坐标
        public Vector3 Positon;
        
        //模型的旋转角度
        public Vector3 Rotation;

        //模型x,y,z上的缩放
        public Vector3 Scale;

        public Mesh(Vector3[] vecs)
        {
            Vertices = vecs;
        }

        public Mesh(int n)
        {
            Vertices = new Vector3[n];
        }
    }
}