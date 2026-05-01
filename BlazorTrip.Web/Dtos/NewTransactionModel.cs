using System.ComponentModel.DataAnnotations;

namespace BlazorTrip.Web.Dtos;

public class NewTransactionModel
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [MinLength(8, ErrorMessage = "Nome deve ter, no mínimo, 8 caracteres")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Categoria é obrigatória")]
    public Guid CategoryId { get; set; }

    [Required(ErrorMessage = "Pagador é obrigatório")]
    public Guid PayerId { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    [Required]
    [MinLength(1, ErrorMessage = "Deve pelo menos um para dividir")]
    public HashSet<Guid> SharedIds { get; set; } = [];

    [Required] public decimal Amount { get; set; }

    public static NewTransactionModel NewWithSharedAdded(IEnumerable<Guid> sharedIds)
    {
        return new NewTransactionModel
        {
            SharedIds = sharedIds.ToHashSet()
        };
    }

    public void OnToggleShared(Guid sharedId)
    {
        if (SharedIds.Contains(sharedId))
        {
            SharedIds.Remove(sharedId);
        }
        else
        {
            SharedIds.Add(sharedId);
        }
    }
}