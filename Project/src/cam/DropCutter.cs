//this file is the correct one
using System;
using System.Collections.Generic;
using System.Text;

// This is the Drop Cutter algorithm
// For a given (x,y) point and a given triangle
// It drops the cutter down along the z-axis until it touches the triangle
// 
// There are three tests:
// 1. cutter vs. vertex
// 2. cutter vs. facet
// 3. cutter vs. edge


namespace monoCAM
{
    class Cutter
    {
        public double R; // shaft radius
        public double r; // corner radius
        public Cutter(double Rset, double rset)
        {
            if (Rset > 0)
            {
                R = Rset;
            }
            else
            {
                // ERROR!
                // Throw an exception or something
            }

            if ((rset > 0) && (rset <= R))
            {
                r = rset;
            }
            else
            {
                // ERROR!
                // Throw an exception or something
            }
        }

        // TODO:
        // - bounding box is probably useful for orhtogonal range search

    } // end Cutter class

    class DropCutter
    {
       
       public static double? VertexTest(Cutter c,Geo.Point e, Geo.Point p)
       {
           // c.R and c.r define the cutter
           // e.x and e.y is the xy-position of the cutter (e.z is ignored)
           // p is the vertex tested against

           // q is the distance along xy-plane from e to vertex
           double q = Math.Sqrt(Math.Pow(e.x - p.x, 2) + Math.Pow((e.y - p.y), 2));

           if (q > c.R)
           { 
               // vertex is outside cutter. no need to do anything!
               return null;
           }
           else if (q <= (c.R - c.r))
           { 
                // vertex is in the cylindical/flat part of the cutter
               return p.z;
           }
           else if ((q > (c.R - c.r)) && (q <= c.R))
           {
               // vertex is in the toroidal part of the cutter
               double h2 = Math.Sqrt(Math.Pow(c.r, 2) - Math.Pow((q - (c.R - c.r)), 2));
               double h1 = c.r - h2;
               return p.z - h1;
           }
           else
           {
               // SERIOUS ERROR, we should not be here!
               System.Console.WriteLine("DropCutter: VertexTest: ERROR!");
               return null;
           }

       } // end VertexTest

        public static double? FacetTest(Cutter cu, Geo.Point e, Geo.Tri t)
        { 
            // local copy of the surface normal
            Vector n = new Vector(t.n.x, t.n.y, t.n.z);
            Geo.Point cc;

            if (n.z == 0)
            {
                // vertical plane, can't touch cutter against that!
                return null;
            }
            else if (n.z < 0)
            {
                // flip the normal so it points up (? is this always required?)
                n = -1*n;
            }

            // define plane containing facet
            double a = n.x;
            double b = n.y;
            double c = n.z;
            double d = -n.x * t.p[0].x - n.y * t.p[0].y - n.z * t.p[0].z;

            // the z-direction normal is a special case (?required?)
            // in debug phase, see if this is a useful case!
            if ((a == 0) && (b == 0))
            {
                e.z = t.p[0].z;
                cc = new Geo.Point(e.x,e.y,e.z);
                if (isinside(t,cc))
                    return e.z;
                else 
                    return null;
            }


            // facet test general case
            // uses trigonometry, so might be too slow?

            // flat endmill and ballnose should be simple to do without trig
            // toroidal case might require offset-ellipse idea?

            /*
            theta = asin(c);
            zf= -d/c - (a*xe+b*ye)/c+ (R-r)/tan(theta) + r/sin(theta) -r;
            e=[xe ye zf];
            u=[0  0  1];
            rc=e + ((R-r)*tan(theta)+r)*u - ((R-r)/cos(theta) + r)*n;
            t=isinside(p1,p2,p3,rc);
            */

            double theta = Math.Asin(c);
            double zf = -d/c - (a*e.x+b*e.y)/c + (cu.R-cu.r)/Math.Tan(theta) + cu.r/Math.Sin(theta) - cu.r;
            Vector ve = new Vector(e.x,e.y,zf);
            Vector u = new Vector(0,0,1);
            Vector rc = new Vector();
            rc = ve +((cu.R-cu.r)*Math.Tan(theta)+cu.r)*u - ((cu.R-cu.r)/Math.Cos(theta)+cu.r)*n;

            cc = new Geo.Point(rc.x, rc.y, rc.z);

            if (isinside(t, cc))
                return zf;
            else
                return null;




        } // end FacetTest

