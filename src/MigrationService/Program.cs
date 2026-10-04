using Npgsql;
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddNpgsqlDataSource(builder.Configuration.GetConnectionString("Default")!);
builder.Services.AddHostedService<Migrator>();
var host = builder.Build();
host.Run();

class Migrator(NpgsqlDataSource ds, ILogger<Migrator> log, IHostApplicationLifetime lifetime) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        try
        {
            await using var conn = await ds.OpenConnectionAsync(ct);
            await using (var init = new NpgsqlCommand(
                "CREATE TABLE IF NOT EXISTS __migrations (name TEXT PRIMARY KEY, applied_at TIMESTAMPTZ DEFAULT now())", conn))
                await init.ExecuteNonQueryAsync(ct);

            var dir = Path.Combine(AppContext.BaseDirectory, "Migrations");
            if (!Directory.Exists(dir)) dir = Path.Combine(Directory.GetCurrentDirectory(), "Migrations");
            foreach (var f in Directory.GetFiles(dir, "*.sql").OrderBy(x => x))
            {
                var name = Path.GetFileName(f);
                await using var chk = new NpgsqlCommand("SELECT 1 FROM __migrations WHERE name=$1", conn);
                chk.Parameters.AddWithValue(name);
                if (await chk.ExecuteScalarAsync(ct) is not null) continue;
                var sql = await File.ReadAllTextAsync(f, ct);
                await using var cmd = new NpgsqlCommand(sql, conn);
                await cmd.ExecuteNonQueryAsync(ct);
                await using var ins = new NpgsqlCommand("INSERT INTO __migrations(name) VALUES($1) ON CONFLICT DO NOTHING", conn);
                ins.Parameters.AddWithValue(name);
                await ins.ExecuteNonQueryAsync(ct);
                log.LogInformation("Applied {M}", name);
            }
        }
        finally
        {
            // Миграции применены — гасим хост, чтобы контейнер завершился
            // с кодом 0 и сработало service_completed_successfully.
            lifetime.StopApplication();
        }
    }
}
