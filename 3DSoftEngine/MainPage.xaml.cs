using System.Diagnostics;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using _3DSoftEngine.Scripts;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace _3DSoftEngine
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Device device;
        Mesh mesh = new Mesh(8);
        Camera came = new Camera();
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += Page_Loaded;
        }

        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Call this Function");
            WriteableBitmap bmp = new WriteableBitmap(640,480);
            device = new Device(bmp);

            frontBuffer.Source = bmp;
            mesh.Positon = new Vector3(2,2,3);
            mesh.Vertices[0] = new Vector3(-1, 1, 1);
            mesh.Vertices[1] = new Vector3(1, 1, 1);
            mesh.Vertices[2] = new Vector3(-1, -1, 1);
            mesh.Vertices[3] = new Vector3(-1, -1, -1);
            mesh.Vertices[4] = new Vector3(-1, 1, -1);
            mesh.Vertices[5] = new Vector3(1, 1, -1);
            mesh.Vertices[6] = new Vector3(1, -1, 1);
            mesh.Vertices[7] = new Vector3(1, -1, -1);

            came.Position = new Vector3(10,0,10);
            came.Target = new Vector3(10,0,0);

            CompositionTarget.Rendering += Rendering;
        }

        private void Rendering(object sender, object e)
        {
            device.Clear(0,0,0,255);
            mesh.Rotation = new Vector3(mesh.Rotation.X + 0.01f, mesh.Rotation.Y + 0.01f, mesh.Rotation.Z);
            device.Render(came,mesh);

            device.Present();
        }
    }
}
