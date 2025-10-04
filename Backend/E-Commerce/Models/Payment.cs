using System;
using System.Collections.Generic;

namespace E_Commerce.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public DateTime PaymentDateUtc { get; set; }

    public decimal Amount { get; set; }

    public string Status { get; set; } = null!;

    public string Method { get; set; } = null!;

    public string? TransactionRef { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
