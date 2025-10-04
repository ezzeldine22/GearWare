﻿using System;
using System.Collections.Generic;

namespace DAL.Data.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string UserId { get; set; }

    public DateTime OrderDateUtc { get; set; }

    public string Status { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public string? ShippingAddress { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Payment? Payment { get; set; }

    public virtual User User { get; set; } = null!;
}
