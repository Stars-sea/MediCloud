using MediCloud.Application.Common.Interfaces.Services;

namespace MediCloud.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider {

    public DateTime UtcNow => DateTime.UtcNow;

}
