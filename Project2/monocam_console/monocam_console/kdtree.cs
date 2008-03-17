using System.Collections.Generic;
using System;
using System.Text;

// implement a kdtree for orthogonal range searching of triangles
// http://en.wikipedia.org/wiki/Kd-tree
// this is also briefly explained by Yau et al. in http://dx.doi.org/10.1080/00207540410001671651


namespace monoCAM
{

    public class kd_node
    {
        public int dim;          // dimension for cut
        public double cutval;    // cut value
        public kd_node hi, lo;   // child nodes
        public List<Tri> tris;   // the triangles contained in a bucket node

        public kd_node()
        { }

        public kd_node(int d, double cv, kd_node hi_c, kd_node lo_c, List<Tri> t_list)
        {
            
            dim = d;
            cutval = cv;
            hi = hi_c;
            lo = lo_c;
            tris = t_list;
        }



    }

    public class kdtree
    {
        
        static public kd_node build_kdtree(List<Tri> tris)
        {
            // given a list of triangles, build a kd-tree

            // this should never happen...
            if (tris.Count == 0)
            {
                System.Console.WriteLine("kdtree build ERROR (tris.Count==0)");
                return new kd_node(0, 0, null, null, tris);
            }
            
            // if the triangles in this node are contained within
            // a rectangle smaller than XX, 
            // OR if only one triangle remains,
            // return a bucket node
            if (tris.Count<=10)
            {
                kd_node bucket_node = new kd_node(0, 0, null, null, tris);
                System.Console.WriteLine("returning bucket node with {0} triangles", tris.Count);
                return bucket_node;
            }

            sp spr = kdtree.spread(tris);
            System.Console.WriteLine("cuttting along dim={0} spread={1}", spr.d, spr.val);

            // if the max spread is 0, return a bucket node (?when does this happen?)
            if (spr.val < 0.2)
            {
                kd_node bucket_node = new kd_node(0, 0, null, null, tris);
                System.Console.WriteLine("returning bucket node with {0} triangles", tris.Count);
                return bucket_node;
            }
            
            //return null;

            // otherwise, select at which triangle to cut
            double cv=spr.start+spr.val/2;
            System.Console.WriteLine("cutvalue={0}",cv);
            

            // build lists of triangles lower and higher than cutval
            List<Tri> tris_hi = new List<Tri>();
            List<Tri> tris_lo = new List<Tri>();
            foreach (Tri t in tris)
            {
                // choose which triangles go into which list here.
                if (spr.d == 0)
                {
                    if (t.bb.maxx > cv)
                        tris_hi.Add(t);
                    else
                        tris_lo.Add(t);
                }
                else if (spr.d == 1)
                {
                    if (t.bb.minx > cv)
                        tris_hi.Add(t);
                    else
                        tris_lo.Add(t);
                }
                else if (spr.d == 2)
                {
                    if (t.bb.maxy > cv)
                        tris_hi.Add(t);
                    else
                        tris_lo.Add(t);
                }
                else if (spr.d == 3)
                {
                    if (t.bb.miny > cv)
                        tris_hi.Add(t);
                    else
                        tris_lo.Add(t);
                }
            
            

            }


            kd_node node = new kd_node();
            node.dim = spr.d;
            node.cutval = cv;
            System.Console.WriteLine("hi_count={0}  lo_count={1}", tris_hi.Count, tris_lo.Count);
            // System.Console.ReadKey();

            node.hi = build_kdtree(tris_hi);
            node.lo = build_kdtree(tris_lo);

            return node;
        }



        public struct sp
        {
            public int d;
            public double val;
            public double start;

            public sp(int dim, double v, double s)
            {
                d = dim;
                val = v;
                start = s;
            }
        }

        private static int sp_comp(sp x, sp y)
        {


            if (x.val > y.val)
                return 1;
            if (y.val > x.val)
                return -1;
            else
                return 0;
        }

