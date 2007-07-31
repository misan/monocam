using System.Collections.Generic;

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
            List<GeoPoint> pointlist=new List<GeoPoint>();


            // find bounding box

            // generate XY pattern

            // drop cutter + generate toolpath


            // dummy test:
            foreach (Geo.Tri t in s.tris)
            {
                GeoPoint p = new GeoPoint(t.p[0].x, t.p[0].y, t.p[0].z);
                pointlist.Add(p);
            }
            foreach (GeoPoint p in pointlist)
                g.addGeom(p);
        }


    }


}