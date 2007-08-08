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
                        System.Console.Write("running CAM algorithm...");
                        surfs.Add((STLSurf)s);
                    }
            }

            foreach (STLSurf s in surfs)
            {
                camtest.stlmachine(g, s);
            }
            System.Console.Write("Done.\n");

        }

        


        public static void stlmachine(GLWindow g, STLSurf s)
        {
            List<Geo.Point> pointlist=new List<Geo.Point>();

            // seems to work...
            // foreach (Geo.Tri t in s.tris)
            //    System.Console.WriteLine("loop1 triangles " + t);
            // System.Console.ReadKey();

            // recalculate normal data
            // create bounding box data
            foreach (Geo.Tri t in s.tris)
            {
                t.recalc_normals(); // FIXME why don't new values stick??
                t.calc_bbox(); // FIXME: why doen't bb-data 'stick' ??
            }

            /*
            // FIXME: if we check bb-data here it is gone!!(??)
            foreach (Geo.Tri t in s.tris)
            {
                System.Console.WriteLine("loop2 triangles " + t);
                System.Console.WriteLine("loop2 direct maxx" + t.bb.maxx + " minx:" + t.bb.minx);
            }
            System.Console.ReadKey();
            */

            // find bounding box (this should probably be done in the STLSurf class?)
            double minx = 0, maxx = 10, miny = 0, maxy = 10;

            // generate XY pattern (a general zigzag-strategy, needed also for pocketing)
            double Nx=50;
            double Ny=50;
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
            double redundant = 0;
            double checks = 0;
            foreach (Geo.Point p in pointlist)
            {
            

                double? v1 = null,v2=null,v3=null,z_new=null,f=null,e1=null,e2=null,e3=null;
                List<double> zlist = new List<double>();
                
                foreach (Geo.Tri t in s.tris)
                {
                    checks++;
                    t.calc_bbox(); // why do we have to re-calculate bb-data here??

                    //System.Console.WriteLine("testing triangle" + t);
                    if (t.bb.minx > (p.x + cu.R))
                    {
                        redundant++;
                        continue;
                    }
                    else if (t.bb.maxx < (p.x - cu.R))
                    {   
                        redundant++;
                        continue;
                    }
                    if (t.bb.miny > (p.y + cu.R))
                    {
                        redundant++;
                        continue;
                    }
                    if (t.bb.maxy < (p.y - cu.R))
                    {
                        redundant++;
                        continue;
                    }



                    
                    
                    v1 = DropCutter.VertexTest(cu, p, t.p[0]);
                    v2 = DropCutter.VertexTest(cu, p, t.p[1]);
                    v3 = DropCutter.VertexTest(cu, p, t.p[2]);
                    if (v2 != null)
                    {
                        zlist.Add((double)v2);
                    }
                    if (v1 != null)
                    {
                        zlist.Add((double)v1);
                    }
                    if (v3 != null)
                    {
                        zlist.Add((double)v3);
                    }



                    
                    f = DropCutter.FacetTest(cu, p, t);
                    if (f != null)
                    {
                        zlist.Add((double)f);
                    }
                    

                    e1 = DropCutter.EdgeTest(cu, p, t.p[0], t.p[1]);
                    e2 = DropCutter.EdgeTest(cu, p, t.p[1], t.p[2]);
                    e3 = DropCutter.EdgeTest(cu, p, t.p[0], t.p[2]);

                    if (e1 != null)
                        zlist.Add((double)e1);
                    if (e2 != null)
                        zlist.Add((double)e2);
                    if (e3 != null)
                        zlist.Add((double)e3);
                     



                    /*
                    if (zlist.Count > 1)
                    {
                        System.Console.Write("Before: ");
                        foreach (double d in zlist)
                            System.Console.Write(d.ToString() + " ");
                        System.Console.Write("\n");
                    }
                     */

                    zlist.Sort();
                    /*
                    if (zlist.Count > 2)
                    {
                        System.Console.Write("After: ");
                        foreach (double d in zlist)
                            System.Console.Write(d.ToString() + " ");
                        System.Console.Write("\n");
                    }
                     */
                    // System.Console.Write("Sorted: ");
                    // foreach (double d in zlist)
                    //    System.Console.Write(d.ToString() + " ");
                    // System.Console.Write("\n");

                    if (zlist.Count > 0)
                        z_new = zlist[zlist.Count-1];
                    /*
                     if (zlist.Count > 1)
                        System.Console.WriteLine("chosen: " + z_new);
                     */
                    // System.Console.ReadKey();


                } // end triangle loop

                if (z_new != null)
                {
                    drop_points.Add(new Geo.Point(p.x, p.y, (double)z_new));
                }


            } // end point-list loop

            System.Console.WriteLine("checked: "+ checks + " redundant: " + redundant);
            System.Console.WriteLine("relevant: "+(checks-redundant) + "  ("+100*(double)(checks-redundant)/(double)checks+"%)");

            // check to see that STL has not changed
            

            // display drop-points
            int i = 1;
            Geo.Point p0=new Geo.Point();
            foreach (Geo.Point p in drop_points)
            {
                
                if (i == 1) // first move
                {
                    p0 = new Geo.Point(p.x, p.y, 10);
                    GeoLine l = new GeoLine(p0, p);
                    l.color = System.Drawing.Color.Yellow;
                    g.addGeom(l);
                    p0 = p;
                }
                else  // don't do anything for last move
                {
                    GeoLine l = new GeoLine(p0, p);
                    l.color = System.Drawing.Color.Magenta;
                    g.addGeom(l);
                    p0 = p;
                }
                i++;
                

                /*
                GeoPoint pg = new GeoPoint(p);
                pg.color = System.Drawing.Color.Aqua;
                g.addGeom(pg); 
                 */
                
            }


            // display zigzag and points
            /*
            i = 1;
            foreach (Geo.Point p in pointlist)
            {
                if (i == 1) 
                {
                    p0 = new Geo.Point(p.x, p.y, 10);
                    GeoLine l = new GeoLine(p0, p);
                    l.color = System.Drawing.Color.Yellow;
                    g.addGeom(l);
                    p0 = p;
                }
                else  
                {
                    GeoLine l = new GeoLine(p0, p);
                    l.color = System.Drawing.Color.Cyan;
                    g.addGeom(l);
                    p0 = p;
                }
                i++;
            }
            */


            // dummy test:
            /*
            foreach (Geo.Tri t in s.tris)
            {
                GeoPoint p = new GeoPoint(t.p[0].x, t.p[0].y, t.p[0].z);
                pointlist.Add(p);
            }
            */


        }

        // a function for displaying the normals in an STL file
        public static void draw_stl_normals(GLWindow g, STLSurf s)
        {
            // draw triangle normas
            foreach (Geo.Tri t in s.tris)
            {
                // from STL file
                Geo.Point p1 = new Geo.Point(t.p[0].x, t.p[0].y, t.p[0].z);
                Geo.Point p2 = new Geo.Point(t.p[0].x + t.n.x, t.p[0].y + t.n.y, t.p[0].z + t.n.z);
                GeoLine l = new GeoLine(p1, p2);
                l.color = System.Drawing.Color.Green;
                g.addGeom(l);

                // calculate yourself:
                Vector v1 = new Vector(t.p[0].x - t.p[1].x, t.p[0].y - t.p[1].y, t.p[0].z - t.p[1].z);
                Vector v2 = new Vector(t.p[0].x - t.p[2].x, t.p[0].y - t.p[2].y, t.p[0].z - t.p[2].z);
                Vector n;
                n = v1.Cross(v2); // the normal is in the direction of the cross product between the edge vectors
                n = (1 / n.Length()) * n; // normalize to length==1
                p1 = new Geo.Point(t.p[0].x, t.p[0].y, t.p[0].z);
                p2 = new Geo.Point(t.p[0].x + n.x, t.p[0].y + n.y, t.p[0].z + n.z);
                l = new GeoLine(p1, p2);
                l.color = System.Drawing.Color.Blue;
                g.addGeom(l);

            }
        }

        // test for bounding box function
        public static void bbox_test(GLWindow g)
        {
            // draw a triangle
            Geo.Point p1 = new Geo.Point(0.1, 0.1, 0);
            Geo.Point p2 = new Geo.Point(0.8, 0.3, 0);
            Geo.Point p3 = new Geo.Point(0.2, 0.6, 0);
            GeoLine l1 = new GeoLine(p1, p2);
            GeoLine l2 = new GeoLine(p1, p3);
            GeoLine l3 = new GeoLine(p3, p2);
            l1.color = System.Drawing.Color.RoyalBlue;
            l2.color = System.Drawing.Color.RoyalBlue;
            l3.color = System.Drawing.Color.RoyalBlue;
            g.addGeom(l1);
            g.addGeom(l2);
            g.addGeom(l3);

            // create triangle and calculate bounding box
            Geo.Tri t = new Geo.Tri(p1, p2, p3);
            t.calc_bbox();
            Geo.Point a = new Geo.Point(t.bb.minx, t.bb.miny, 0);
            Geo.Point b = new Geo.Point(t.bb.maxx, t.bb.miny, 0);
            Geo.Point c = new Geo.Point(t.bb.maxx, t.bb.maxy, 0);
            Geo.Point d = new Geo.Point(t.bb.minx, t.bb.maxy, 0);
            GeoLine h1 = new GeoLine(a, b);
            GeoLine h2 = new GeoLine(b, c);
            GeoLine h3 = new GeoLine(c, d);
            GeoLine h4 = new GeoLine(d, a);
            h1.color = System.Drawing.Color.Red;
            h2.color = System.Drawing.Color.Red;
            h3.color = System.Drawing.Color.Red;
            h4.color = System.Drawing.Color.Red;
            g.addGeom(h1);
            g.addGeom(h2);
            g.addGeom(h3);
            g.addGeom(h4);

            // check that the points are OK
            GeoPoint v1 = new GeoPoint(t.bb.minx, t.bb.miny, 0);
            v1.color = System.Drawing.Color.Green;
            g.addGeom(v1);
            GeoPoint v2 = new GeoPoint(t.bb.maxx, t.bb.maxy, 0);
            v2.color = System.Drawing.Color.Azure;
            g.addGeom(v2);


        }

        // testing that the isinside function works OK
        public static void isinside_test(GLWindow g)
        {
            Random r = new Random();

            // generate lots of random points
            int N = 100;
            List<GeoPoint> points = new List<GeoPoint>();
            for (int n = 0; n < N; n++)
            {
                GeoPoint p = new GeoPoint(0.001 * (float)r.Next(0, 1000), 0.001 * (float)r.Next(0, 1000), 0);
                points.Add(p);
            }
            

            // draw a triangle
            Geo.Point p1 = new Geo.Point(0.1, 0.1, 0);
            Geo.Point p2 = new Geo.Point(0.8, 0.1, 0);
            Geo.Point p3 = new Geo.Point(0.2, 0.6, 0);
            GeoLine l1 = new GeoLine(p1, p2);
            GeoLine l2 = new GeoLine(p1, p3);
            GeoLine l3 = new GeoLine(p3, p2);
            l1.color = System.Drawing.Color.RoyalBlue;
            l2.color = System.Drawing.Color.RoyalBlue;
            l3.color = System.Drawing.Color.RoyalBlue;
            g.addGeom(l1);
            g.addGeom(l2);
            g.addGeom(l3);
            // p.color = System.Drawing.Color.Aqua;

            // draw points
            Geo.Tri t = new Geo.Tri(p2, p3, p1);
            foreach (GeoPoint p in points)
            {
                if (DropCutter.isinside(t,p.p))
                    p.color = System.Drawing.Color.Aqua;
                else
                    p.color = System.Drawing.Color.Red;

                g.addGeom(p);
            }
            
        }

    } // end Drop Cutter class


}