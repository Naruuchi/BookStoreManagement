using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStore_Managerment.Model;

[Table("Output")]
public partial class Output
{
    [Key]
    [Column("id")]
    [StringLength(128)]
    public string Id { get; set; } = null!;

    [Column("dateOutput", TypeName = "date")]
    public DateTime? DateOutput { get; set; }

    [InverseProperty("Output")]
    public virtual ICollection<OutputInfo> OutputInfos { get; set; } = new List<OutputInfo>();
}
