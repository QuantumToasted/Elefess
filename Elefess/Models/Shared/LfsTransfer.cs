namespace Elefess.Models;

public sealed record LfsTransfer(string Type)
{
    public static LfsTransfer Basic => new(LfsUtil.Constants.TransferAdapters.BASIC);

    public static implicit operator string(LfsTransfer transfer)
        => transfer.Type;

    public static implicit operator LfsTransfer(string s)
        => new(s);
}