using BlazorTrip.Web.Dtos;
using Microsoft.AspNetCore.Components;

namespace BlazorTrip.Web.Components;

public partial class TransactionTable
{
    [Parameter] [EditorRequired] public IEnumerable<TransactionDto> Transactions { get; set; }

    [Parameter] public EventCallback<TransactionDto> OnDelete { get; set; }

    [Parameter] public EventCallback<TransactionDto> OnEdit { get; set; }

    private bool isPreviewVisible = false;

    private TransactionDto? _selected;
    
    private IQueryable<TransactionDto> Query =>
        Transactions.AsQueryable();

    private void OpenPreview(TransactionDto transaction)
    {
        _selected = transaction;
        isPreviewVisible = true;
    }

    private void ClosePreview()
    {
        _selected = null;
        isPreviewVisible = false;
    }
}