        public static bool isinside(Geo.Tri t, Geo.Point p)
        {
            // point in triangle test

            // a new Tri projected onto the xy plane:
            Geo.Point p1 = new Geo.Point(t.p[0].x, t.p[0].y, 0);
            Geo.Point p2 = new Geo.Point(t.p[1].x, t.p[1].y, 0);
            Geo.Point p3 = new Geo.Point(t.p[2].x, t.p[2].y, 0);
            Geo.Point pt = new Geo.Point(p.x, p.y, 0);

            bool b1 = isright(p1, p2, pt);
            bool b2 = isright(p3, p1, pt);
            bool b3 = isright(p2, p3, pt);

            if (b1 && b2 && b3)
            {
                return true;
            }
            else if (!b1 && !b2 && !b3)
            {
                return true;
            }
            else
            {
                return false;
            }

        } // end isinside()

        public static bool isright(Geo.Point p1, Geo.Point p2, Geo.Point p)
        {
            // is point p right of line through points p1 and p2 ?

            // this is an ugly way of doing a determinant
            // should be prettyfied sometime...
            double a1 = p2.x - p1.x;
            double a2 = p2.y - p2.y;
            double t1 = a2;
            double t2 = -a1;
            double b1 = p.x - p1.x;
            double b2 = p.y - p1.y;

            double t = t1 * b1 + t2 * b2;
            if (t>0)
                return true;
            else
                return false;
        } // end isright()



