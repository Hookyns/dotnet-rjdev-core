using RJDev.Core.Essentials.AppStrings;

namespace RJDev.Core.Essentials.Results;

/// <summary>
/// Details about the error that occurred
/// </summary>
/// <param name="Message"></param>
/// <param name="Args"></param>
public record ResultError(AppString Message, params object[] Args);