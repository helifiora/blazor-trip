using BlazorTrip.Domain.Models;

namespace BlazorTrip.Application.Dto;

public record PersonReportDto(
    Person Person,
    decimal TotalSpent,
    decimal Balance,
    IEnumerable<PersonReportShareDto> TransactionShares
);

public record PersonReportDetailDto(
    List<ReportShareDto> Shares,
    decimal ShareTotal
);

public record PersonReportShareDto
{
    public Person PersonToPay { get; }
    public decimal ValueToPay { get; }
    public List<ReportShareDto> Shares { get; }
    public List<ReportShareDto> SharesToPay { get; }
    public decimal SharesToPayTotal { get; }
    public List<ReportShareDto> SharesToReceive { get; }
    public decimal SharesToReceiveTotal { get; }

    public PersonReportShareDto(Person personToPay, decimal valueToPay, List<ReportShareDto> shares)
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