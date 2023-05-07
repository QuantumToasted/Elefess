namespace Elefess.Models;

/// <summary>
/// A Git LFS batch transfer adapter.
/// </summary>
/// <param name="Type">The name, or "type", of the adapter.</param>
public sealed record LfsTransferAdapter(string Type)
{
    /// <summary>
    /// An <see cref="LfsTransferAdapter"/> that represents the <c>basic</c> transfer adapter.
    /// </summary>
    public static LfsTransferAdapter Basic => new(LfsUtil.Constants.TransferAdapters.BASIC);

#pragma warning disable CS1591
    public static implicit operator string(LfsTransferAdapter transferAdapter)
        => transferAdapter.Type;

    public static implicit operator LfsTransferAdapter(string s)
        => new(s);
#pragma warning restore CS1591
}