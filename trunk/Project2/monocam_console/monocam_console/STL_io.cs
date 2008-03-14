using System;
using System.Collections.Generic;
using System.Text;

namespace monoCAM
{
   class STL
   {
       static public STLSurf Load(System.IO.StreamReader fs)
       {
           // Here's where autodetection will be provided for loading binary stl files and by wrapping STLA(scii) and STLB(inary)
           // but for now as only one format is provided, not bothering with it, we need to define a object format for handling file loading/saving
           // System.IO.StreamReader fs = new System.IO.StreamReader(FileName);

           STLSurf surf = new STLSurf();
           int state = 0;
           int counter = 0;
           string[] data;
           Tri triangle = new Tri();
           Vector normal = new Vector();

           System.Globalization.CultureInfo locale =  new System.Globalization.CultureInfo("en-GB");
           int n_triangles=0;
           while ( !fs.EndOfStream ) {
               data = fs.ReadLine().TrimStart(' ').Split(' ');

               switch (state)
               {
                   case 0:
                       if (data[0].Equals("solid"))
                       {
                           
                           state = 1; // continue readingx
                       }
                       break;
                   case 1:
                       if (data[0].Equals("facet")) {
                           normal.x = double.Parse(data[2], locale);
                           normal.y = double.Parse(data[3], locale);
                           normal.z = double.Parse(data[4], locale);
                           triangle = new Tri(normal);
                           counter = 0;
                           state = 2;
                       }
                       break;
                   case 2:
                       if (data[0].Equals("vertex"))
                       {
                           if ( counter <= 2 ) {
                               triangle.p[counter].x = double.Parse(data[1], locale);
                               triangle.p[counter].y = double.Parse(data[2], locale);
                               triangle.p[counter].z = double.Parse(data[3], locale);
                               // System.Console.WriteLine("STLReader: added point" + triangle.p[counter]);
                               // System.Console.ReadKey();
                               counter++;
                           }


                       } else if (data[0].Equals("endfacet"))
                       {
                           if (counter == 3)
                           {
                               surf.AddTriangle(triangle);
                               n_triangles += 1;

                           }
                           state = 1;
                       }
                   break;
               }
           }
           fs.Close();
           System.Console.WriteLine("STLReader: read {0} triangles!",n_triangles);
           return (surf);
       }

   }
}