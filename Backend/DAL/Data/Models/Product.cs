using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Data.Models;

public partial class Product
{
    public int ProductId { get; set; } 

 
    public string Name { get; set; } = null!; // 


    public string Description { get; set; } //


    public decimal Price { get; set; } //

    
    public int StockQuantity { get; set; } // 


    public DateTime CreatedAtUtc { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public int CategoryId { get; set; }
    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
