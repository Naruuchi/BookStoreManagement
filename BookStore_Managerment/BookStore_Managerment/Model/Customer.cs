using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore_Managerment.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace BookStore_Managerment.Model;

[Table("Customer")]
public partial class Customer : BaseViewModel
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("displayName")]
    private string? _DisplayName;
    public string? DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }

    [Column("address")]
    private string? _Address;
    public string? Address { get => _Address; set { _Address = value; OnPropertyChanged(); } }

    [Column("phone")]
    [StringLength(20)]
    [Unicode(false)]
    private string? _Phone;
    public string? Phone { get => _Phone; set { _Phone = value; OnPropertyChanged(); } }

    [Column("email")]
    [StringLength(200)]
    [Unicode(false)]
    private string? _Email;
    public string? Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }

    [InverseProperty("Customer")]
    public virtual ICollection<OutputInfo> OutputInfos { get; set; } = new List<OutputInfo>();
}
