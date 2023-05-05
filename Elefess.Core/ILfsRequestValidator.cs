using Elefess.Core.Models;

namespace Elefess.Core;

public interface ILfsRequestValidator
{
    Task ValidateAsync(LfsBatchTransferRequest request);
}