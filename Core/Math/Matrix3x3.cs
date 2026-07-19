using System.Windows;

namespace KfuPet.Core.Math
{
    public class Matrix3x3
    {
        public double M11 { get; set; }
        public double M12 { get; set; }
        public double M13 { get; set; }

        public double M21 { get; set; }
        public double M22 { get; set; }
        public double M23 { get; set; }

        public double M31 { get; set; }
        public double M32 { get; set; }
        public double M33 { get; set; }

        public Matrix3x3()
        {
            M11 = 1; M12 = 0; M13 = 0;
            M21 = 0; M22 = 1; M23 = 0;
            M31 = 0; M32 = 0; M33 = 1;
        }

        public Matrix3x3(double m11, double m12, double m13,
                         double m21, double m22, double m23,
                         double m31, double m32, double m33)
        {
            M11 = m11; M12 = m12; M13 = m13;
            M21 = m21; M22 = m22; M23 = m23;
            M31 = m31; M32 = m32; M33 = m33;
        }

        public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b)
        {
            return new Matrix3x3(
                a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31,
                a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32,
                a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33,

                a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31,
                a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32,
                a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33,

                a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31,
                a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32,
                a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33
            );
        }

        public Point Transform(Point point)
        {
            return new Point(
                M11 * point.X + M12 * point.Y + M13,
                M21 * point.X + M22 * point.Y + M23
            );
        }

        public static Matrix3x3 Translation(double x, double y)
        {
            return new Matrix3x3(
                1, 0, x,
                0, 1, y,
                0, 0, 1
            );
        }

        public static Matrix3x3 Rotation(double angle)
        {
            double cos = System.Math.Cos(angle);
            double sin = System.Math.Sin(angle);
            return new Matrix3x3(
                cos, -sin, 0,
                sin, cos, 0,
                0, 0, 1
            );
        }

        public static Matrix3x3 Scale(double x, double y)
        {
            return new Matrix3x3(
                x, 0, 0,
                0, y, 0,
                0, 0, 1
            );
        }

        public static Matrix3x3 Identity => new Matrix3x3();
    }
}