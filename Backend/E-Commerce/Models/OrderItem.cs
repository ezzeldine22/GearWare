using System;
using System.Collections.Generic;

namespace E_Commerce.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
