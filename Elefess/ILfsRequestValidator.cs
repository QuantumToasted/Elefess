using Elefess.Models;

namespace Elefess;

public interface ILfsRequestValidator
{
    Task ValidateAsync(LfsBatchTransferRequest request);
}