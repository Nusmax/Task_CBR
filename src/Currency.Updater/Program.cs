using System.Globalization;
using System.Text;
using System.Xml.Linq;
using Npgsql;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHttpClient("cbr");
builder.Services.AddHostedService<Updater>();
builder.Services.AddNpgsqlDataSource(builder.Configuration.GetConnectionString("Default")!);
var host = builder.Build();
host.Run();

class Updater(IHttpClientFactory http, NpgsqlDataSource ds, ILogger<Updater> log) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        await Tick(ct);
        using var t = new PeriodicTimer(TimeSpan.FromHours(24));
        while (await t.WaitForNextTickAsync(ct)) await Tick(ct);
    }
    async Task Tick(CancellationToken ct)
    {
        try
        {
            var c = http.CreateClient("cbr");
            var bytes = await c.GetByteArrayAsync("http://www.cbr.ru/scripts/XML_daily.asp", ct);
            var xml = Encoding.GetEncoding("windows-1251").GetString(bytes);
            var doc = XDocument.Parse(xml);
            await using var conn = await ds.OpenConnectionAsync(ct);
            foreach (var v in doc.Root!.Elements("Valute"))
            {
                var code = v.Element("CharCode")!.Value;
                var name = v.Element("Name")!.Value;
                var nom = int.Parse(v.Element("Nominal")!.Value);
                var val = decimal.Parse(v.Element("Value")!.Value, new CultureInfo("ru-RU")) / nom;
                await using var cmd = new NpgsqlCommand(
                    "INSERT INTO currency(id,name,rate) VALUES($1,$2,$3) ON CONFLICT(id) DO UPDATE SET name=$2, rate=$3", conn);
                cmd.Parameters.AddWithValue(code); cmd.Parameters.AddWithValue(name); cmd.Parameters.AddWithValue(val);
                await cmd.ExecuteNonQueryAsync(ct);
            }
            log.LogInformation("CBR updated");
        }
        catch (Exception ex) { log.LogError(ex, "CBR update failed"); }
    }
}
