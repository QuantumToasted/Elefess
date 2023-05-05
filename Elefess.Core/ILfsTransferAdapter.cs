using System.Diagnostics.CodeAnalysis;
using Elefess.Core.Models;

namespace Elefess.Core;

public interface ILfsTransferAdapter
{
    Task<LfsTransfer> SelectTransferAsync(LfsBatchTransferRequest request, CancellationToken cancellationToken);
}