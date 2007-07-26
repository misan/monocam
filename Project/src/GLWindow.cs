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
    public partial class GLWindow : Form
    {
        public Camera cam;
        public List<int> dlist;

        public GLWindow()
        {
            InitializeComponent();
            cam = new Camera();
            dlist = new List<int>();

            GLPanel.InitializeContexts();
            Gl.glShadeModel(Gl.GL_SMOOTH);
            Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            Gl.glClearDepth(1.0f);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glDepthFunc(Gl.GL_LEQUAL);
            Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
            GLWindow_Resize(this, new EventArgs());
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
        }

        private void GLWindow_Resize(object sender, EventArgs e)
        {
            float aspect = (float)GLPanel.Width / (float)GLPanel.Height; // THIS WILL FAIL WHEN Height == 0 !!

            System.Console.WriteLine("resize: {0} x {1}, aspect= {2},cam.x={3}", GLPanel.Width, GLPanel.Height, aspect, cam.eye.x);

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Gl.glViewport(0, 0, GLPanel.Width, GLPanel.Height);
            /* gluPerspective( GLdouble	fovy,   field of view in degrees (y-direction)
			       GLdouble	aspect,             aspect ratio: (width/height)
			       GLdouble	zNear,              distance from viewer to near clipping plane
			       GLdouble	zFar )              distance from viewer to far clipping plane
            */
            Glu.gluPerspective(45d, aspect, 0.1d, 100.0d);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            
            Glu.gluLookAt(cam.eye.x, cam.eye.y, cam.eye.z, cam.cen.x, cam.cen.y, cam.cen.z, cam.up.x, cam.up.y, cam.up.z);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
        }

        private void GLWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            GLPanel.DestroyContexts();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GLWindow_Load(object sender, EventArgs e)
        {

        }

        private void GLPanel_Paint(object sender, PaintEventArgs e)
        {

            System.Console.WriteLine("Paint! {0}",cam.eye.x);
            // this is where all drawing happens.

            /*
            Gl.glColor3f(1, 0, 0);
            Gl.glBegin(Gl.GL_TRIANGLES);						// Drawing Using Triangles
            Gl.glVertex3f(0.0f, 1.0f, 0.0f);				// Top
            Gl.glVertex3f(-1.0f, -1.0f, 0.0f);				// Bottom Left
            Gl.glVertex3f(1.0f, -1.0f, 0.0f);				// Bottom Right
            Gl.glEnd();							// Finished Drawing The Triangle
            */

            // run through the dislpaylists:

            // let's draw in wireframe mode
            Gl.glPolygonMode(Gl.GL_FRONT, Gl.GL_LINE);

            foreach (int l in dlist)
            {
                

                Gl.glCallList(l);
            }


            // draw some coordinate axes
            Gl.glColor3f(1, 0, 0); // X-axis is RED
            Gl.glBegin(Gl.GL_LINES);						
            Gl.glVertex3f(0.0f, 0.0f, 0.0f);				
            Gl.glVertex3f(1.0f, 0.0f, 0.0f);				
            Gl.glEnd();

            Gl.glColor3f(0, 1, 0);  // Y-axis is GREEN
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex3f(0.0f, 0.0f, 0.0f);
            Gl.glVertex3f(0.0f, 1.0f, 0.0f);
            Gl.glEnd();

            Gl.glColor3f(0, 0, 1); // Z-axis is BLUE
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex3f(0.0f, 0.0f, 0.0f);
            Gl.glVertex3f(0.0f, 0.0f, 1.0f);
            Gl.glEnd();

            Gl.glFinish();
            // Gl.glFlush();
        }



        private void GLPanel_KeyDown(object sender, KeyEventArgs e)
        {
            System.Console.WriteLine("you pressed: " + e.KeyCode);
            if (e.KeyCode == Keys.Oemplus)       // + for zoom in
                cam.zoom(-0.2);
            else if (e.KeyCode == Keys.OemMinus) // - for zoom out
                cam.zoom(+0.2);
            else if (e.KeyCode == Keys.D)  // D for down
                cam.rotate_fi(+0.1);
            else if (e.KeyCode == Keys.U)  // U for up
                cam.rotate_fi(-0.1);
            else if (e.KeyCode == Keys.L)  // L for left
                cam.rotate_theta(-0.1);
            else if (e.KeyCode == Keys.R)  // R for right
                cam.rotate_theta(+0.1);
            else if (e.KeyCode == Keys.X)  // X for +X-view
                cam.x_view(true);
            else if ((e.Shift == true) && (e.KeyCode == Keys.X))  // shift-X for -X-view
            {
                System.Console.WriteLine("shift.X!"); //WE NEVER GET HERE.... FIXME
                cam.x_view(false);
            }
            else if (e.KeyCode == Keys.Y)  // Y for +Y-view
                cam.y_view(true);
            else if (e.KeyCode == Keys.Z)  // Z for +Z-view
                cam.z_view(true);
            else if (e.KeyCode == Keys.A) // pan left
                cam.pan_lr(-0.03);
            else if (e.KeyCode == Keys.S) // pan right
                cam.pan_lr(+0.03);
            else if (e.KeyCode == Keys.Q) // pan up
                cam.pan_ud(+0.03);
            else if (e.KeyCode == Keys.W) // pan down
                cam.pan_ud(-0.03);


            GLWindow_Resize(this, null);
            //GLPanel_Paint(this, null);
            GLPanel.Refresh(); // this calls paint indirectly.
        }


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
                System.Console.WriteLine("Camera: cen=" + cen + " eye=" + eye);
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
                    _theta = Math.PI/2;
                else
                    _theta = 3*Math.PI/2;

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

        private void openSTLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Console.WriteLine("opening STL");
            System.IO.StreamReader rdr = file_open();
            STLSurf s=null;
            if (rdr != null)
                s = STL.Load(rdr);

            if (s != null)
            {
                System.Console.WriteLine("Adding STL surface to geo-collection!");

                Renderer.MakeRenderList(ref s.gldata[0]); // make the display-list

                dlist.Add((int)s.gldata[0].dlistID);           // add it to the list of displayed lists
            }
        }

        static public System.IO.StreamReader file_open()
            {
                OpenFileDialog ofn = new OpenFileDialog();
                ofn.Filter = "STL file (*.STL)|*.STL";
                ofn.Title = "Open STL file";

                    if (ofn.ShowDialog() == DialogResult.Cancel)
                        return null;

                    System.IO.FileStream strm;
                    try
                    {
                        strm = new System.IO.FileStream(ofn.FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                        System.IO.StreamReader rdr = new System.IO.StreamReader(strm);
                        return rdr;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error opening file", "File Error",
                                         MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return null;
                    }    
        } // end file-open test


        


    } // end GLWindow class
}