using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using _3DTools;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
namespace _3dTest
{
    /// <summary>
    /// UserControl1.xaml 的互動邏輯
    /// </summary>
    public partial class UserControl1 : UserControl
    {

        
        
        
        public UserControl1()
        {
            InitializeComponent();

            abc();
            
        }

        void drawCube(int x0 ,int y0, int z0, int x1,int y1,int z1) { 

            
    

        }

        

        void abc()
        {
            
            ScreenSpaceLines3D wireFrameCube = new ScreenSpaceLines3D();
            Color c = Colors.Orange;
            int width = 2;
            Point3D p0 = new Point3D(0, 0, 0);
            Point3D p1 = new Point3D(10, 0, 0);
            Point3D p2 = new Point3D(10, 10, 0);//endpoint
            Point3D p3 = new Point3D(0, 10, 0);
            Point3D p4 = new Point3D(0, 0, 10);
            Point3D p5 = new Point3D(10, 0, 10);
            Point3D p6 = new Point3D(10, 10, 10);
            Point3D p7 = new Point3D(0, 10, 10);//startpoint
            wireFrameCube.Thickness = width;
            wireFrameCube.Color = c;
            
            wireFrameCube.Points.Add(p0);
            wireFrameCube.Points.Add(p1);

            wireFrameCube.Points.Add(p1);
            wireFrameCube.Points.Add(p2);

            wireFrameCube.Points.Add(p2);
            wireFrameCube.Points.Add(p3);

            wireFrameCube.Points.Add(p3);
            wireFrameCube.Points.Add(p0);

            wireFrameCube.Points.Add(p4);
            wireFrameCube.Points.Add(p5);

            wireFrameCube.Points.Add(p5);
            wireFrameCube.Points.Add(p6);

            wireFrameCube.Points.Add(p6);
            wireFrameCube.Points.Add(p7);

            wireFrameCube.Points.Add(p7);
            wireFrameCube.Points.Add(p4);

            wireFrameCube.Points.Add(p0);
            wireFrameCube.Points.Add(p4);

            wireFrameCube.Points.Add(p1);
            wireFrameCube.Points.Add(p5);

            wireFrameCube.Points.Add(p2);
            wireFrameCube.Points.Add(p6);

            wireFrameCube.Points.Add(p3);
            wireFrameCube.Points.Add(p7);
            wireFrameCube.Transform = new Transform3DGroup();

            mainViewPort.Children.Add(wireFrameCube);
           // mainViewPort.Children.Add(wireFrameCube);


            Point3D ap0 = new Point3D(-20, 0, 0);
            Point3D ap1 = new Point3D(20, 0, 0);
            Point3D ap2 = new Point3D(0, -20, 0);
            Point3D ap3 = new Point3D(0, 20, 0);
            Point3D ap4 = new Point3D(0, 0, -20);
            Point3D ap5 = new Point3D(0, 0, 20);

            ScreenSpaceLines3D wireframe = new ScreenSpaceLines3D();
            ScreenSpaceLines3D wireframe2 = new ScreenSpaceLines3D();
            ScreenSpaceLines3D wireframe3 = new ScreenSpaceLines3D();
            wireframe.Points.Add(ap0);
            wireframe.Points.Add(ap1);
            wireframe2.Points.Add(ap2);
            wireframe2.Points.Add(ap3);
            wireframe3.Points.Add(ap4);
            wireframe3.Points.Add(ap5);
            wireframe.Color = Colors.Black;
            wireframe2.Color = Colors.Red;
            wireframe3.Color = Colors.Green;
            wireframe.Thickness = 3;
            wireframe2.Thickness = 3;
            wireframe3.Thickness = 3;
            //黑X軸 洪Y軸 綠Z軸
            mainViewPort.Children.Add(wireframe);

            mainViewPort.Children.Add(wireframe2);

            mainViewPort.Children.Add(wireframe3);

            
        }

