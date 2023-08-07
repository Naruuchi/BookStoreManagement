using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore_Managerment.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace BookStore_Managerment.Model;

[Table("InputInfo")]
public partial class InputInfo : BaseViewModel
{
    [Key]
    [Column("id")]
    [StringLength(128)]
    public string Id { get; set; } = null!;

    [Column("bookId")]
    [StringLength(128)]
    public string BookId { get; set; } = null!;

    [Column("inputId")]
    [StringLength(128)]
    public string InputId { get; set; } = null!;

    [Column("amount")]
    private int? _Amount;
    public int? Amount { get => _Amount; set { _Amount = value; OnPropertyChanged(); } }

    [Column("inputPrice")]

    private double? _InputPrice;
    public double? InputPrice { get => _InputPrice; set { _InputPrice = value; OnPropertyChanged(); } }

    [Column("outputPrice")]
    private double? _OutputPrice;
    public double? OutputPrice { get => _OutputPrice; set { _OutputPrice = value; OnPropertyChanged(); } }

    [Column("dateInput", TypeName = "date")]
    private DateTime? _DateInput;
    public DateTime? DateInput { get => _DateInput; set { _DateInput = value; OnPropertyChanged(); } }

    [ForeignKey("BookId")]
    [InverseProperty("InputInfos")]
    public virtual Book Book { get; set; } = null!;

    [ForeignKey("InputId")]
    [InverseProperty("InputInfos")]
    public virtual Input Input { get; set; } = null!;

    [InverseProperty("InputInfo")]
    public virtual ICollection<OutputInfo> OutputInfos { get; set; } = new List<OutputInfo>();
}
