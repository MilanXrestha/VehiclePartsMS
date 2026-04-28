using System.ComponentModel.DataAnnotations;

namespace VehiclePartsMS.Domain.Models;

public class User
{
    [Key]
    public long Id { get; set; }

    [Required, MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Phone { get; set; }

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual CustomerProfile? CustomerProfile { get; set; }
    public virtual ICollection<SalesInvoice> SalesInvoicesAsStaff { get; set; } = new List<SalesInvoice>();
}
