using RJDev.Core.Essentials.AppStrings;

namespace RJDev.Core.Essentials.Results;

public record ResultError(AppString Message, params object[] Args);