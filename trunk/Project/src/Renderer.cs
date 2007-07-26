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

        public static void MakeRenderList(ref Geo.glList data)
        {
            if (data.Points == null)
            {
                return;
            }
            int state = 0;
            int error;

            if (data.dlistID == null)
                data.dlistID = Gl.glGenLists(1);

            while (state != 4)
            {
                error = 0;
                switch (state)
                {
                    case 0:
                        Gl.glNewList(1, Gl.GL_COMPILE);
                        state = 1;
                        break;
                    case 1:
                        if (data.type == Geo.glType.GL_POINTS)
                            Gl.glBegin(Gl.GL_POINT);
                        else if (data.type == Geo.glType.GL_LINES)
                            Gl.glBegin(Gl.GL_LINE);

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
                    return; // abort.
                }
            }// end while
            System.Console.WriteLine("Made display-list!");
        }
    }
}
