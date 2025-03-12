using Microsoft.AspNetCore.Components;
using System.Collections.Immutable;

namespace CSStack.SuBlazor
{
    public partial class SuDialog
    {
        [Inject]
        public required SuDialogService SuDialogService { get; set; }

        public ImmutableList<SuDialogService.DialogContext> SortedDialogContexts => SuDialogService?.DialogContexts.OrderBy(x => x.Index).ToImmutableList() ?? [];

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SuDialogService.OnDialogContextsChange += StateHasChanged;
        }
    }
}
