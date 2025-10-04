using System;
using System.Collections.Generic;

namespace DAL.Data.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public string UserId { get; set; }

    public int ProductId { get; set; }

    public float Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
