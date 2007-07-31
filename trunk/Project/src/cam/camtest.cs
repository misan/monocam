using System.Collections.Generic;
using System;
using System.Text;


namespace monoCAM
{

    public static class camtest
    {
        public static void run(GLWindow g)
        {
            // finds all STL surfaces and runs stlmachine on them

             List<STLSurf> surfs = new List<STLSurf>();
            foreach (Geo s in g.geom.obj_list)
            {

                    if (s.GetType() == typeof(STLSurf))
                    {
                        System.Console.WriteLine("found stl surf " + s);
                        System.Console.WriteLine("running CAM algorithm");
                        surfs.Add((STLSurf)s);
                    }
            }

            foreach (STLSurf s in surfs)
            {
                camtest.stlmachine(g, s);
            }


        }


        public static void stlmachine(GLWindow g, STLSurf s)
        {
            List<Geo.Point> pointlist=new List<Geo.Point>();


            // find bounding box (this should probably be done in the STLSurf class?)
            double minx = 0, maxx = 10, miny = 0, maxy = 10;

            // generate XY pattern (a general zigzag-strategy, needed also for pocketing)
            double Nx=10;
            double Ny=10;
            double dx=(maxx-minx)/(double)(Nx-1);
            double dy = (maxy - miny) / (double)(Ny-1);
            double x = minx;
            
            for (int n = 0; n < Nx; n++)
            {
                if (n%2==0)
                {
                    double y = miny;
                    for (int m = 0; m < Ny; m++)
                    {
                        pointlist.Add(new Geo.Point(x,y,5));
                        // System.Console.WriteLine("x:"+x+" y:"+y);
                        y += dy;
                        // System.Console.ReadKey();
                    }
                }
                else
                {
                    double y = maxy;
                    for (int m = 0; m < Ny; m++)
                    {
                        pointlist.Add(new Geo.Point(x,y,5));
                        //System.Console.WriteLine("x:" + x + " y:" + y);
                        y -= dy;
                        //System.Console.ReadKey();
                    }
                }
                x += dx;
            }


            // drop cutter (i.e. add z-data)
            double R=1,r=0.2;
            Cutter cu = new Cutter(R,r);
            List<Geo.Point> drop_points = new List<Geo.Point>();

            foreach (Geo.Point p in pointlist)
            {
                double? v1 = null,v2=null,v3=null,z_new=null,f=null;
                foreach (Geo.Tri t in s.tris)
                {

                    v1 = DropCutter.VertexTest(cu, p, t.p[0]);
                    v2 = DropCutter.VertexTest(cu, p, t.p[1]);
                    v3 = DropCutter.VertexTest(cu, p, t.p[2]);

                    if (v1 != null)
                    {
                        if (z_new == null)
                            z_new = v1;
                        else if (z_new < v1)
                            z_new = v1;
                    }
                    if (v2 != null)
                    {
                        if (v2 > z_new)
                            z_new = v2;
                    }
                    if (v3 != null)
                    {
                        if (v3 > z_new)
                            z_new = v3;
                    }

                    f = DropCutter.FacetTest(cu, p, t);
                    if (f != null)
                    {
                        if (f > z_new)
                            z_new = f;
                    }


                }

                if (z_new != null)
                {
                    drop_points.Add(new Geo.Point(p.x, p.y, (double)z_new));
                }
            }

            // display drop-points
            foreach (Geo.Point p in drop_points)
            {
                GeoPoint pg = new GeoPoint(p);
                g.addGeom(pg); 
            }


            // display zigzag and points
            int i = 1;
            Geo.Point p0=new Geo.Point();
            foreach (Geo.Point p in pointlist)
            {
                if (i == 1) // first move
                {
                    p0 = new Geo.Point(p.x, p.y, 10);
                    GeoLine l = new GeoLine(p0, p);
                    g.addGeom(l);
                    p0 = p;
                }
                else  // don't do anything for last move
                {
                    GeoLine l = new GeoLine(p0, p);
                    g.addGeom(l);
                    p0 = p;
                }
                GeoPoint p2 = new GeoPoint(p);
                g.addGeom(p2);  
                i++;
            }



            // dummy test:
            /*
            foreach (Geo.Tri t in s.tris)
            {
                GeoPoint p = new GeoPoint(t.p[0].x, t.p[0].y, t.p[0].z);
                pointlist.Add(p);
            }
            */


        }


    }


}