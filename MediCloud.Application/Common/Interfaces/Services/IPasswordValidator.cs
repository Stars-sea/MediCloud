using MediCloud.Domain.Common.Contracts;

namespace MediCloud.Application.Common.Interfaces.Services;

public interface IPasswordValidator {

    Task<Result> ValidateAsync(string password);

}
