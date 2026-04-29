using System;
using System.Collections.Generic;
using System.Reflection;

namespace RJDev.Core.Reflection.AssemblyFinder
{
    /// <summary>
    /// Finds assemblies in application execution path by path pattern and loads them.
    /// </summary>
    public interface IAssemblyFinder
    {
        /// <summary>
        /// Returns assemblies from application execution path matched by path pattern
        /// </summary>
        /// <remarks>
        /// Support base wildcard (* and ?) characters.
        /// </remarks>
        /// <param name="searchPattern">The search string to match against the names of files in path. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
        /// <param name="filter">Optional predicate to decide which matched assembly file paths should be loaded. If null, all matched assemblies are loaded.</param>
        /// <returns></returns>
        IEnumerable<Assembly> GetAssemblies(
            string searchPattern,
            Func<string, bool>? filter = null
        );

        /// <summary>
        /// Return list of failed loadings
        /// </summary>
        /// <returns></returns>
        IList<(string path, Exception exception)> GetErrors();
    }
}
