using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore_Managerment.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace BookStore_Managerment.Model;

[Table("OutputInfo")]
public partial class OutputInfo : BaseViewModel
{
    [Key]
    [Column("id")]
    [StringLength(128)]
    public string Id { get; set; } = null!;

    [Column("bookId")]
    [StringLength(128)]
    public string BookId { get; set; } = null!;

    [Column("inputInfoId")]
    [StringLength(128)]
    public string InputInfoId { get; set; } = null!;

    [Column("outputId")]
    [StringLength(128)]
    public string OutputId { get; set; } = null!;

    [Column("customerId")]
    public int? CustomerId { get; set; }

    [Column("amount")]
    private int? _Amount;
    public int? Amount { get => _Amount; set { _Amount = value; OnPropertyChanged(); } }

    [ForeignKey("BookId")]
    [InverseProperty("OutputInfos")]
    public virtual Book Book { get; set; } = null!;

    [ForeignKey("CustomerId")]
    [InverseProperty("OutputInfos")]
    private Customer? _Customer;
    public virtual Customer? Customer { get => _Customer; set { _Customer = value; OnPropertyChanged(); } }

    [ForeignKey("InputInfoId")]
    [InverseProperty("OutputInfos")]
    public virtual InputInfo InputInfo { get; set; } = null!;

    [ForeignKey("OutputId")]
    [InverseProperty("OutputInfos")]
    public virtual Output Output { get; set; } = null!;
}
