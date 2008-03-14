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
   public class GeoCollection
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

       public struct Bbox // bounding box for triangles and cutter
       {
           public double maxx, minx, maxy, miny;
       }



       public Geo()
       {       
           // default constructor
       }
       
   }

   /* ****************** END of base class definition **********/




   public class Point : Geo
   {
       // experimental implementation of a Point
       public double x;
       public double y;
       public double z;

       public Point()
       {
           SetPos(0, 0, 0);

       }

       public Point(double x, double y, double z)
           // constructor with literal initial values
       {
           // GeoPoint();
           SetPos(x, y, z);
      
       }

       public Point(Point p_in)
           // constructor with another point as initial values
       {
           x = p_in.x;
           y = p_in.y;
           z = p_in.z;
     
       }     

 

       public void SetPos(double X, double Y, double Z)
       {
           x = X;
           y = Y;
           z = Z;
       }

       public override string ToString()
       {
           return "Point(" + x + " , " + y + " , " + z + ")";
       }

       public static Point operator -(Point p1, Point p2)
       {
           return new Point(p1.x - p2.x, p1.y - p2.y, p1.z - p2.z);
       }

   }



   class Line : Geo
   {
       // experimental Line class. May be used initially for CAM output
       public Point start, end;
       public Line(Point s, Point e)
       {
           start = s;
           end = e;
       }

       public override string ToString()
       {
           return "Line from " + start + " to " + end;
       }

       public double Length()
       {
           return Math.Sqrt(Math.Pow(start.x - end.x, 2) + Math.Pow(start.y - end.y, 2) + Math.Pow(start.z - end.z, 2));
       }

   }


   public class Tri : Geo 
   {

       public Point[] p;
       public Vector n;
       public Bbox bb;

       public Tri(Point P1, Point P2, Point P3)
       {
           p = new Point[3];
           bb = new Bbox();
           p[0] = P1;
           p[1] = P2;
           p[2] = P3;
           // if normal is not given, calculate it here.
           Vector v1 = new Vector(p[0].x - p[1].x, p[0].y - p[1].y, p[0].z - p[1].z);
           Vector v2 = new Vector(p[0].x - p[2].x, p[0].y - p[2].y, p[0].z - p[2].z);
           n = v1.Cross(v2); // the normal is in the direction of the cross product between the edge vectors
           n = (1 / n.Length()) * n; // normalize to length==1
       }

       public Tri()
       {
           p = new Point[3];
           bb = new Bbox();
           p[0] = new Point(0,0,0);
           p[1] = new Point(0, 0, 0);
           p[2] = new Point(0, 0, 0);
           // if normal is not given, calculate it here.

           n = null; // normalize to length==1
       }

       public Tri(Point P1, Point P2, Point P3, Vector N)
       {
           p = new Point[3];
           bb = new Bbox();
           p[0] = P1;
           p[1] = P2;
           p[2] = P3;
           n = N;
       }

       public Tri(Vector N)
       {
           // a strange constructor indeed...
           p = new Point[3];
           bb = new Bbox();
           p[0] = new Point();
           p[1] = new Point();
           p[2] = new Point();
           n = N;
       }

       public void recalc_normals()
       {
           // normal data from STL files is usually junk, so recalculate:
           Vector v1 = new Vector(p[0].x - p[1].x, p[0].y - p[1].y, p[0].z - p[1].z);
           Vector v2 = new Vector(p[0].x - p[2].x, p[0].y - p[2].y, p[0].z - p[2].z);
           n = v1.Cross(v2); // the normal is in the direction of the cross product between the edge vectors
           n = (1 / n.Length()) * n; // normalize to length==1
       }

       public void calc_bbox()
       {
           // find the bounxing box in the XY plane for the triangle
           bb.maxx = p[0].x;
           bb.minx = p[0].x;
           bb.maxy = p[0].y;
           bb.miny = p[0].y;

           if (p[1].x < bb.minx)
               bb.minx = p[1].x;
           if (p[2].x < bb.minx)
               bb.minx = p[2].x;

           if (p[1].x > bb.maxx)
               bb.maxx = p[1].x;
           if (p[2].x > bb.maxx)
               bb.maxx = p[2].x;

           if (p[1].y < bb.miny)
               bb.miny = p[1].y;
           if (p[2].y < bb.miny)
               bb.miny = p[2].y;

           if (p[1].y > bb.maxy)
               bb.maxy = p[1].y;
           if (p[2].y > bb.maxy)
               bb.maxy = p[2].y;

           // System.Console.WriteLine("minx={0} maxx={1} miny={2} maxy={3}",bb.minx,bb.maxx,bb.miny,bb.maxy);
           // System.Console.ReadKey();

       }

       public override string ToString()
       {
           return "bb.minx=" + bb.minx.ToString() + " bb.maxx=" + bb.maxx.ToString();
       }

   } // end Tri struct


    public class ToolPath : Geo
    {
        public List<Point> points;
        public ToolPath()
        {
            points = new List<Point>();
        }

    }

   public class STLSurf : Geo
   {
       // experimental STL surface class
       public List<Tri> tris;  // a list that holds the vertices.
       // the length of the list should always be a multiple of three!


       public STLSurf()
       {
           tris = new List<Tri>();
       }

       public void AddTriangle(Tri t)
       {
           // add one triangle to the surface
           tris.Add(t);
       }

       public override string ToString()
       {
           return "STLSurf with " + tris.Count + " triangles";
       }

   } // end STLSurf class

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

       public Vector Cross(Point p)
       {
           // cross product with point
           // point coordinates are treated as vector coordinates
           double xc = y * p.z - z * p.y;
           double yc = z * p.x - x * p.z;
           double zc = x * p.y - y * p.x;
           return new Vector(xc, yc, zc);
       }
       public void normalize()
       {
           double l = Math.Sqrt(Math.Pow(x,2)+Math.Pow(y,2)+Math.Pow(z,2));
           x = x / l;
           y = y / l;
           z = z / l;
       }

       public double Length()
       {
           return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
       }

   }
}