using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore_Managerment.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace BookStore_Managerment.Model;

public partial class User : BaseViewModel
{
    [Key]
    [Column("id")]
    private int _Id;
    public int Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }

    [Column("displayName")]
    private string? _DisplayName;
    public string? DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }

    [Column("username")]
    [StringLength(100)]
    [Unicode(false)]
    private string? _Username;
    public string? Username { get => _Username; set { _Username = value; OnPropertyChanged(); } }

    [Column("password")]
    [Unicode(false)]
    private string? _Password;
    public string? Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }

    [Column("role")]
    [StringLength(255)]
    [Unicode(false)]
    private string? _Role;
    public string? Role { get => _Role; set { _Role = value; OnPropertyChanged(); } }
}
