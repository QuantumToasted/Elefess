namespace Elefess.Models;

/// <summary>
/// A Git LFS batch transfer adapter.
/// </summary>
public sealed class LfsTransferAdapter
{
    /// <summary>
    /// The name, or "type", of the adapter.
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// An <see cref="LfsTransferAdapter"/> that represents the <c>basic</c> transfer adapter.
    /// </summary>
    public static LfsTransferAdapter Basic => new() { Type = LfsUtil.Constants.TransferAdapters.BASIC };

#pragma warning disable CS1591
    public override bool Equals(object? obj) => (obj as LfsTransferAdapter)?.Type == Type;
    public override int GetHashCode() => Type.GetHashCode();
    public static bool operator ==(LfsTransferAdapter l, LfsTransferAdapter r) => l.Type == r.Type;
    public static bool operator !=(LfsTransferAdapter l, LfsTransferAdapter r) => l.Type != r.Type;
    public static bool operator ==(LfsTransferAdapter l, string r) => l.Type == r;
    public static bool operator !=(LfsTransferAdapter l, string r) => l.Type != r;
    public static bool operator ==(string l, LfsTransferAdapter r) => l == r.Type;
    public static bool operator !=(string l, LfsTransferAdapter r) => l != r.Type;
    public static implicit operator string(LfsTransferAdapter transferAdapter) => transferAdapter.Type;
    public static implicit operator LfsTransferAdapter(string s) => new() { Type = s };
#pragma warning restore CS1591
}