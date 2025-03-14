using Microsoft.AspNetCore.Components;
using System.Collections.Immutable;
using System.Collections.Specialized;

namespace CSStack.SuBlazor
{
    public partial class SuDialog : IDisposable
    {
        public ImmutableList<SuDialogService.DialogContext> SortedDialogContexts =>
        SuDialogService?.DialogContexts.OrderBy(x => x.Index).ToImmutableList() ?? [];

        [Inject]
        public required SuDialogService SuDialogService { get; set; }

        public void Dispose()
        {
            SuDialogService.DialogContexts.CollectionChanged -= Update;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SuDialogService.DialogContexts.CollectionChanged += Update;
        }

        private void Update(object? sender, NotifyCollectionChangedEventArgs args)
        {
            InvokeAsync(StateHasChanged);
        }
    }
}
