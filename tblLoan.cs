//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LoanMangSys.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblLoan
    {
        public int ApplicationNo { get; set; }
        public string UserName { get; set; }
        public string PropertyDetails { get; set; }
        public System.DateTime ApplicationDate { get; set; }
        public decimal LoanAmount { get; set; }
        public int LoanTenure { get; set; }
        public decimal EMI { get; set; }
        public string LoanStatus { get; set; }
    
        public virtual tblUser tblUser { get; set; }
    }
}
