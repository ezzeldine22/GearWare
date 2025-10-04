﻿using System;
using System.Collections.Generic;

namespace DAL.Data.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public string UserId { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual User User { get; set; } = null!;
}
