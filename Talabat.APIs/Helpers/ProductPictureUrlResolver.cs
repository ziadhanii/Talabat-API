public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
{
    private readonly IConfiguration _configuration;

    public ProductPictureUrlResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
    {
        return !string.IsNullOrEmpty(source.PictureUrl) ? $"{_configuration["APIBaseUrl"]}/{source.PictureUrl}" : string.Empty;
    }
}