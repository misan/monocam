using System.Collections.Generic;
using System;
using System.Text;

// implement a kdtree for orthogonal range searching of triangles
// http://en.wikipedia.org/wiki/Kd-tree
// this is also briefly explained by Yau et al. in http://dx.doi.org/10.1080/00207540410001671651


namespace monoCAM
{
    public enum cutdim 
    {
        PLUS_X, MINUS_X, PLUS_Y, MINUS_Y
    }

    public class kd_node
    {
        public bool bucket;      // a bucket node contains triangles and has no children
        public cutdim dim;       // dimension for cut
        public double cutval;    // cut value
        public kd_node hi, lo;   // child nodes
        public List<Tri> tris;   // the triangles contained in a bucket node

        public kd_node()
        { }

        public kd_node(cutdim d, double cv, kd_node hi_c, kd_node lo_c, bool b, List<Tri> t_list)
        {
            bucket = b;
            dim = d;
            cutval = cv;
            hi = hi_c;
            lo = lo_c;
            tris = t_list;
        }



    }

    public class kdtree
    {
        
        static public kd_node build_kdtree(List<Tri> tris, cutdim d)
        {
            // given a list of trianges, build a kd-tree

            // this should never happen...
            if (tris.Count == 0)
                System.Console.WriteLine("kdtree build ERROR (tris.Count==0)");
            
            // if the triangles in this node are contained within
            // a rectangle smaller than XX, 
            // OR if only one triangle remains,
            // return a bucket node
            if ((kdtree.spread(tris, d) == 0.0) || (tris.Count==1))
            {
                kd_node bucket_node = new kd_node(d, 0, null, null, true, tris);
                return bucket_node;
            }

            // otherwise, select at which triangle to cut
            double cv=0;

            // build lists of triangles lower and higher than cutval
            List<Tri> tris_hi = new List<Tri>();
            List<Tri> tris_lo = new List<Tri>();


            kd_node node = new kd_node();
            node.dim = d;
            node.cutval = cv;
            node.hi = build_kdtree(tris_hi,d+1);
            node.lo = build_kdtree(tris_lo,d+1);

            return node;
        }

        static public double spread(List<Tri> tris, cutdim d)
        {
            // find the maximum 'extent' of triangles in list tris along dimension d
            double spr = 0, min = 0, max = 0;
            if (tris.Count == 0)
                return -1;
            else
            {

                foreach (Tri t in tris)
                {
                    System.Console.WriteLine("bbox:" + t.bb.minx + "," + t.bb.maxx + "," + t.bb.miny + "," + t.bb.maxy);
                }
            }
            return spr;

        }

    }
}