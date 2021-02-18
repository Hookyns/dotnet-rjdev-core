using System;
using System.Collections.Generic;
using System.Reflection;

namespace RJDev.Core.Reflection.AssemblyFinder
{
    public interface IAssemblyFinder
    {
        /// <summary>
        /// Returns assemblies from application execution path matched by path pattern
        /// </summary>
        /// <remarks>
        /// Support base wildcard (* and ?) characters.
        /// </remarks>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        IEnumerable<Assembly> GetAssemblies(string searchPattern);

        /// <summary>
        /// Return list of failed loadings
        /// </summary>
        /// <returns></returns>
        IList<(string path, Exception exception)> GetErrors();
    }
}