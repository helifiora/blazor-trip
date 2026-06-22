using System.ComponentModel.DataAnnotations;
using BlazorTrip.Domain.Models;

namespace BlazorTrip.Application.Dto;

public class CreateCategoryDto
{
    [Required] public string Name { get; set; } = string.Empty;

    [Required] public string Logo { get; set; } = string.Empty;

    public Category CreateCategory()
    {
        return Category.Create(Name, Logo);
    }
}