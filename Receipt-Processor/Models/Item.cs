using System.ComponentModel.DataAnnotations;

namespace Receipt_Processor.Models;
public class Item
{
    /// <summary>
    /// The Short Product Description for the item.
    /// </summary>
    [Required]
    [RegularExpression(@"^[\w\s\-]+$", ErrorMessage = "The receipt is invalid")]
    public string ShortDescription { get; set; }

    /// <summary>
    /// The total price paid for this item.
    /// </summary>
    [Required]
    [RegularExpression(@"^\d+\.\d{2}$", ErrorMessage = "The receipt is invalid")]
    public string Price { get; set; }
}

