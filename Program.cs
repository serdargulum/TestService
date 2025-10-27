using System.Diagnostics;

class Program
{
    const int totalIteration = 1000;

    static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        var options = GetOptions();

        foreach (var item in options)
        {
            Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");
            await CallService(item.Value, item.Key);
        }
    }

    static Dictionary<string, string> GetOptions()
    {
        Dictionary<string, string> options = new Dictionary<string, string>();
        options.Add("CSharpService - Async EF", "http://localhost:5230/api/products/async");
        options.Add("CSharpService - Sync EF", "http://localhost:5230/api/products");
        options.Add("CSharpService - Async Sql", "http://localhost:5230/api/products/sql");
        options.Add("CSharpService - Async Dapper", "http://localhost:5230/api/products/dapper");
        options.Add("CSharpService - Sync Void", "http://localhost:5230/api/test/void");
        options.Add("CSharpService - Calculation", "http://localhost:5230/api/test/calculation");
        options.Add("LaravelService - Sync Eloquent", "http://127.0.0.1:8000/api/products");
        options.Add("LaravelService - Void", "http://127.0.0.1:8000/api/test/void");
        options.Add("LaravelService - Calculation", "http://127.0.0.1:8000/api/test/calculation");

        return options;
    }

    static async Task CallService(string url, string key)
    {
        int totalRequests = totalIteration;
        int success = 0;

        Stopwatch stopwatch = Stopwatch.StartNew();

        for (int i = 0; i < totalRequests; i++)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    success++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata ({key}): {ex.Message}");
            }
        }

        stopwatch.Stop();

        TimeSpan totalTime = stopwatch.Elapsed;
        TimeSpan averageTime = TimeSpan.FromMilliseconds(totalTime.TotalMilliseconds / totalRequests);

        Console.WriteLine($"Service: {key}");
        Console.WriteLine($"Toplam süre: {totalTime}");
        Console.WriteLine($"Başarılı istek sayısı: {success}/{totalRequests}");
        Console.WriteLine($"Ortalama süre (her istek): {averageTime}");
        Console.WriteLine();
    }
}
