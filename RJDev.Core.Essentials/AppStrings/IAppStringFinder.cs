using System.Collections.Generic;
using System.Reflection;

namespace RJDev.Core.Essentials.AppStrings;

/// <summary>
/// Interface for the service looking for all the statically declared <see cref="AppString"/> in application
/// so we know all the messages used in source code.
/// </summary>
public interface IAppStringFinder
{
	/// <summary>
	/// Returns collection of all the <see cref="AppString"/>s declared in the application.
	/// </summary>
	/// <param name="cached">Use cached rasult or look for all the AppStrings again.</param>
	/// <param name="assemblies">Which assemblies should be used. All assemblies in the bin will be used if no assemblies specified.</param>
	/// <returns></returns>
	IEnumerable<AppString> GetAllAppStrings(bool cached = true, Assembly[]? assemblies = null);

	/// <summary>
	/// Return <see cref="AppString"/> by its <see cref="AppString.Id"/> property.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="assemblies">Which assemblies should be used. All assemblies in the bin will be used if no assemblies specified.</param>
	/// <returns></returns>
	AppString GetAppString(string id, Assembly[]? assemblies = null);
}