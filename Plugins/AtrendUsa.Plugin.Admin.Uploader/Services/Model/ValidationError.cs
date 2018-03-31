namespace AtrendUsa.Plugin.Admin.Uploader.Services.Model
{
    public class ValidationError
    {
        public ValidationError()
        {
        }

        public ValidationError(string field)
        {
            Field = field;
        }

        public ValidationError(string field, string message)
        {
            Field = field;
            Message = message;
        }

        public string Field { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return string.Format("{0} : {1}", Field, Message);
        }
    }
}