        Point mouseLastPosition;
        double mouseDeltaFactor = 2;
        private void mainViewPort_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mouseLastPosition = e.GetPosition(this);
        }

        private void mainViewPort_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                Point newMousePosition = e.GetPosition(this);
                if (mouseLastPosition.X != newMousePosition.X)
                {
                    HorizontalTransform(mouseLastPosition.X < newMousePosition.X, mouseDeltaFactor);//水平变换
                }
                if (mouseLastPosition.Y != newMousePosition.Y)// change position in the horizontal direction
                {
                    VerticalTransform(mouseLastPosition.Y > newMousePosition.Y, mouseDeltaFactor);//垂直变换
                }
                mouseLastPosition = newMousePosition;
            }
        }

        private void VerticalTransform(bool upDown, double angleDeltaFactor)
        {
            Vector3D postion = new Vector3D(camera.Position.X, camera.Position.Y, camera.Position.Z);
            Vector3D rotateAxis = Vector3D.CrossProduct(postion, camera.UpDirection);
            RotateTransform3D rt3d = new RotateTransform3D();
            AxisAngleRotation3D rotate = new AxisAngleRotation3D(rotateAxis, angleDeltaFactor * (upDown ? -1 : 1));
            rt3d.Rotation = rotate;
            Matrix3D matrix = rt3d.Value;
            Point3D newPostition = matrix.Transform(camera.Position);
            camera.Position = newPostition;
            camera.LookDirection = new Vector3D(-newPostition.X, -newPostition.Y, -newPostition.Z);

            //update the up direction
            Vector3D newUpDirection = Vector3D.CrossProduct(camera.LookDirection, rotateAxis);
            newUpDirection.Normalize();
            camera.UpDirection = newUpDirection;
        }
        private void HorizontalTransform(bool leftRight, double angleDeltaFactor)
        {
            Vector3D postion = new Vector3D(camera.Position.X, camera.Position.Y, camera.Position.Z);
            Vector3D rotateAxis = camera.UpDirection;
            RotateTransform3D rt3d = new RotateTransform3D();
            AxisAngleRotation3D rotate = new AxisAngleRotation3D(rotateAxis, angleDeltaFactor * (leftRight ? -1 : 1));
            rt3d.Rotation = rotate;
            Matrix3D matrix = rt3d.Value;
            Point3D newPostition = matrix.Transform(camera.Position);
            camera.Position = newPostition;
            camera.LookDirection = new Vector3D(-newPostition.X, -newPostition.Y, -newPostition.Z);
        }

        private void mainViewPort_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double scaleFactor = 3;
            //120 near ,   -120 far
            System.Diagnostics.Debug.WriteLine(e.Delta.ToString());
            Point3D currentPosition = camera.Position;
            Vector3D lookDirection = camera.LookDirection;//new Vector3D(camera.LookDirection.X, camera.LookDirection.Y, camera.LookDirection.Z);
            lookDirection.Normalize();

            lookDirection *= scaleFactor;

            if (e.Delta == 120)//getting near
            {
                if ((currentPosition.X + lookDirection.X) * currentPosition.X > 0)
                {
                    currentPosition += lookDirection;
                }
            }
            if (e.Delta == -120)//getting far
            {
                currentPosition -= lookDirection;
            }

            Point3DAnimation positionAnimation = new Point3DAnimation();
            positionAnimation.BeginTime = new TimeSpan(0, 0, 0);
            positionAnimation.Duration = TimeSpan.FromMilliseconds(100);
            positionAnimation.To = currentPosition;
            positionAnimation.From = camera.Position;
            positionAnimation.Completed += new EventHandler(positionAnimation_Completed);
            camera.BeginAnimation(PerspectiveCamera.PositionProperty, positionAnimation, HandoffBehavior.Compose);
        }

        void positionAnimation_Completed(object sender, EventArgs e)
        {
            Point3D position = camera.Position;
            camera.BeginAnimation(PerspectiveCamera.PositionProperty, null);
            camera.Position = position;
        }

        
        
        
        
    }
}
