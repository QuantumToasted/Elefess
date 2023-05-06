using System.Diagnostics.CodeAnalysis;
using Elefess.Models;

namespace Elefess;

public interface ILfsTransferAdapter
{
    Task<LfsTransfer> SelectTransferAsync(LfsBatchTransferRequest request, CancellationToken cancellationToken);
}