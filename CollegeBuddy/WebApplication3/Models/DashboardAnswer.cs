//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication3.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DashboardAnswer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DashboardAnswer()
        {
            this.CommentTables = new HashSet<CommentTable>();
        }
    
        public int AID { get; set; }
        public int QID { get; set; }
        public string Answer { get; set; }
        public string Name { get; set; }
        public Nullable<int> Upvotes { get; set; }
    
        public virtual DashboardQuestion DashboardQuestion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommentTable> CommentTables { get; set; }
    }
}
