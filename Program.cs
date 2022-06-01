using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace ЧислДифф
{
    class Program
    {
        public delegate double F(double x);
        static void Main()
        {
            Console.WindowHeight = 50;
            Console.WindowWidth = 120;
            int n = 11;
            int k = n % 2;

            double a = -1, b = 1; //интервал
            int m = 20;
            double step = Round((b - a) / m, 3);

            double dx = Pow(10, -4);
            F f = (xx) => Exp(-xx * (k + 2)) * Cos(0.1 * n * xx + PI * n / 2) + Pow(xx, n);

            F derf = (xx) => (-n * Exp(-(k + 2) * xx) * Sin(n * xx / 10 + n * PI / 2)) / 10 +
                            (-2 - k) * Exp(-(2 + k) * xx) * Cos(n * xx / 10 + n * PI / 2) +
                            n * Pow(xx, n - 1);

            F der2f = (xx) => (-k - 2) * ((-k - 2) * Exp((-k - 2) * xx) * Cos(n * xx / 10 + PI * n / 2) - n * Exp((-k - 2) * xx) * Sin(n * xx / 10 + PI * n / 2) / 10) -
                              n * ((-k - 2) * Exp((-k - 2) * xx) * Sin(n * xx / 10 + PI * n / 2) + n * Exp((-k - 2) * xx) * Cos(n * xx / 10 + PI * n / 2) / 10) / 10 +
                              (n - 1) * n * Pow(xx, n - 2);

            Console.WriteLine("Функция: ");
            Console.WriteLine("e^(" + -(k + 2) + "*x) * Cos(" + (0.1 * n) + "*x + PI * " + ((double)n / 2) + ") + x^" + n);
            Console.WriteLine();

            Console.WriteLine("f'(x), 1-ый порядок точности: ");
            double[] derF = new double[m];
            for (int i = 0; i <= m; i++)
            {
                Console.WriteLine("x = " + (a + i * step) + "   :   f'(x) = " + DerFunctionX1(f, a + i * step, dx) + "                      " +
                    "f'(x) = " + derf(a + i * step));
            }
            Console.WriteLine();

            Console.WriteLine("f'(x), 2-ой порядок точности: ");
            for (int i = 0; i <= m; i++)
            {
                Console.WriteLine("x = " + (a + i * step) + "   :   f'(x) = " + DerFunctionX2(f, a + i * step, dx) + "                      " +
                    "f'(x) = " + derf(a + i * step));
            }
            Console.WriteLine();

            Console.WriteLine("f''(x), 2-ой порядок точности: ");
            for (int i = 0; i <= m; i++)
            {
                Console.WriteLine("x = " + (a + i * step) + "   :   f''(x) = " + Der2FunctionX2(f, a + i * step, dx) + "                      " +
                    "f''(x) = " + der2f(a + i * step));

            }
            Console.WriteLine("\n\n");



            //Лагранж
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Лагранж:");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("f'(x)");
            int count = 500; //кол-во точек
            double[] x = new double[count];
            double step2 = Round((b - a) / count, 3);

            int j = count / m;
            for (int i = 0; i < count; i++)
            {
                x[i] = a + i * step2;
            }
            double[] coefs = DerLagrange(f, x);

            for (int i = 0; i < m; i++)
            {
                Console.WriteLine("x = " + Round(a + i * step2 * j, 2) + "   :   f'(x) = " + coefs[i * j] + "                      " +
                    "f'(x) = " + derf(Round(a + i * step2 * j, 2)));
            }
            Console.WriteLine("x = " + b + "   :   f'(x) = " + coefs[count - 1] + "                      " +
                    "f'(x) = " + derf(b));
            Console.WriteLine();

            coefs = Der2Lagrange(f, x);
            Console.WriteLine("f''(x)");
            for (int i = 0; i < m; i++)
            {
                Console.WriteLine("x = " + Round(a + i * step2 * j, 2) + "   :   f'(x) = " + coefs[i * j] + "                      " +
                    "f''(x) = " + der2f(Round(a + i * step2 * j, 2)));
            }
            Console.WriteLine("x = " + b + "   :   f'(x) = " + coefs[count - 1] + "                      " +
                    "f''(x) = " + der2f(b));
            Console.WriteLine("\n\n");



            //Cплайны
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Сплайны:");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("f'(x)");

            coefs = DerSpline(f, x);
            for (int i = 0; i < m; i++)
            {
                Console.WriteLine("x = " + Round(a + i * step2 * j, 2) + "   :   f'(x) = " + coefs[i * j] + "                      " +
                    "f'(x) = " + derf(Round(a + i * step2 * j, 2)));
            }
            Console.WriteLine("x = " + b + "   :   f'(x) = " + coefs[count - 1] + "                      " +
                    "f'(x) = " + derf(b));
            Console.WriteLine();

            coefs = Der2Spline(f, x);
            Console.WriteLine("f''(x)");
            for (int i = 0; i < m; i++)
            {
                Console.WriteLine("x = " + Round(a + i * step2 * j, 2) + "   :   f'(x) = " + coefs[i * j] + "                      " +
                    "f''(x) = " + der2f(Round(a + i * step2 * j, 2)));
            }
            Console.WriteLine("x = " + b + "   :   f'(x) = " + coefs[count - 1] + "                      " +
                    "f''(x) = " + der2f(b));

            Console.ReadLine();
        }

        static double DerFunctionX1(F f, double x, double dx) =>
            (f(x + dx) - f(x)) / dx;

        static double DerFunctionX2(F f, double x, double dx) =>
            (f(x + dx) - f(x - dx)) / (2 * dx);

        static double Der2FunctionX2(F f, double x, double dx) =>
            (f(x - dx) - 2 * f(x) + f(x + dx)) / Pow(dx, 2);

        static double[] DerLagrange(F f, double[] x)
        {
            int n = x.Length;
            double[] res = new double[n];
            for (int i = 1; i < n - 1; i++)
            {
                res[i - 1] = f(x[i - 1]) * (2 * x[i - 1] - x[i] - x[i + 1]) / ((x[i] - x[i - 1]) * (x[i + 1] - x[i - 1]))
                            - f(x[i]) * (x[i - 1] - x[i + 1]) / ((x[i] - x[i - 1]) * (x[i + 1] - x[i]))
                            + f(x[i + 1]) * (x[i - 1] - x[i]) / ((x[i + 1] - x[i]) * (x[i + 1] - x[i - 1]));

                res[i] = f(x[i - 1]) * (x[i] - x[i + 1]) / ((x[i] - x[i - 1]) * (x[i + 1] - x[i - 1]))
                        - f(x[i]) * (2 * x[i] - x[i - 1] - x[i + 1]) / ((x[i] - x[i - 1]) * (x[i + 1] - x[i]))
                        + f(x[i + 1]) * (x[i] - x[i - 1]) / ((x[i + 1] - x[i]) * (x[i + 1] - x[i - 1]));

                res[i + 1] = f(x[i - 1]) * (x[i + 1] - x[i]) / ((x[i] - x[i - 1]) * (x[i + 1] - x[i - 1]))
                            - f(x[i]) * (x[i + 1] - x[i - 1]) / ((x[i] - x[i - 1]) * (x[i + 1] - x[i]))
                            + f(x[i + 1]) * (2 * x[i + 1] - x[i] - x[i - 1]) / ((x[i + 1] - x[i]) * (x[i + 1] - x[i - 1]));
            }

            return res;
        }

        static double[] Der2Lagrange(F f, double[] x)
        {
            int n = x.Length;

            double[] res = new double[n];
            for (int i = 1; i < n - 1; i += 3)
            {
                res[i - 1] = res[i] = res[i + 1] =
                    ((f(x[i + 1]) - f(x[i])) / (x[i + 1] - x[i]) - (f(x[i]) - f(x[i - 1])) / (x[i] - x[i - 1])) /
                    (x[i + 1] - x[i - 1]) * 2;
            }
            res[n - 3] = res[n - 2] = res[n - 1] =
                    ((f(x[n - 1]) - f(x[n - 2])) / (x[n - 1] - x[n - 2]) - (f(x[n - 2]) - f(x[n - 3])) / (x[n - 2] - x[n - 3])) /
                    (x[n - 1] - x[n - 3]) * 2;
            return res;
        }

        public static double[] DerSpline(F f, double[] x)
        {
            int n = x.Length;

            double A = (f(x[1]) - f(x[0])) / (x[1] - x[0]);
            double B = (f(x[n - 1]) - f(x[n - 2])) / (x[n - 1] - x[n - 2]);
            double[,] M = new double[n - 2, n - 2];
            double[] b = new double[n - 2];
            for (int i = 1; i < n - 3; i++)
            {
                M[i, i - 1] = (x[i] - x[i - 1]) / 6;
                M[i, i] = (x[i + 1] - x[i - 1]) / 3;
                M[i, i + 1] = (x[i + 1] - x[i]) / 6;
            }
            for (int i = 1; i < n - 1; i++)
            {
                b[i - 1] = (f(x[i + 1]) - f(x[i])) / (x[i + 1] - x[i]) -
                           (f(x[i]) - f(x[i - 1])) / (x[i] - x[i - 1]);
            }
            b[0] -= A * (x[1] - x[0]) / 6;
            b[n - 3] -= B * (x[n - 1] - x[n - 2]) / 6;

            M[0, 0] = (x[2] - x[0]) / 3;
            M[0, 1] = (x[2] - x[1]) / 6;
            M[n - 3, n - 4] = (x[n - 2] - x[n - 3]) / 6;
            M[n - 3, n - 3] = (x[n - 1] - x[n - 3]) / 3;

            double[] s = new double[n];
            double[] tArr = TridiagonalRoots(M, b);
            for (int i = 0; i < tArr.Length; i++)
            {
                s[i + 1] = tArr[i];
            }
            s[0] = A;
            s[n - 1] = B;

            double[] res = new double[n];
            for (int i = 0; i < n - 1; i++)
            {
                res[i] = (f(x[i + 1]) - f(x[i])) / (x[i + 1] - x[i]) -
                    (x[i + 1] - x[i]) * (2 * s[i] + s[i + 1]) / 6;
            }
            res[n - 1] = (f(x[n - 1]) - f(x[n - 2])) / (x[n - 1] - x[n - 2]) -
                (x[n - 1] - x[n - 2]) * (-s[n - 2] - 2 * s[n - 1]) / 6;

            return res;
        }

        public static double[] Der2Spline(F f, double[] x)
        {
            int n = x.Length;

            double A = 2 * ((f(x[2]) - f(x[1])) / (x[2] - x[1])
                       - (f(x[1]) - f(x[0])) / (x[1] - x[0]))
                       / (x[2] - x[0]),

                   B = 2 * ((f(x[n - 1]) - f(x[n - 2])) / (x[n - 1] - x[n - 2])
                       - (f(x[n - 2]) - f(x[n - 3])) / (x[n - 2] - x[n - 3]))
                       / (x[n - 1] - x[n - 3]);

            double[,] M = new double[n - 2, n - 2];
            double[] b = new double[n - 2];
            for (int i = 1; i < n - 3; i++)
            {
                M[i, i - 1] = (x[i] - x[i - 1]) / 6;
                M[i, i] = (x[i + 1] - x[i - 1]) / 3;
                M[i, i + 1] = (x[i + 1] - x[i]) / 6;
            }
            for (int i = 1; i < n - 1; i++)
            {
                b[i - 1] = (f(x[i + 1]) - f(x[i])) / (x[i + 1] - x[i]) -
                           (f(x[i]) - f(x[i - 1])) / (x[i] - x[i - 1]);
            }
            b[0] -= A * (x[1] - x[0]) / 6;
            b[n - 3] -= B * (x[n - 1] - x[n - 2]) / 6;

            M[0, 0] = (x[2] - x[0]) / 3;
            M[0, 1] = (x[2] - x[1]) / 6;
            M[n - 3, n - 4] = (x[n - 2] - x[n - 3]) / 6;
            M[n - 3, n - 3] = (x[n - 1] - x[n - 3]) / 3;

            double[] s = new double[n];
            double[] tArr = TridiagonalRoots(M, b);
            for (int i = 0; i < tArr.Length; i++)
                s[i + 1] = tArr[i];

            s[0] = A;
            s[n - 1] = B;

            return s;
        }

        public static double[] TridiagonalRoots(double[,] a, double[] r)
        {
            double[] b = new double[a.GetLength(0)];
            double[] c = new double[a.GetLength(0)];
            double[] d = new double[a.GetLength(0)];
            double[] delta = new double[a.GetLength(0)];
            double[] alpha = new double[a.GetLength(0)];
            double[] x = new double[a.GetLength(0)];

            for (int i = 0; i < a.GetLength(0); i++)
            {
                if (i == 0)
                    b[i] = 0;
                else
                    b[i] = a[i, i - 1];

                if (i == a.GetLength(0) - 1)
                    d[i] = 0;
                else
                    d[i] = a[i, i + 1];

                c[i] = a[i, i];
            }

            delta[0] = -d[0] / c[0];
            alpha[0] = r[0] / c[0];
            for (int i = 1; i < a.GetLength(0); i++)
            {
                delta[i] = -d[i] / (c[i] + b[i] * delta[i - 1]);
                alpha[i] = (r[i] - b[i] * alpha[i - 1]) / (c[i] + b[i] * delta[i - 1]);
            }

            x[a.GetLength(0) - 1] = alpha[a.GetLength(0) - 1];
            for (int i = a.GetLength(0) - 2; i >= 0; i--)
            {
                x[i] = delta[i] * x[i + 1] + alpha[i];
            }

            return x;
        }
    }
}
