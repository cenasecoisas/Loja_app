//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Loja_app
{
    using System;
    using System.Collections.Generic;
    
    public partial class IVA_Table
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public IVA_Table()
        {
            this.Produto_Table = new HashSet<Produto_Table>();
        }
    
        public int ID { get; set; }
        public Nullable<int> IVA { get; set; }
        public string Descrição { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Produto_Table> Produto_Table { get; set; }
    }
}
