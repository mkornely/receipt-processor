using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using Receipt_Processor.Models;

namespace Receipt_Processor.Controllers;

[ApiController]
[Route("receipts")]
public class ReceiptController : ControllerBase
{
    private ConcurrentDictionary<Guid, int> Receipts = new();

    /// <summary>
    /// Submits a receipt for processing.
    /// </summary>
    /// <remarks>
    /// Calculates points based on the receipt data and assigns a unique ID.
    /// </remarks>
    /// <param name="receipt">The receipt object containing retailer, purchase date, time, items, and total.</param>
    /// <returns>The ID assigned to the receipt.</returns>
    /// <response code="200">Successfully processed the receipt and returned its unique ID.</response>
    /// <response code="400">The receipt data provided is invalid.</response>
    [HttpPost(Name = "Process")]
    public ActionResult<Guid> Process([FromBody] Receipt receipt)
    {
        // Generate a new unique ID for the receipt
        var id = Guid.NewGuid();

        // Calculate the total points for the receipt
        int points = CalculatePoints(receipt);

        // Store the receipt's points using the generated ID
        Receipts[id] = points;

        // Return the generated ID in a JSON response
        return Ok(new { id });
    }

    /// <summary>
    /// Retrieves the points awarded for a specific receipt.
    /// </summary>
    /// <remarks>
    /// Returns the total points for a receipt identified by its unique ID.
    /// </remarks>
    /// <param name="id">The unique ID of the receipt.</param>
    /// <returns>The total points awarded for the receipt.</returns>
    /// <response code="200">Successfully retrieved the points for the specified receipt ID.</response>
    /// <response code="404">No receipt found with the provided ID.</response>
    [HttpGet("{id}/points")]
    public ActionResult<int> GetPoints([FromRoute] Guid id)
    {
        // Try to find the receipt by ID
        if (Receipts.TryGetValue(id, out var points))
        {
            // Return the points in a JSON object
            return Ok(new { points });
        }

        // Return 404 if the receipt ID is not found
        return NotFound("Receipt not found.");
    }

    /// <summary>
    /// Calculates the points for a receipt based on its data.
    /// </summary>
    /// <param name="receipt">The receipt to calculate points for.</param>
    /// <returns>The total points calculated for the receipt.</returns>
    private int CalculatePoints(Receipt receipt)
    {
        var points = 0;

        // 1 point for each alphanumeric character in the retailer name
        points += receipt.Retailer.Count(char.IsLetterOrDigit);

        // 50 points if the total is a whole number
        if (decimal.TryParse(receipt.Total, out var totalAmount) && totalAmount % 1 == 0)
        {
            points += 50;
        }

        // 25 points if the total is a multiple of 0.25
        if (totalAmount % 0.25m == 0)
        {
            points += 25;
        }

        //  5 points for every two items
        points += (receipt.Items.Count / 2) * 5;

        // Additional points for item descriptions
        foreach (var item in receipt.Items)
        {
            if (!string.IsNullOrWhiteSpace(item.ShortDescription) &&
                item.ShortDescription.Trim().Length % 3 == 0 &&
                decimal.TryParse(item.Price, out var itemPrice))
            {
                points += (int)Math.Ceiling(itemPrice * 0.2m);
            }
        }

        // 6 points if the purchase day is odd
        if (DateTime.TryParse(receipt.PurchaseDate, out var purchaseDate) && purchaseDate.Day % 2 == 1)
        {
            points += 6;
        }

        //10 points if the time is between 2 PM and 4 PM
        if (TimeSpan.TryParse(receipt.PurchaseTime, out var purchaseTime) &&
            purchaseTime.Hours >= 14 && purchaseTime.Hours < 16)
        {
            points += 10;
        }

        return points;
    }
}
