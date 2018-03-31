using AtrendUsa.Plugin.Admin.Uploader.Services.Model;
using Nop.Core.Domain.Catalog;

namespace AtrendUsa.Plugin.Admin.Uploader.Services.Interfaces
{
    public interface IProductResolver : IResolver<ProductResolveInput, Product>
    {
    }
}