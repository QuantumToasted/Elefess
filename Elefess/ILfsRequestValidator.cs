using Elefess.Models;

namespace Elefess;

/// <summary>
/// Represents an optional Git LFS request validator, responsible for performing additional validation or checks on a batch request.
/// </summary>
public interface ILfsRequestValidator
{
    /// <summary>
    /// Validates an LFS batch request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <returns>A <see cref="Task"/> representing the validation work.</returns>
    /// <remarks>This method should throw an <see cref="Exception"/> if validation fails.</remarks>
    Task ValidateAsync(LfsBatchTransferRequest request);
}