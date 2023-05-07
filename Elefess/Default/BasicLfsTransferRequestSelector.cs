using Elefess.Models;

namespace Elefess;

/// <summary>
/// A Git LFS transfer adapter selector which only supports the <c>basic</c> transfer adapter.
/// </summary>
public sealed class BasicLfsTransferRequestSelector : ILfsTransferSelector
{
    /// <inheritdoc />
    public Task<LfsTransferAdapter> SelectTransferAsync(ICollection<LfsTransferAdapter> requestedTransferAdapters, CancellationToken cancellationToken)
    {
        if (requestedTransferAdapters.FirstOrDefault(x => x == LfsTransferAdapter.Basic) is not { } selectedTransferType)
        {
            throw new ArgumentOutOfRangeException(nameof(requestedTransferAdapters),
                $"Only the \"{LfsUtil.Constants.TransferAdapters.BASIC}\" transfer adapter is supported.");
        }

        return Task.FromResult(selectedTransferType);
    }
}