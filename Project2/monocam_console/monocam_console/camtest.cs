using System.Collections.Generic;
using System;
using System.Text;


namespace monoCAM
{

    public static class camtest
    {
        public static void run(GeoCollection g)
        {
            // find all STL surfaces and runs stlmachine on them
            List<STLSurf> surfs = new List<STLSurf>();
            foreach (Geo s in g.obj_list)
            {
                    if (s.GetType() == typeof(STLSurf))
                    {
                        System.Console.WriteLine("found " + s);
                        System.Console.WriteLine("running CAM algorithm on {0} ...",s);
                        surfs.Add((STLSurf)s);
                    }
            }

            foreach (STLSurf s in surfs)
            {
                camtest.stlmachine(s,g);
            }
            System.Console.Write("Done.\n");

        }

        


        public static void stlmachine(STLSurf s, GeoCollection g)
        {
            List<Point> pointlist=new List<Point>();

            // seems to work...
            // foreach (Geo.Tri t in s.tris)
            //    System.Console.WriteLine("loop1 triangles " + t);
            // System.Console.ReadKey();

            // recalculate normal data
            // create bounding box data
            foreach (Tri t in s.tris)
            {
                t.recalc_normals();  // FIXME why don't new values stick??
                t.calc_bbox();       // FIXME: why doen't bb-data 'stick' ??
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
            // store in a list called pointlist
            double Nx=3;
            double Ny=3;
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
                        pointlist.Add(new Point(x,y,5));
                        // System.Console.WriteLine("x:"+x+" y:"+y);
                        y += dy; // go forward in the y-axis direction 
                        // System.Console.ReadKey();
                    }
                }
                else
                {
                    double y = maxy;
                    for (int m = 0; m < Ny; m++)
                    {
                        pointlist.Add(new Point(x,y,5));
                        //System.Console.WriteLine("x:" + x + " y:" + y);
                        y -= dy; // go backward in the y-axis direction
                        //System.Console.ReadKey();
                    }
                }
                x += dx;
            }


            // drop cutter (i.e. add z-data)

            double R=1,r=0.2; // this is the cutter definition
            Cutter cu = new Cutter(R,r);

            List<Point> drop_points = new List<Point>();
            double redundant = 0; // number of unneccesary calls to drop-cutter
            double checks = 0;    // number of relevant calls

            foreach (Point p in pointlist) // loop through each point
            {
                double? v1 = null,v2=null,v3=null,z_new=null,f=null,e1=null,e2=null,e3=null;
                
                // store the possible z-values in this list
                // the highest one of these should be chosen in the end
                List<double> zlist = new List<double>();
                
                // loop through each triangle
                // FIXME: here a bucketing-scheme or a kd-tree search should
                // be implemented so that only triangles really under the cutter
                // are tested against.
                foreach (Tri t in s.tris)
                {
                    checks++;
                    t.calc_bbox(); // FIXME: why do we have to re-calculate bb-data here??

                    //System.Console.WriteLine("testing triangle" + t);

                    // here are four ways the triangle bounding box can be
                    // outside the cutter bounding box
                    // redundant could be used to test the performance of bucketing/kd-tree
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
                    
                    // test cutter against each vertex
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



                    // test cutter against facet
                    f = DropCutter.FacetTest(cu, p, t);
                    if (f != null)
                    {
                        zlist.Add((double)f);
                    }
                    

                    // test cutter against each edge
                    e1 = DropCutter.EdgeTest(cu, p, t.p[0], t.p[1]);
                    e2 = DropCutter.EdgeTest(cu, p, t.p[1], t.p[2]);
                    e3 = DropCutter.EdgeTest(cu, p, t.p[0], t.p[2]);
                    if (e1 != null)
                        zlist.Add((double)e1);
                    if (e2 != null)
                        zlist.Add((double)e2);
                    if (e3 != null)
                        zlist.Add((double)e3);

                    // now we have some suggestions for z in zlist
                    // by sorting it we get the highest one at the end of the list
                    zlist.Sort();
                    
                    // if there's anything in the list, return the last element
                    if (zlist.Count > 0)
                        z_new = zlist[zlist.Count-1];

                } // end triangle loop

                // we've gone through all triangles for this XY-location
                // if we found a z-value, let's add the valid cutter location
                // to a list drop_points
                if (z_new != null)
                {
                    drop_points.Add(new Point(p.x, p.y, (double)z_new));
                }


            } // end point-list loop


