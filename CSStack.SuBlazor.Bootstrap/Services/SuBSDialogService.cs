namespace CSStack.SuBlazor.Bootstrap
{
    public sealed class SuBSDialogService : SuDialogService
    {
        public SuBSDialogService(Options option)
            : base(option)
        {
            BackgroundClass = $"{BackgroundClass} modal";
            BackgroundStyle = $"{BackgroundStyle} display: block;";
        }
    }
}
