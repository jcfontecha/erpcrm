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
    
    public partial class Country
    {
        public Country()
        {
            this.Addresses = new ObservableCollection<Address>();
            this.Schools = new ObservableCollection<School>();
        }
    
        public int idCountry { get; set; }
        public string name { get; set; }
    
        public virtual ObservableCollection<Address> Addresses { get; set; }
        public virtual ObservableCollection<School> Schools { get; set; }
    }
}
