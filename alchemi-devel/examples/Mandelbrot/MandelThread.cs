using System;
using System.Drawing;
using KarlsTools; // http://www.codeproject.com/dotnet/complex_math.asp
using Alchemi.Core;

namespace Alchemi.Examples.Mandelbrot
{
    [Serializable]
    public class MandelThread : GThread
    {
        Bitmap _map;
        int _mapNumX;
        int _mapNumY;
        int _width;
        int _height;
        int _xOffset;
        int _yOffset;
        int _zoom;
        Color _clr1;
        Color _clr2;

        public MandelThread(int mapNumX, int mapNumY, int width, int height, int xOffset, int yOffset, int zoom, Color clr1, Color clr2)
        {
            _width = width;
            _height = height;
            _xOffset = xOffset;
            _yOffset = yOffset;
            _zoom = zoom;
            _mapNumX = mapNumX;
            _mapNumY = mapNumY;
            _clr1 = clr1;
            _clr2 = clr2;
        }

        public override void Start()
        {
            _map = new Bitmap(_width, _height);


            for (double x=0; x<_width; x++)
            {
                for (double y=0; y<_height; y++)
                {
                    Complex z = new Complex(0, 0);
                    Complex c = new Complex((x + _xOffset)/_zoom, (y+_yOffset)/_zoom);
                    
                    bool inSet = true;
                    int i;
                    for (i=0; i<=63; i++)
                    {
                        z = (z * z) + c;

                        if (Double.IsInfinity(z.Real()) | Double.IsNaN(z.Imag()))
                        {
                            inSet = false;
                            break;
                        }
                    }
                    if (inSet)
                    {
                        _map.SetPixel((int) x, (int) y, Color.Black);
                    }
                    else
                    {
                        Color clr = GradientColor(_clr1, _clr2, ((double) i)/63);
                        _map.SetPixel((int) x, (int)y, clr);
                    }
                }
            }
        }

        static Color GradientColor(Color start, Color end, double delta)
        {
            return Color.FromArgb(
                GradientColorValue(start.R, end.R, delta),
                GradientColorValue(start.G, end.G, delta),
                GradientColorValue(start.B, end.B, delta)
                );
        }

        static int GradientColorValue(int start, int end, double delta)
        {
            return (int) (start + (end - start) * delta);
        }

        public Bitmap Map
        {
            get { return _map; }
        }
        
        public int MapNumX     
        {
            get { return _mapNumX; }
        }

        public int MapNumY
        {
            get { return _mapNumY; }
        }

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }
    }
}
