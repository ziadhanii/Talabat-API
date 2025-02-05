using Talabat.Core.Order_Aggregate;

namespace Talabat.Repository.Data;

public static class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        try
        {
            if (!context.Brands.Any())
            {
                var brandsData = await File.ReadAllTextAsync("../Talabat.Repository/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if (brands != null && brands.Count > 0)
                {
                    await context.Brands.AddRangeAsync(brands);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.Categories.Any())
            {
                var categoriesData = await File.ReadAllTextAsync("../Talabat.Repository/Data/DataSeed/categories.json");
                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);

                if (categories != null && categories.Count > 0)
                {
                    await context.Categories.AddRangeAsync(categories);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.Products.Any())
            {
                var productsData = await File.ReadAllTextAsync("../Talabat.Repository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products != null && products.Count > 0)
                {
                    await context.Products.AddRangeAsync(products);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.DeliveryMethods.Any())
            {
                var deliveryMethodsData =
                    await File.ReadAllTextAsync("../Talabat.Repository/Data/DataSeed/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

                if (deliveryMethods != null && deliveryMethods.Count > 0)
                {
                    await context.DeliveryMethods.AddRangeAsync(deliveryMethods);
                    await context.SaveChangesAsync();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
        }
    }
}