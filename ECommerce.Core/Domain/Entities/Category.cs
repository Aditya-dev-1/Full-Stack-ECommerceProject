using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Drawing;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Entities
{
    /// <summary>
    /// Represents Categories In our Application ex: Mobiles,Tshirts,watches etc. 
    /// </summary>
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(100)]
        public string Cat_Name { get; set; }

        public bool Status {  get; set; } //Active or Inactive

        public IEnumerable<Product> Products { get; set; } = new List<Product>(); //NavigationProperty to Produts as One Category -> Many Products
        
    }
}
