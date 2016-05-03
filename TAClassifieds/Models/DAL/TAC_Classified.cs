//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TAClassifieds.Models.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class TAC_Classified
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TAC_Classified()
        {
            this.TAC_ClassifiedContact = new HashSet<TAC_ClassifiedContact>();
        }
    
        public int ClassifiedId { get; set; }
        public string ClassifiedTitle { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string ClassifiedImage { get; set; }
        public decimal ClassifiedPrice { get; set; }
        public System.DateTime PostedDate { get; set; }
        public System.Guid CreatedBy { get; set; }
        public int CategoryId { get; set; }
    
        public virtual TAC_Category TAC_Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TAC_ClassifiedContact> TAC_ClassifiedContact { get; set; }
    }
}