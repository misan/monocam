using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tao.OpenGl;
using Tao.FreeGlut;

namespace monoCAM
{ 

    public class Camera
    {
        public Geo.Point eye;
        public Geo.Point cen;
        public Vector up;

        // camera position in spherical coordinates:
        private double _r;
        private double _theta;
        private double _fi;
        private const double r_minimum = 0.1;
        private Vector _u = new Vector(0, 0, 1);

        public Camera()
        {
            eye = new Geo.Point(0, 0, 0);
            cen = new Geo.Point(0, 0, 0);
            up = new Vector(0, 0, 1);

            _theta = 0.1;
            _fi = 1;
            _r = 3;
            recalc();

        }

        private void recalc()
        {
            // recalculate eye position
            eye.x = cen.x + _r * Math.Cos(_theta) * Math.Sin(_fi);
            eye.y = cen.y + _r * Math.Sin(_theta) * Math.Sin(_fi);
            eye.z = cen.z + _r * Math.Cos(_fi);

            // recalculate up-vector
            Vector n = new Vector(Math.Sin(_theta), -Math.Cos(_theta), 0);
            up = n.Cross(eye - cen);
            up.normalize();
            // System.Console.WriteLine("Camera: cen=" + cen + " eye=" + eye);
        }


        public void zoom(double amount)
        {
            _r += amount;
            if (_r <= 0)
                _r = r_minimum;
            recalc();
        }

        public void pan_lr(double amount)
        {
            // move cen to the left
            Vector v = up.Cross(eye - cen);
            v.normalize();
            cen.x += amount * v.x;
            cen.y += amount * v.y;
            cen.z += amount * v.z;
            recalc();
        }
        public void pan_ud(double amount)
        {
            // move cen to the left
            cen.x += amount * up.x;
            cen.y += amount * up.y;
            cen.z += amount * up.z;
            recalc();
        }

        public void rotate_fi(double amount)
        {
            _fi += amount;
            if (_fi >= Math.PI)
                _fi = Math.PI;
            else if (_fi <= 0)
                _fi = 0;

            recalc();
            // System.Console.WriteLine("rotated to fi={0}", _fi);
        }


        public void rotate_theta(double amount)
        {
            _theta += amount;

            recalc();
            // System.Console.WriteLine("rotated to theta={0}", _theta);
        }

        public void x_view(bool b)
        {
            // set view along X-axis
            // if b== true along +X axis
            // if b==false along -X axis
            if (b)
                _theta = 0;
            else
                _theta = Math.PI;

            _fi = Math.PI / 2;
            recalc();
        }

        public void y_view(bool b)
        {
            // set view along Y-axis
            // if b== true along +Y axis
            // if b==false along -Y axis
            if (b)
                _theta = Math.PI / 2;
            else
                _theta = 3 * Math.PI / 2;

            _fi = Math.PI / 2;
            recalc();
        }

        public void z_view(bool b)
        {
            // set view along Y-axis
            // if b== true along +Y axis
            // if b==false along -Y axis
            if (b)
                _fi = 0;
            else
                _fi = Math.PI;

            _theta = 0;
            recalc();
        }



    }
}