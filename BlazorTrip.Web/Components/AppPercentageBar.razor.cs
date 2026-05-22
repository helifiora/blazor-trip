using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace BlazorTrip.Web.Components;

public partial class AppPercentageBar : ComponentBase
{
    [Parameter] public decimal Value { get; set; } = 0;

    [Parameter] public decimal Max { get; set; } = 100;

    [Parameter] public decimal Min { get; set; } = 0;

    private decimal Percentage
    {
        get
        {
            if (Max == 0) return 0;
            return Math.Clamp(Math.Round((Value / Max) * 100, 2), 0, 100);
        }
    }

    private string PercentageString => $"{Percentage.ToString(CultureInfo.InvariantCulture)}%";
}