//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ERPCRM
{
    using System;
    using System.Collections.ObjectModel;
    
    public partial class Invoice
    {
        public int idInvoice { get; set; }
        public int idSaleOrder { get; set; }
        public int idClient { get; set; }
        public float tax { get; set; }
        public float finalPrice { get; set; }
    
        public virtual Client Client { get; set; }
        public virtual SaleOrder SaleOrder { get; set; }
    }
}