            // print some statistics:
            System.Console.WriteLine("checked: "+ checks + " redundant: " + redundant);
            double fraction=(100*(double)(checks-redundant)/(double)checks);
            System.Console.WriteLine("relevant: "+(checks-redundant) + "  ("+fraction.ToString("N3")+"%)");

           
            

            // FIXME: now a toolpath object should be created 
            // that has rapids/feeds according to the points calculated above
            int i = 1;
            Point p0=new Point();
            foreach (Point p in drop_points)
            {
                
                if (i == 1) // first move
                {
                    p0 = new Point(p.x, p.y, 10);
                    Line l = new Line(p0, p);
                    g.add(l); //  ADD geometry to toolpath
                    p0 = p;
                }
                else  // don't do anything for last move
                {
                    Line l = new Line(p0, p);
                    g.add(l);  // ADD geometry to toolpath
                    p0 = p;
                }
                i++;
            }

        }



        // test for bounding box function
        public static void bbox_test(GeoCollection g)
        {
            // draw a triangle
            Point p1 = new Point(0.1, 0.1, 0);
            Point p2 = new Point(0.8, 0.3, 0);
            Point p3 = new Point(0.2, 0.6, 0);
            Line l1 = new Line(p1, p2);
            Line l2 = new Line(p1, p3);
            Line l3 = new Line(p3, p2);
            g.add(l1);
            g.add(l2);
            g.add(l3);

            // create triangle and calculate bounding box
            Tri t = new Tri(p1, p2, p3);
            t.calc_bbox();
            Point a = new Point(t.bb.minx, t.bb.miny, 0);
            Point b = new Point(t.bb.maxx, t.bb.miny, 0);
            Point c = new Point(t.bb.maxx, t.bb.maxy, 0);
            Point d = new Point(t.bb.minx, t.bb.maxy, 0);
            Line h1 = new Line(a, b);
            Line h2 = new Line(b, c);
            Line h3 = new Line(c, d);
            Line h4 = new Line(d, a);
            g.add(h1);
            g.add(h2);
            g.add(h3);
            g.add(h4);
        }

        // isinside unit test
        public static void isinside_test(GeoCollection g)
        {
            Random r = new Random();

            // generate lots of random points
            int N = 100;
            List<Point> points = new List<Point>();
            for (int n = 0; n < N; n++)
            {
                Point p = new Point(0.001 * (float)r.Next(0, 1000), 0.001 * (float)r.Next(0, 1000), 0);
                points.Add(p);
            }

            // draw a triangle
            Point p1 = new Point(0.1, 0.1, 0);
            Point p2 = new Point(0.8, 0.1, 0);
            Point p3 = new Point(0.2, 0.6, 0);
            Line l1 = new Line(p1, p2);
            Line l2 = new Line(p1, p3);
            Line l3 = new Line(p3, p2);
            g.add(l1);
            g.add(l2);
            g.add(l3);
            // p.color = System.Drawing.Color.Aqua;

            // draw points
            Tri t = new Tri(p2, p3, p1);
            foreach (Point p in points)
            {
                /*
                if (DropCutter.isinside(t,p.p))
                   p.color = System.Drawing.Color.Aqua;
                else
                    p.color = System.Drawing.Color.Red;
                */

                g.add(p);
            }
            
        }

    } // end Drop Cutter class


}