var transactions = new List<Transaction>
{
    new(1, "Boots", 25.50m, 200),
    new(2, "Tesco", 18.00m, 500),
    new(3, "Boots", 42.75m, 200),
    new(4, "Wickes", 125.00m, 400),
    new(5, "Tesco", 12.99m, 200),
    new(6, "Boots", 8.50m, 503),
    new(7, "Wickes", 75.00m, 200),
    new(8, "Tesco", 250.00m, 500)
};

Console.WriteLine("All transactions:");
foreach (var transaction in transactions)
{
    Console.WriteLine(
        $"{transaction.Id}: {transaction.Merchant} " +
        $"£{transaction.Amount:F2} Status {transaction.StatusCode}");
}

Console.WriteLine("\nFailed transactions:");
var failedTransactions = transactions
    .Where(transaction => transaction.StatusCode != 200)
    .ToList();
foreach (var transaction in failedTransactions)
{
    Console.WriteLine(
        $"{transaction.Merchant}: Status {transaction.StatusCode}");
}

Console.WriteLine("\nFailed Boots transactions:");
var failedBootsTransactions = transactions
    .Where(transaction =>
        transaction.Merchant == "Boots" &&
        transaction.StatusCode != 200)
    .ToList();
foreach (var transaction in failedBootsTransactions)
{
    Console.WriteLine(
        $"{transaction.Merchant}: Status {transaction.StatusCode}");
}

Console.WriteLine("\nMerchant names:");
var merchantNames = transactions
    .Select(transaction => transaction.Merchant)
    .Distinct()
    .OrderBy(merchant => merchant)
    .ToList();
foreach (var merchant in merchantNames)
{
    Console.WriteLine(merchant);
}

var summaries = transactions
    .Select(transaction => new
    {
        transaction.Merchant,
        transaction.Amount,
        Successful = transaction.StatusCode == 200
    })
    .ToList();
foreach (var summary in summaries)
{
    Console.WriteLine(
        $"{summary.Merchant}: £{summary.Amount:F2}, " +
        $"Success: {summary.Successful}");
}

var successfulCount = transactions
    .Count(transaction => transaction.StatusCode == 200);
var failedCount = transactions
    .Count(transaction => transaction.StatusCode != 200);
var successfulValue = transactions
    .Where(transaction => transaction.StatusCode == 200)
    .Sum(transaction => transaction.Amount);
var averageTransactionValue = transactions
    .Average(transaction => transaction.Amount);
Console.WriteLine($"\nSuccessful: {successfulCount}");
Console.WriteLine($"Failed: {failedCount}");
Console.WriteLine($"Successful value: £{successfulValue:F2}");
Console.WriteLine($"Average value: £{averageTransactionValue:F2}");

var hasServerErrors = transactions
    .Any(transaction => transaction.StatusCode >= 500);
Console.WriteLine($"Contains server errors: {hasServerErrors}");

Console.WriteLine("\nMerchant summary:");
var merchantSummaries = transactions
    .GroupBy(transaction => transaction.Merchant)
    .Select(group => new
    {
        Merchant = group.Key,
        TransactionCount = group.Count(),
        FailedCount = group.Count(transaction => transaction.StatusCode != 200),
        TotalValue = group.Sum(transaction => transaction.Amount)
    })
    .OrderByDescending(summary => summary.FailedCount)
    .ToList();
foreach (var summary in merchantSummaries)
{
    Console.WriteLine(
        $"{summary.Merchant}: " +
        $"{summary.TransactionCount} transactions, " +
        $"{summary.FailedCount} failed, " +
        $"£{summary.TotalValue:F2} total");
}

Console.WriteLine("\nHigh-value failures:");
var highValueFailures = transactions
    .Where(transaction =>
        transaction.StatusCode != 200 &&
        transaction.Amount >= 50m)
    .OrderByDescending(transaction => transaction.Amount)
    .ToList();
foreach (var transaction in highValueFailures)
{
    Console.WriteLine(
        $"{transaction.Merchant}: " +
        $"£{transaction.Amount:F2}, " +
        $"Status {transaction.StatusCode}");
}
public record Transaction(
    int Id,
    string Merchant,
    decimal Amount,
    int StatusCode);