        static public sp spread(List<Tri> tris)
        {
            // find the maximum 'extent' of triangles in list tris along dimension d
            double max_xplus=0, min_xplus=0, max_xminus=0, min_xminus=0;
            double max_yplus = 0, min_yplus = 0, max_yminus = 0, min_yminus = 0;

            double spr_xplus = 0, spr_xminus = 0, spr_yplus = 0, spr_yminus = 0;

            if (tris.Count == 0)
                return new sp(0,0,0);
            else
            {
                System.Console.WriteLine("calculating spread for {0} triangles", tris.Count);
                int n = 1;
                foreach (Tri t in tris)
                {
                    
                    if (n == 1)
                    {
                        // on the first iteration assing all values from the first triangle
                        max_xplus  = t.bb.maxx;
                        min_xplus  = t.bb.maxx;
                        max_xminus = t.bb.minx;
                        min_xminus = t.bb.minx;

                        max_yplus  = t.bb.maxy;
                        min_yplus  = t.bb.maxy;
                        max_yminus = t.bb.miny;
                        min_yminus = t.bb.miny;
                    }
                    else
                    {
                        //System.Console.WriteLine("minx={0} maxx={1} miny={2} maxy={3}", t.bb.minx, t.bb.maxx, t.bb.miny, t.bb.maxy);
                        // compute spread in xplus
                        if (t.bb.maxx > max_xplus)
                            max_xplus = t.bb.maxx;
                        if (t.bb.maxx < min_xplus)
                            min_xplus = t.bb.maxx;

                        if (spr_xplus < (max_xplus - min_xplus))
                            spr_xplus = max_xplus - min_xplus;

                        // compute spread in xminus
                        if (t.bb.minx > max_xminus)
                            max_xminus = t.bb.minx;
                        if (t.bb.minx < min_xminus)
                            min_xminus = t.bb.minx;

                        if (spr_xminus< (max_xminus - min_xminus))
                            spr_xminus = max_xminus - min_xminus;

                        // compute spread in yplus
                        if (t.bb.maxy > max_yplus)
                            max_yplus = t.bb.maxy;
                        if (t.bb.maxy < min_yplus)
                            min_yplus = t.bb.maxy;

                        if (spr_yplus < (max_yplus - min_yplus))
                            spr_yplus = max_yplus - min_yplus;

                        // compute spread in yminus
                        if (t.bb.miny > max_yminus)
                            max_yminus = t.bb.miny;
                        if (t.bb.miny < min_yminus)
                            min_yminus = t.bb.miny;

                        if (spr_yminus < (max_yminus - min_yminus))
                            spr_yminus = max_yminus - min_yminus;

                    }
                    n = n + 1;
                }
                
                System.Console.WriteLine("spr_xplus={0}", spr_xplus);
                System.Console.WriteLine("spr_xminus={0}", spr_xminus);
                System.Console.WriteLine("spr_yplus={0}", spr_yplus);
                System.Console.WriteLine("spr_yminus={0}", spr_yminus);

                // find max spread and return dimension and cut value
                List<sp> spreads = new List<sp>();
                spreads.Add(new sp(0, spr_xplus,min_xplus));
                spreads.Add(new sp(1, spr_xminus,min_xminus));
                spreads.Add(new sp(2, spr_yplus,min_yplus));
                spreads.Add(new sp(3, spr_yminus,min_yminus));
                spreads.Sort(sp_comp);

 
                System.Console.WriteLine("returning dim={0} spr={1}", spreads[spreads.Count - 1].d, spreads[spreads.Count - 1].val);
                return spreads[spreads.Count-1];
            }
            

        } // end spread()


        public static void PrintKdtree(kd_node root)
        {
            if (root.tris == null)
                return;

            if (root.tris.Count > 0)
                System.Console.WriteLine("bucket node with {0} tris", root.tris.Count);

            if (root.hi != null)
                PrintKdtree(root.hi);
        }

    }
}