using Elefess.Models;

namespace Elefess;

public sealed class BasicLfsTransferRequestAdapter : ILfsTransferAdapter
{
    public Task<LfsTransfer> SelectTransferAsync(LfsBatchTransferRequest request, CancellationToken cancellationToken)
    {
        if (request.RequestedTransferTypes.FirstOrDefault(x => x == LfsTransfer.Basic) is not { } selectedTransferType)
        {
            throw new ArgumentOutOfRangeException(nameof(request),
                $"Only the \"{LfsUtil.Constants.TransferAdapters.BASIC}\" transfer adapter is supported.");
        }

        return Task.FromResult(selectedTransferType);
    }
}