        public static double? EdgeTest(Cutter cu, Geo.Point e, Geo.Point p1, Geo.Point p2)
        { 
            // contact cutter against edge from p1 to p2

            // translate segment so that cutter is at (0,0)
            Geo.Point start = new Geo.Point(p1.x - e.x, p1.y - e.y, p1.z);
            Geo.Point end = new Geo.Point(p2.x - e.x, p2.y - e.y, p2.z);

            // find angle btw. segment and X-axis
            double dx = end.x - start.x;
            double dy = end.y - start.y;
            double alfa;
            if (dx != 0)
                alfa = Math.Atan(dy / dx);
            else
                alfa = Math.PI / 2;

            // rotation matrix for rotation around z-axis:
            // should probably implement a matrix class later

            // rotate by angle alfa
            start.x = start.x * Math.Cos(alfa) + start.y * Math.Sin(alfa);
            start.y = -start.x * Math.Sin(alfa) + start.y * Math.Cos(alfa);
            end.x = end.x * Math.Cos(alfa) + end.y * Math.Sin(alfa);
            end.y = -end.x * Math.Sin(alfa) + end.y * Math.Cos(alfa);

            // check if segment is below cutter
            if (start.y > 0)
            {   // if it's above cutter then rotate some more
                alfa = alfa + Math.PI;
                start.x = start.x * Math.Cos(alfa) + start.y * Math.Sin(alfa);
                start.y = -start.x * Math.Sin(alfa) + start.y * Math.Cos(alfa);
                end.x = end.x * Math.Cos(alfa) + end.y * Math.Sin(alfa);
                end.y = -end.x * Math.Sin(alfa) + end.y * Math.Cos(alfa);
            }

            double l = -start.y; // distance from cutter to edge

            // now we have two different algorithms depending on the cutter:
            if (cu.r == 0)
            {
                // this is the flat endmill case
                // it is easier and faster than the general case, so we handle it separately
                if (l > cu.R) // edge is outside of the cutter
                    return null;
                else // we are inside the cutter
                {   // so calculate CC point
                    double xc1 = Math.Sqrt(Math.Pow(cu.R, 2) - Math.Pow(l, 2));
                    double xc2 = -xc1;
                    double zc1 = ((xc1 - start.x) / (end.x - start.x)) * (end.z - start.z) + start.z;
                    double zc2 = ((xc2 - start.x) / (end.x - start.x)) * (end.z - start.z) + start.z;

                    // choose the higher point
                    double zc,xc;
                    if (zc1 > zc2)
                    {
                        zc = zc1;
                        xc = xc1;
                    }
                    else
                    {
                        zc = zc2;
                        xc = xc2;
                    }

                    // now that we have a CC point, check if it's in the edge
                    if ((start.x > xc) && (xc < end.x))
                        return null;
                    else if ((end.x < xc) && (xc > start.x))
                        return null;
                    else
                        return zc;

                }
                // unreachable place (according to compiler)
            } // end of flat endmill (r=0) case

            else if (cu.r > 0)
            { 
                // this is the general case (r>0)   ball-nose or bull-nose (spherical or toroidal)
                // later a separate case for the ball-cutter might be added (for performance)

                double xd=0, w=0, h=0, xd1=0, xd2=0, xc=0 , ze=0, zc=0;

                if (l > cu.R) // edge is outside of the cutter
                    return null;
                else if (((cu.R-cu.r)<l)&&(l<=cu.R))
                {    // toroidal case
                    xd=0; // center of ellipse
                    w=Math.Sqrt(Math.Pow(cu.R,2)-Math.Pow(l,2)); // width of ellipse
                    h=Math.Sqrt(Math.Pow(cu.r,2)-Math.Pow((l-(cu.R-cu.r)),2)); // height of ellipse
                }
                else if ((cu.R-cu.r)>=l)
                {
                    // quarter ellipse case
                    xd1=Math.Sqrt( Math.Pow((cu.R-cu.r),2)-Math.Pow(l,2));
                    xd2=-xd1;
                    h=cu.r; // ellipse height
                    w=Math.Sqrt( Math.Pow(cu.R,2)-Math.Pow(l,2) )- Math.Sqrt( Math.Pow((cu.R-cu.r),2)-Math.Pow(l,2) ); // ellipse height
                }

                // now there is a special case where the theta calculation will fail if
                // the segment is horziontal, i.e. start.z==end.z  so we need to catch that here
                if (start.z==end.z)
                {
                    if ((cu.R-cu.r)<l) 
                    {
                        // half-ellipse case
                        xc=0;
                        h=Math.Sqrt(Math.Pow(cu.r,2)-Math.Pow((l-(cu.R-cu.r)),2));
                        ze = start.z + h - cu.r;
                    }
                    else if ((cu.R - cu.r) > l)
                    {
                        // quarter ellipse case
                        xc = 0;
                        ze = start.z;
                    }

                    // now we have a CC point
                    // so we need to check if the CC point is in the edge
                    if ((start.x > xc) && (xc < end.x))
                        return null;
                    else if ((end.x < xc) && (xc > start.x))
                        return null;
                    else
                        return ze;

                } // end horizontal edge special case


                // now the general case where the theta calculation works
                double theta = Math.Atan( h*(start.x-end.x)/(w*(start.z-end.z))  );

                // based on this calculate the CC point
                if (((cu.R - cu.r) < l) && (cu.R <= l))
                {
                    // half-ellipse case
                    double xc1 = xd + Math.Abs(w * Math.Cos(theta));
                    double xc2 = xd - Math.Abs(w * Math.Cos(theta));
                    double zc1 = ((xc1 - start.x) / (end.x - start.x)) * (end.z - start.z) + start.z;
                    double zc2 = ((xc2 - start.x) / (end.x - start.x)) * (end.z - start.z) + start.z;
                    // select the higher point:
                    if (zc1 > zc2)
                    {
                        zc = zc1;
                        xc = xc1;
                    }
                    else
                    {
                        zc = zc2;
                        xc = xc2;
                    }

                }
                else if ((cu.R - cu.r) > l)
                { 
                    // quarter ellipse case
                    double xc1 = xd1 + Math.Abs(w * Math.Cos(theta));
                    double xc2 = xd2 - Math.Abs(w * Math.Cos(theta));
                    double zc1 = ((xc1 - start.x) / (end.x - start.x)) * (end.z - start.z) + start.z;
                    double zc2 = ((xc2 - start.x) / (end.x - start.x)) * (end.z - start.z) + start.z;
                    // select the higher point:
                    if (zc1 > zc2)
                    {
                        zc = zc1;
                        xc = xc1;
                    }
                    else
                    {
                        zc = zc2;
                        xc = xc2;
                    }
                }

                // now we have a valid xc value, so calculate the ze value:
                ze = zc + Math.Abs(h * Math.Sin(theta)) - cu.r;

                // finally, check that the CC point is in the edge
                if ((start.x > xc) && (xc < end.x))
                    return null;
                else if ((end.x < xc) && (xc > start.x))
                    return null;
                else
                    return ze;


                // this line is unreachable (according to compiler)
                
            } // end of toroidal/spherical case
 
            
            // if we ever get here it is probably a serious error!
            System.Console.WriteLine("EdgeTest: ERROR: no case returned a valid ze!");
            return null;

        } // end of EdgeTest method




   } // end DropCutter class


} // end namespace monoCAM