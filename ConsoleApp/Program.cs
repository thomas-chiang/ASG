using System.Text;
using System.Text.Json;

namespace ConsoleApp;

internal class Program
{
    private static async Task Main()
    {
        var csvFilePath = "1001.csv";
        var newCsvFilePath = "1001_result.csv";

        // Read the CSV file and process each row
        var lines = await File.ReadAllLinesAsync(csvFilePath);
        var updatedLines = new List<string>();

        using (var client = new HttpClient())
        {
            foreach (var line in lines)
            {
                Console.WriteLine(
                    "================================================================================================================================");
                var parts = line.Split(',');
                var company = parts[0].Split('.')[0];
                if (!parts[0].Contains("1001"))
                {
                    Console.WriteLine($"!!!!!!!formKind {parts[0]} Do not below 1001, continue");
                    updatedLines.Add(line);
                    continue;
                }

                var formKind = parts[0];
                var formNo = parts[1];

                // Create the request body
                var requestBody = new
                {
                    formKind = formKind,
                    formNo = formNo
                };
                var jsonRequestBody = JsonSerializer.Serialize(requestBody);

                // Send the POST request
                var httpResponse = await client.PostAsync(
                    "http://localhost:5212/ApolloSyncGaia1001FormOperation",
                    new StringContent(jsonRequestBody, Encoding.UTF8, "application/json")
                );

                string result = null;
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();

                    // Parse the JSON content
                    using (var doc = JsonDocument.Parse(responseContent))
                    {
                        // Pretty print the JSON content
                        var options = new JsonSerializerOptions { WriteIndented = true };
                        var prettyJson = JsonSerializer.Serialize(doc.RootElement, options);
                        Console.WriteLine("Response Content:");
                        Console.OutputEncoding = Encoding.UTF8;
                        Console.WriteLine(prettyJson);

                        if (doc.RootElement.TryGetProperty("apolloSyncGaia1001FormOperation",
                                out var apolloSyncGaia1001FormOperation))
                            if (apolloSyncGaia1001FormOperation.TryGetProperty("situation",
                                    out var situationElement))
                                result = situationElement.GetString();
                    }
                }

                if (!string.IsNullOrEmpty(result))
                {
                    if (result == "忘打卡_一般拋轉失敗")
                        updatedLines.Add($"{line},{result},預防,已已處理");
                    else
                        updatedLines.Add($"{line},{result},預防,不用處理");
                }
                else
                {
                    updatedLines.Add(line); // No result, keep the original line
                }

                Console.WriteLine(
                    "================================================================================================================================");
            }
        }

        // Write the updated lines to the new CSV file
        await File.WriteAllLinesAsync(newCsvFilePath, updatedLines, Encoding.UTF8);

        Console.WriteLine($"CSV file updated successfully! New file created: {newCsvFilePath}");
    }
}