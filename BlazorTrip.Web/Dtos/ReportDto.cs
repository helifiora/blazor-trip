using BlazorTrip.Domain;

namespace BlazorTrip.Web.Dtos;

public record ReportDto(
    Person Person,
    decimal TotalSpent,
    decimal Balance,
    IEnumerable<ReportPayerDto> TransactionShares
);

public record ReportPayerDto
{
    public Person PersonToPay { get;  }
    public decimal ValueToPay { get;  }
    public List<ReportShareDto> Shares { get;  }
    public List<ReportShareDto> SharesToPay { get;  }
    public decimal SharesToPayTotal { get; }
    
    public List<ReportShareDto> SharesToReceive { get; }
    public decimal SharesToReceiveTotal { get; }

    public ReportPayerDto(Person personToPay, decimal valueToPay, List<ReportShareDto> shares)
    {
        PersonToPay = personToPay;
        ValueToPay = valueToPay;
        Shares = shares;

        SharesToPay = shares.Where(s => s.PersonToPay.Id == personToPay.Id).ToList();
        SharesToPayTotal = SharesToPay.Sum(s => s.SharedAmount);

        SharesToReceive = shares.Where(s => s.PersonToPay.Id != personToPay.Id).ToList();
        SharesToReceiveTotal = SharesToReceive.Sum(s => s.SharedAmount);
    }
}

public record ReportShareDto(
    TransactionDto Transaction,
    Person PersonToPay,
    Person PersonToReceive,
    decimal SharedAmount
);