namespace AtrendUsa.Plugin.Admin.Uploader.Services.Model
{
    public class ProductResolveInput : InputBase
    {
        public ProductResolveInput(UserContext userContext) : base(userContext)
        {
        }

        public ProductInputRow ProductInputRow { get; set; }
    }
}