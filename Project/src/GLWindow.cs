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
        public GeoCollection geom;
        Random random; // used for random point generation (TESTING ONLY)
        public int mdownx, mdowny; // mouse down coordinates
        public int mupx, mupy; // mouse up coordinates
        public bool projection; // true=perspective, false=orthographic

        public GLWindow()
        {
            InitializeComponent();
            cam = new Camera();
            dlist = new List<int>();
            geom = new GeoCollection();
            random = new Random();
            projection = true; // FIXME: this belongs to the camera class


            GLPanel.InitializeContexts();
            Gl.glShadeModel(Gl.GL_SMOOTH);
            Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            Gl.glClearDepth(1.0f);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glDepthFunc(Gl.GL_LEQUAL);
            Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
            GLWindow_Resize(this, null);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
        }

        private void GLWindow_Resize(object sender, EventArgs e)
        {
            float aspect = (float)GLPanel.Width / (float)GLPanel.Height; // THIS WILL FAIL WHEN Height == 0 !!

            // System.Console.WriteLine("resize: {0} x {1}, aspect= {2},cam.x={3}", GLPanel.Width, GLPanel.Height, aspect, cam.eye.x);

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Gl.glViewport(0, 0, GLPanel.Width, GLPanel.Height);

            if (projection)
            {
                /* gluPerspective( GLdouble	fovy,   field of view in degrees (y-direction)
			       GLdouble	aspect,             aspect ratio: (width/height)
			       GLdouble	zNear,              distance from viewer to near clipping plane
			       GLdouble	zFar )              distance from viewer to far clipping plane
                    */
                Glu.gluPerspective(45d, aspect, 0.0, 100.0d);
            }
            else
            {
                /*
                 * void glOrtho(GLdouble left, GLdouble right, GLdouble bottom
                            , GLdouble top , GLdouble zNear, GLdouble zFar);
                 */
                // This still needs a lot of work.
                // both zooming and panning are different when using glOrtho!
                Gl.glOrtho(-10,10,-10,10,0,100);
            }

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

            //System.Console.Write("Paint! " + g);
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
            Gl.glPolygonMode(Gl.GL_BACK, Gl.GL_LINE);

            Gl.glLineWidth(1);
            Gl.glPointSize(3);

            //System.Console.Write("rendering lists: ");
            foreach (int l in dlist)
            {

                //System.Console.Write(l+" ");
                Gl.glCallList(l);
            }

            // if you really want to see what objects are rendered
            // uncomment these two lines
            // foreach (Geo go in g.obj_list)
            //    go.DummyRender();

            //System.Console.Write("\n");


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

            if (e.KeyCode == Keys.X)  // X for +X-view
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
            else if (e.KeyCode == Keys.Oemplus)
                cam.zoom(+0.1);
            else if (e.KeyCode == Keys.OemMinus)
                cam.zoom(-0.1);
      
            GLWindow_Resize(this, null);
            //GLPanel_Paint(this, null);
            GLPanel.Refresh(); // this calls paint indirectly.
        }





        private void openSTLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Console.WriteLine("opening STL");
            System.IO.StreamReader rdr = file_open();
            STLSurf s=null;
            if (rdr != null)
                s = STL.Load(rdr);

            if (s != null)
                addGeom(s);
            else
                System.Console.WriteLine("loading STL file failed. no geometry created.");
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
        }



        private void geoPointToolStripMenuItem_Click(object sender, EventArgs e)
        {           
            //System.Console.Write("adding GeoPoint!: ");
            GeoPoint p = new GeoPoint((double)RandomNumber(-10, 10), (double)RandomNumber(-10, 10), (double)RandomNumber(-10, 10));
            System.Console.WriteLine(p);
            p.color = System.Drawing.Color.Green;
            addGeom(p);
        }

        public void addGeom(Geo g)
        {
            g.gengldata(); // generate gl-data.
            int N = g.gldata.Length;
            for (int n=0; n < N;n++ )
            {
                // make renderlist(s)
                Renderer.MakeRenderList(ref g.gldata[n]);
                // add list to display-lists being rendered
                dlist.Add((int)g.gldata[n].dlistID);
            }
            // add object to geo
            geom.add(g);
            GLPanel.Refresh();

        }

        public int RandomNumber(int min, int max)
        {
            return random.Next(min, max);
        }

        private void addRandomGeoLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Geo.Point p1 = new Geo.Point((double)RandomNumber(-10, 10), (double)RandomNumber(-10, 10), (double)RandomNumber(-10, 10));
            Geo.Point p2 = new Geo.Point((double)RandomNumber(-10, 10), (double)RandomNumber(-10, 10), (double)RandomNumber(-10, 10));
            GeoLine l = new GeoLine(p1, p2);
            l.color = System.Drawing.Color.Red;
            addGeom(l);
            // l.gengldata();
            // Renderer.MakeRenderList(ref l.gldata[0]);
            // dlist.Add((int)l.gldata[0].dlistID);
            // geom.add(l);
            // GLPanel.Refresh();
        }

        private void GLPanel_MouseDown(object sender, MouseEventArgs e)
        {
            // System.Console.WriteLine("mousedown at" + e.Location+"button="+e.Button+"delta="+e.Delta);
            mdownx = e.X;
            mdowny = e.Y;
        }

        private void GLPanel_MouseMove(object sender, MouseEventArgs e)
        {
            // System.Console.WriteLine("mousemove");
            double dragx, dragy;
            // double theta_scale = 0.01;  // multiply drag amount (in pixels) by this to get angle
            // double fi_scale = 0.01;
            // double pan_scale = 0.01;

            double x_limit = 1;  // require this amount of angle change before updating view
            double y_limit = 1;
            double x_pan_limit = 1; // require this amount of panning before updating view
            double y_pan_limit = 1;
            
            if (e.Button != MouseButtons.None)
            {
                // System.Console.WriteLine("button=" + e.Button + "delta" + e.Delta + "clicks" + e.Clicks);
                // System.Console.WriteLine("drag x:" + (e.X - mdownx) + " drag y:" + (e.Y - mdowny));
                switch (e.Button)
                {
                    case (MouseButtons.Right):
                        dragx = e.X - mdownx;
                        dragy = e.Y - mdowny;
                        if (Math.Abs(dragx) > x_limit)
                        {
                            cam.rotate_theta(dragx);
                            GLWindow_Resize(this, null);
                            GLPanel.Refresh();
                            mdownx = e.X;
                            break;
                        }
                        if (Math.Abs(dragy) > y_limit)
                        {
                            cam.rotate_fi(dragy);
                            GLWindow_Resize(this, null);
                            GLPanel.Refresh();
                            mdowny = e.Y;
                            break;
                        }
                        break;
                    case (MouseButtons.Left):
                        dragx = (e.X - mdownx);
                        dragy = (e.Y - mdowny);
                        if (Math.Abs(dragx) > x_pan_limit)
                        {
                            cam.pan_lr(-dragx);
                            GLWindow_Resize(this, null);
                            GLPanel.Refresh();
                            mdownx = e.X;
                            break;
                        }
                        if (Math.Abs(dragy) > y_pan_limit)
                        {
                            cam.pan_ud(dragy);
                            GLWindow_Resize(this, null);
                            GLPanel.Refresh();
                            mdowny = e.Y;
                            break;
                        }
                        break;
                }
            }
        }

        private void GLPanel_MouseWheel(object sender, MouseEventArgs e)
        {
           double zoomratio = 0.01;
           cam.zoom((double)e.Delta * zoomratio);
           GLWindow_Resize(this, null);
           GLPanel.Refresh();
        }

        private void perspectiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Console.WriteLine("To perspective projection");
            projection = true;
            GLWindow_Resize(this, null);
            GLPanel.Refresh();
            //menuStrip1.Items[2].c
        }

        private void orthographicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Console.WriteLine("To orthographic projection");
            projection = false;
            GLWindow_Resize(this, null);
            GLPanel.Refresh();

        }

        private void testCAMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            camtest.run(this);
        }

        private void isinsideTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            camtest.isinside_test(this);
        }

        private void bBoxTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            camtest.bbox_test(this);
        }


    } // end GLWindow class

}