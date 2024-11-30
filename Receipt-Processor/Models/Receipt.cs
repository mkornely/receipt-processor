using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Receipt_Processor.Models;

public class Receipt
{
    /// <summary>
    /// The name of the retailer or store the receipt is from.
    /// </summary>
    [Required]
    [RegularExpression(@"^[\w\s\-&]+$", ErrorMessage = "The receipt is invalid")]
    public string Retailer { get; set; } 

    /// <summary>
    /// The date of the purchase printed on the receipt.
    /// </summary>
    [Required]
    [DataType(DataType.Date)]
    public string PurchaseDate { get; set; } 

    /// <summary>
    /// The time of the purchase printed on the receipt. 24-hour time expected.
    /// </summary>
    [Required]
    [RegularExpression(@"^([01]\d|2[0-3]):([0-5]\d)$", ErrorMessage = "The receipt is invalid")]
    public string PurchaseTime { get; set; }

    /// <summary>
    /// The list of items purchased.
    /// </summary>
    [Required]
    [MinLength(1, ErrorMessage = "The receipt is invalid")]
    public List<Item> Items { get; set; }

    /// <summary>
    /// The total amount paid on the receipt.
    /// </summary>
    [Required]
    [RegularExpression(@"^\d+\.\d{2}$", ErrorMessage = "The receipt is invalid")]
    public string Total { get; set; } 
}