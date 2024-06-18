using Newtonsoft.Json;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);

var salesFiles = FindFiles(storesDirectory);

var salesTotal = CalculateSalesTotal(salesFiles);

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");

SalesSummary(salesFiles, salesTotal);


IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}


double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    // Loop over each file path in salesFiles
    foreach (var file in salesFiles)
    {
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);

        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        // Add the amount found in the Total field to the salesTotal variable
        salesTotal += data?.Total ?? 0;
    }

    return salesTotal;
}

double SalesSummary(IEnumerable<string> salesFiles, double salesTotal)
{
    var fileName = "SalesSummary.txt";

    File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), fileName), $"Sales Summary{Environment.NewLine}");
    File.AppendAllText(Path.Combine(Directory.GetCurrentDirectory(), fileName), $"----------------------------{Environment.NewLine}");
    File.AppendAllText(Path.Combine(Directory.GetCurrentDirectory(), fileName), $"{salesTotal.ToString("C")}{Environment.NewLine}");
    File.AppendAllText(Path.Combine(Directory.GetCurrentDirectory(), fileName), $"{Environment.NewLine}");
    File.AppendAllText(Path.Combine(Directory.GetCurrentDirectory(), fileName), $"Details:{Environment.NewLine}");


    foreach (var file in salesFiles)
    {
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);

        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        // Add the amount found in the Total field to the salesTotal variable

        File.AppendAllText(Path.Combine(Directory.GetCurrentDirectory(), fileName), $"{file}: {salesTotal.ToString("C")}{Environment.NewLine}");
        salesTotal += data?.Total ?? 0;
    }

    return 0;
}

record SalesData(double Total);
