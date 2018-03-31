namespace AtrendUsa.Plugin.Admin.Uploader.Services.Interfaces
{
    public interface IResolver<in TC, out T>
    {
        T Resolve(TC input);
    }
}