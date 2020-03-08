using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class DashboardDetails
    {
        public int QID { get; set; }
        public string Question { get; set; }
        public System.DateTime Datetime { get; set; }
        public string ID { get; set; }
        public int AID { get; set; }
        public string Answer { get; set; }

        public static string LibraryPDF(int Pkey,Detail user)
        {
            var context = new CollegeBuddyEntities();
            LibraryTable xy = new LibraryTable();
            var obj = context.LibraryTables.SingleOrDefault(x => x.Pkey == Pkey && x.ID == user.ID);
            //var obj = context.LibraryTables.SqlQuery("Select * from LibraryTable where Pkey="+Pkey+"and ID="+user.ID);
           //var obj = context.LibraryTables.SingleOrDefault(x => x.Pkey == Pkey);
            if (obj == null)
            {
                xy.ID = user.ID;
                xy.Pkey = Pkey;
                context.LibraryTables.Add(xy);
                context.SaveChanges();
                return ("Added to library");
            }
            else
            {
                return ("PDF Already in library");
            }
        }
    }

    
}