using MediCloud.Domain.Common.Errors;

namespace MediCloud.Application.Common.Interfaces.Services;

public interface IPasswordValidator {

    Task<IList<Error>> ValidateAsync(string password);

}
