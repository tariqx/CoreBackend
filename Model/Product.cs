using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CoreBackend.Model
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Column(TypeName = "varchar(80)")]
        public string Name { get; set; }
        [Column(TypeName = "varchar(80)")]
        public string Brand { get; set; }

    }
}
