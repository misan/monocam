using System;
using System.Collections.Generic;
using System.Text;

// this file should contain geometry stuff for monoCAM
// ToDo:
// we need a geometry container that contains all geometry.
// it should support operations like these:
// GeoPoint mypoint = new GeoPoint(x,y,z)
// geo_cont.add(mypoint)
// geo_cont.delete(mypoint)
//
// all the objects in this container should have a mechanism
// for letting the renderer know that something has changed.

namespace monoCAM
{
   class GeoCollection
   {
       // this container should contain all geometry objects
       // possibly separate collections could be used for:
       // - CAD data
       // - CAM data
       // - special data (grids, coordinate systems etc)

       public List<Geo> obj_list;

       public GeoCollection()
       {
           obj_list = new List<Geo>();
       }

       public void add(Geo g)
       {
           obj_list.Add(g);
       }

       public void del(Geo g)
       {
           obj_list.Remove(g);
       }

       public int Count()
       {
           return obj_list.Count;
       }

       public override string ToString()
       {
           return "GeoCollection with " + Count() + " objects\n";
       }



   }

   public class Geo
   {
       // this is the base class for all geometry objects
       // first some useful struct definitions:

       public struct Point
       {
           public double X;
           public double Y;
           public double Z;
           public Point(double x, double y, double z)
           {
               X = x;
               Y = y;
               Z = z;
           }
           public override string ToString()
           {
               return "(" + X + " , " + Y + " , " + Z + ")";
           }
       }

       public struct Tri // consider making it a class so we can check for null
       {

           public Point[] p;
           public Vector n;
           public Tri(Point P1, Point P2, Point P3)
           {
               p = new Point[3];
               p[0] = P1;
               p[1] = P2;
               p[2] = P3;
               // if normal is not given, calculate it here.
               Vector v1 = new Vector(p[0].X - p[1].X, p[0].Y - p[1].Y, p[0].Z - p[1].Z);
               Vector v2 = new Vector(p[0].X - p[2].X, p[0].Y - p[2].Y, p[0].Z- p[2].Z);
               n = v1.Cross(v2); // the normal is in the direction of the cross product between the edge vectors
               n = (1 / n.Length()) * n; // normalize to length==1
           }

           public Tri(Point P1, Point P2, Point P3, Vector N)
           {
               p = new Point[3];
               p[0] = P1;
               p[1] = P2;
               p[2] = P3;
               n = N;
           }

           public Tri(Vector N) {

               p = new Point[3];
               p[0] = new Point();
               p[1] = new Point();
               p[2] = new Point();
               n = N;

           }





       }

       public struct glColor
       {
           public float R;
           public float G;
           public float B;
       }

       public enum glType : int
       {
           GL_POINTS, GL_LINES, GL_TRIANGLES
       }

       public struct glList
       {
           // this defines a gl display-list
           // this data can render:
           // - a single point  (or many points, but that is not needed)
           // - a curve consisting of one or many line segments
           // - a surface consisting of one or many triangles

           public glType type;    // one of GL_POINTS, GL_LINES, GL_TRIANGLES
           public glColor color;
           public Point[] Points;
           public bool shown;        // false means hidden
           #pragma warning disable 0649 // The value should be set by the renderlist generator.
           public int? dlistID;       // the OpenGL display-list ID

           // TO-DO:
           // - line types (solid, dashed, etc)
           // - line width
           // - point size
           // - wireframe/shaded mode switch
       }



       public glList[] gldata;   // all data that the renderer needs is here
       public int layer;         // the layer of the object

       public string name;       // the name of the object

       public Geo()
       {
           // default constructor
       }

       public void DummyRender()
       {
           // this is a dummy method that simulates how the renderer
           // will interact with a geo object
           // the renderer code would be fairly similar but use OpenGL
           // commands instead of Write as I have done here.

           System.Console.Write("object " + this + " has {0} displaylists:\n", gldata.Length);

           // here we loop through the display-lists of the object:
           for (int n = 0; n < gldata.Length; n++)
           {

               System.Console.Write("\t{0}: ", n + 1);

               // print out the type of display-list:
               string t;
               switch (gldata[n].type)
               {
                   case glType.GL_LINES:
                       t = "LINES";
                       break;
                   case glType.GL_POINTS:
                       t = "POINTS";
                       break;
                   case glType.GL_TRIANGLES:
                       t = "TRIANGLES";
                       break;
                   default:
                       t = "ERROR";
                       break;
               }

               System.Console.Write("type=" + t);

               // print out the color
               System.Console.Write(" color=(" + gldata[n].color.R + "," + gldata[n].color.G + "," + gldata[n].color.B + ")");

               // print out how many points with this type and color
               System.Console.Write(" N={0} points: {1}\n", gldata[n].Points.Length, gldata[n].shown ? "(shown)" : "(hidden)");

               // loop through the points and print the coordinates
               for (int m = 0; m < gldata[n].Points.Length; m++)
               {
                   System.Console.Write("\t\t(" + gldata[n].Points[m].X + "," + gldata[n].Points[m].Y + "," + gldata[n].Points[m].Z + ")\n");
               }

               //System.Console.Write("\n");
           }
       }
   }




