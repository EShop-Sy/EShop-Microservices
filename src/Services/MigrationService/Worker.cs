using System.Diagnostics;
using Catalog.API.Data;
using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.MigrationService;

public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource SActivitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = SActivitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

            await RunMigrationAsync(dbContext, stoppingToken);
            await SeedDataAsync(dbContext, stoppingToken);
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }

    private static async Task RunMigrationAsync(ProductDbContext dbContext, CancellationToken stoppingToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            await dbContext.Database.MigrateAsync(stoppingToken);
        });
    }

    private static async Task SeedDataAsync(ProductDbContext dbContext, CancellationToken stoppingToken)
    {
        IEnumerable<Product> products =
        [
            new()
            {
                Name = "Solar Powered Flashlight", Description = "A fantastic product for outdoor enthusiasts",
                Price = 19.99m, ImageUrl = "images/a_bananas.jpeg"
            },
            new()
            {
                Name = "Hiking Poles", Description = "Ideal for camping and hiking trips", Price = 24.99m,
                ImageUrl = "images/a_bananas.jpeg"
            },
            new()
            {
                Name = "Outdoor Rain Jacket", Description = "This product will keep you warm and dry in all weathers",
                Price = 49.99m, ImageUrl = "images/a_bananas.jpeg"
            },
            new()
            {
                Name = "Survival Kit", Description = "A must-have for any outdoor adventurer", Price = 99.99m,
                ImageUrl = "images/a_bananas.jpeg"
            },
            new()
            {
                Name = "Outdoor Backpack",
                Description = "This backpack is perfect for carrying all your outdoor essentials", Price = 39.99m,
                ImageUrl = "images/a_bananas.jpeg"
            },
            new()
            {
                Name = "Camping Cookware", Description = "This cookware set is ideal for cooking outdoors",
                Price = 29.99m, ImageUrl = "images/a_bananas.jpeg"
            },
            new()
            {
                Name = "Camping Stove", Description = "This stove is perfect for cooking outdoors", Price = 49.99m,
                ImageUrl = "images/a_bananas.jpeg"
            },
            new()
            {
                Name = "Camping Lantern", Description = "This lantern is perfect for lighting up your campsite",
                Price = 19.99m, ImageUrl = "images/a_bananas.jpeg"
            },
            new()
            {
                Name = "Camping Tent", Description = "This tent is perfect for camping trips", Price = 99.99m,
                ImageUrl = "images/a_bananas.jpeg"
            }
        ];

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Seed the database
            await using var transaction = await dbContext.Database.BeginTransactionAsync(stoppingToken);
            await dbContext.Products.AddRangeAsync(products, stoppingToken);
            await dbContext.SaveChangesAsync(stoppingToken);
            await transaction.CommitAsync(stoppingToken);
        });
    }
}