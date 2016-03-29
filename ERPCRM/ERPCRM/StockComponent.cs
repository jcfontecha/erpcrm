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
    
    public partial class StockComponent
    {
        public int idStockComponent { get; set; }
        public int idComponent { get; set; }
        public int idExpense { get; set; }
        public int idVendor { get; set; }
        public Nullable<int> idStockProduct { get; set; }
        public int warehouse { get; set; }
        public int shelf { get; set; }
        public int drawer { get; set; }
        public System.DateTime expirationDate { get; set; }
    
        public virtual Component Component { get; set; }
        public virtual Expense Expense { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual StockProduct StockProduct { get; set; }
    }
}