   /* ****************** END of base class definition **********/




   class GeoPoint : Geo
   {
       // experimental implementation of a Point
       public Point p;
       public GeoPoint(double x, double y, double z)
       {
           // GeoPoint();
           SetPos(x, y, z);
           SetName(null);
           layer = 0;

           gldata = new glList[1]; // we only need one glList for drawing a point
           gldata[0].type = glType.GL_POINTS;
           gldata[0].Points = new Point[1]; // we only need one point
           gldata[0].Points[0].X = x;
           gldata[0].Points[0].Y = y;
           gldata[0].Points[0].Z = z;
           gldata[0].color.R = 11;
           gldata[0].color.G = 22;
           gldata[0].color.B = 33;
           gldata[0].shown = true;
       }
       public void SetName(string s)
       {
           if (s != null)
               name = s;
           else
               name = "p";
       }
       public void Hide()
       {
           gldata[0].shown = false;
       }

       public void SetPos(double X, double Y, double Z)
       {
           p.X = X;
           p.Y = Y;
           p.Z = Z;
       }

       public override string ToString()
       {
           return "GeoPoint " + name + " at (" + p.X + " , " + p.Y + " , " + p.Z + ")";
       }

   }

   class GeoLine : Geo
   {
       // experimental Line class. May be used initially for CAM output
       public Point start, end;
       public GeoLine(Point s, Point e)
       {
           start = s;
           end = e;
           gengldata();
       }
       public void gengldata()
       {
           gldata = new glList[1]; // we need only one display-list for representing a Line
           gldata[0].type = glType.GL_LINES;
           gldata[0].Points = new Point[2]; // we need two points for a line
           gldata[0].Points[0] = start;
           gldata[0].Points[1] = end;
           gldata[0].shown = true;
       }

       public override string ToString()
       {
           return "GeoLine from " + start + " to " + end;
       }

       public double Length()
       {
           return Math.Sqrt(Math.Pow(start.X - end.X, 2) + Math.Pow(start.Y - end.Y, 2) + Math.Pow(start.Z - end.Z, 2));
       }

   }

   class STLSurf : Geo
   {
       // experimental STL surface class
       private List<Tri> tris;  // a list that holds the vertices.
       // the length of the list should always be a multiple of three!


       public STLSurf()
       {
           tris = new List<Tri>();
           gengldata();
       }

       public void gengldata()
       {
           gldata = new glList[1]; // we need only one display-list for representing an STLSurf
           gldata[0].type = glType.GL_TRIANGLES;
           gldata[0].Points = new Point[tris.Count * 3];

           // loop thorugh triangles and add points to gllist
           int n = 0;
           foreach (Tri t in tris)
           {
               gldata[0].Points[n] = new Point(t.p[0].X, t.p[0].Y, t.p[0].Z);
               gldata[0].Points[n + 1] = new Point(t.p[1].X, t.p[1].Y, t.p[1].Z);
               gldata[0].Points[n + 2] = new Point(t.p[2].X, t.p[2].Y, t.p[2].Z);
               n += 3;
           }
           gldata[0].color.R = 11;
           gldata[0].color.G = 22;
           gldata[0].color.B = 33;
           gldata[0].shown = true;
       }

       public void AddTriangle(Tri t)
       {
           // add one triangle to the surface
           tris.Add(t);

           // need to re-generate gldata when new triangle added:
           gengldata();
       }

       public override string ToString()
       {
           return "STLSurf with " + tris.Count + " triangles";
       }

   }

   public class Vector
   {
       // experimental vector class for MonoCAM
       // todo: rotations? (== matrix multiplication?)

       public double x, y, z;
       public Vector(double X, double Y, double Z)
       {
           this.x = X;
           this.y = Y;
           this.z = Z;
       }
       public Vector()
       {
           this.x = 0;
           this.y = 0;
           this.z = 0;
       }

       public override string ToString()
       {
           return "("+x+","+y+","+z+")";
       }

       public static Vector operator *(double a, Vector v)
       {
           // scalar multiplication
           return new Vector(a * v.x, a * v.y, a * v.z);
       }

       public static Vector operator +(Vector v1, Vector v2)
       {
           // vector addition
           return new Vector(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
       }

       public static Vector operator -(Vector v1, Vector v2)
       {
           // vector subtraction
           return new Vector(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
       }

       public double Dot(Vector v)
       {
           // dot product
           return x * v.x + y * v.y + z * v.z;
       }

       public Vector Cross(Vector v)
       {
           // cross product
           // NEEDS TESTING!
           double xc = y * v.z - z * v.y;
           double yc = z * v.x - x * v.z;
           double zc = x * v.y - y * v.x;
           return new Vector(xc, yc, zc);
       }

       public double Length()
       {
           return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
       }

   }
}