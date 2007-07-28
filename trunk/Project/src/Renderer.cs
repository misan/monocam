using System;
using System.Collections.Generic;
using System.Text;
using Tao.OpenGl;
using Tao.FreeGlut;
using monoCAM;
using Debug;

namespace monoCAM
{
    class Renderer : DebugClient
    {

        static int dlID=0;

        public static void MakeRenderList(ref Geo.glList data)
        {
            if (data.Points == null)
            {
                System.Console.WriteLine("MakeRenderList: no gldata present - nothing to do!");
                return;
            }
            int state = 0;
            int error;
            dlID++;
            // disable for now, doesn't seem to work correctly...
            // if (data.dlistID == null)
            /*
            if (Gl.glIsList(data.dlistID) == 0)
                data.dlistID = Gl.glGenLists(1);
            else
                System.Console.WriteLine("error allocating ID={0}", data.dlistID);
             */

            data.dlistID = dlID;

            while (state != 4)
            {
                error = 0;
                switch (state)
                {
                    case 0:
                        Gl.glNewList(data.dlistID, Gl.GL_COMPILE);
                        state = 1;
                        break;
                    case 1:
                        if (data.type == Geo.glType.GL_POINTS)
                            Gl.glBegin(Gl.GL_POINTS);
                        else if (data.type == Geo.glType.GL_LINES)
                            Gl.glBegin(Gl.GL_LINES);
                        else if (data.type == Geo.glType.GL_TRIANGLES)
                            Gl.glBegin(Gl.GL_TRIANGLES);
                        state = 2;
                        break;
                    case 2:
                        for (int n = 0; n < data.Points.Length; n++)
                        {
                            Gl.glVertex3d(data.Points[n].x, data.Points[n].y, data.Points[n].z);
                        }
                        state = 3;
                        break;
                    case 3:
                        Gl.glEnd();
                        Gl.glEndList();
                        state = 4;
                        break;
                }
                error = Gl.glGetError();
                if (error > 0)
                {
                    //ThrowDebugMessage(this, 0, "OpenGL error (" + Glu.gluErrorString(error) + ")");
                    System.Console.WriteLine("MakeRenderList: Error making display-list ID={0}", data.dlistID);
                    return; // abort.
                }
            }// end while
            System.Console.WriteLine("MakeRenderList: Made display-list! ID={0}", data.dlistID);
        }
    }
}
