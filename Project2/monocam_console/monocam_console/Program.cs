using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace monoCAM
{
    class Program
    {
        static void Main(string[] args)
        {
            GeoCollection g = new GeoCollection();
            System.Console.WriteLine("MonoCAM 2008 Mar 03");

            // load an STL file
            System.String FileName = "Demo1.stl";
            System.Console.WriteLine("opening STL file {0}",FileName);
            System.IO.StreamReader rdr = file_open(FileName);
            STLSurf s = null;
            if (rdr != null)
                s = STL.Load(rdr); 
            if (s != null)
                g.add(s);
            else
                System.Console.WriteLine("loading STL file failed. no geometry created.");


            WriteGeoColl(g);

            // try a cam operation
            //camtest.run(g);

            //WriteGeoColl(g);

            // test kd-tree
            // kdtree.spread(s.tris, cutdim.MINUS_X);


            Stopwatch st = new Stopwatch();
            Console.WriteLine("Building kd-tree. Stopwatch start");
            st.Start();
            kd_node root;
            root = kdtree.build_kdtree(s.tris);
            st.Stop();
            Console.WriteLine("Elapsed = {0}", st.Elapsed.ToString());

            //kdtree.PrintKdtree(root);

            Cutter c = new Cutter(0.1, 0);
            Point p = new Point(0, 0, 0);
            List<Tri> tris = new List<Tri>();
            kdtree.search_kdtree(tris, p, c, root);
            System.Console.WriteLine("found {0} triangles!", tris.Count);
            System.Console.WriteLine("ns={0}",kdtree.ns);
            if (tris.Count <= 10)
            {
                foreach (Tri t in tris)
                {
                    Console.WriteLine("x: " + t.bb.minx + " / " + t.bb.maxx + " " + (p.x - c.R) + "to" + (p.x + c.R));
                    Console.WriteLine("y: " + t.bb.miny + " / " + t.bb.maxy + " " + (p.y - c.R) + "to" + (p.y + c.R));
                }
            }
            
            // display the kd_tree

            //kdtree.PrintKdtree(root);

            // wait for user to end program
            System.Console.WriteLine("Press any key to end");
            System.Console.ReadKey();
        }

        static public void WriteGeoColl(GeoCollection g)
        {
            System.Console.WriteLine(g);
            int n = 0;
            foreach (Geo go in g.obj_list)
            {
                System.Console.Write("({0}) ", n);
                System.Console.WriteLine(go);
                n = n + 1;
            }
        }


        static public System.IO.StreamReader file_open(System.String FileName)
        {
            // opens a StreamReader object for reading in a file
            System.IO.FileStream strm;
            try
            {
                strm = new System.IO.FileStream(FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                System.IO.StreamReader rdr = new System.IO.StreamReader(strm);
                return rdr;
            }
            catch (Exception)
            {
                System.Console.WriteLine("Error opening file {0}",FileName);
                return null;
            }
        }






    }
}
