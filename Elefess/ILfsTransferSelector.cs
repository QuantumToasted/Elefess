using Elefess.Models;

namespace Elefess;

/// <summary>
/// Represents a Git LFS transfer adapter selector, responsible for choosing the appropriate transfer adapter for a batch request.
/// </summary>
public interface ILfsTransferSelector
{
    /// <summary>
    /// Selects the appropriate <see cref="LfsTransferAdapter"/> method for a batch request.
    /// </summary>
    /// <param name="requestedTransferAdapters">A collection of <see cref="LfsTransferAdapter"/>s the client supports or is requesting.</param>
    /// <param name="cancellationToken">The cancellation token for the request.</param>
    /// <returns>A <see cref="Task"/> representing the agreed-upon <see cref="LfsTransferAdapter"/> that both the client and server support.</returns>
    /// <remarks>This method should throw an <see cref="Exception"/> if an appropriate transfer adapter cannot be selected.</remarks>
    Task<LfsTransferAdapter> SelectTransferAsync(ICollection<LfsTransferAdapter> requestedTransferAdapters, CancellationToken cancellationToken);
}