namespace TestSoft.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("dbtest.bomm")]
    public partial class bomm
    {
        public int ID { get; set; }

        [StringLength(45)]
        public string bom_level { get; set; }

        [StringLength(45)]
        public string Parent_Part_Number { get; set; }

        [StringLength(300)]
        public string Part_Number { get; set; }

        [StringLength(45)]
        public string Part_Name { get; set; }

        [StringLength(45)]
        public string Revision { get; set; }

        [StringLength(45)]
        public string Quantit { get; set; }

        [StringLength(45)]
        public string Unit_of_measure { get; set; }

        [StringLength(45)]
        public string Procurement_Type { get; set; }

        [StringLength(45)]
        public string Reference_Designatos { get; set; }

        [StringLength(45)]
        public string BOM_Notes { get; set; }
    }
}
