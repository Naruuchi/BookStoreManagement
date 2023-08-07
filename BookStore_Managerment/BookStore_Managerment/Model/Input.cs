using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore_Managerment.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace BookStore_Managerment.Model;

[Table("Input")]
public partial class Input : BaseViewModel
{
    [Key]
    [Column("id")]
    [StringLength(128)]
    public string Id { get; set; } = null!;

    [Column("dateInput", TypeName = "date")]
    private DateTime? _DateInput;
    public DateTime? DateInput { get => _DateInput; set { _DateInput = value; OnPropertyChanged(); } }

    [InverseProperty("Input")]
    public virtual ICollection<InputInfo> InputInfos { get; set; } = new List<InputInfo>();
}
