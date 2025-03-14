namespace CSStack.SuBlazor.Bootstrap
{
    public sealed class SuBSDialogService : SuDialogService
    {
        public SuBSDialogService(Options options)
            : base(options)
        {
            BackgroundClass = $"{BackgroundClass} modal";
            BackgroundStyle = $"{BackgroundStyle} display: block;";
        }
    }
}
