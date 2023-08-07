using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore_Managerment.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace BookStore_Managerment.Model;

[Table("Book")]
public partial class Book : BaseViewModel
{
    [Key]
    [Column("id")]
    [StringLength(128)]
    public string Id { get; set; } = null!;

    [Column("displayName")]
    private string? _DisplayName;
    public string? DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }

    [Column("suplierId")]
    public int SuplierId { get; set; }

    [InverseProperty("Book")]
    public virtual ICollection<InputInfo> InputInfos { get; set; } = new List<InputInfo>();

    [InverseProperty("Book")]
    public virtual ICollection<OutputInfo> OutputInfos { get; set; } = new List<OutputInfo>();

    [ForeignKey("SuplierId")]
    [InverseProperty("Books")]
    public virtual Suplier Suplier { get; set; } = null!;
}

