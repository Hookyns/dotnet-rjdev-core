using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace RJDev.Core.Reflection.AssemblyFinder
{
    public class DefaultAssemblyFinder : IAssemblyFinder
    {
        /// <summary>
        /// List of assembly paths its loading failed
        /// </summary>
        private readonly List<(string, Exception)> failedAssemblyPaths = new();

        /// <inheritdoc />
        public IEnumerable<Assembly> GetAssemblies(string searchPattern)
        {
            IEnumerable<string>? assembliesPaths = Directory.GetFiles(AppContext.BaseDirectory, searchPattern, SearchOption.AllDirectories)
                .Where(fileName => fileName.EndsWith(".dll") || fileName.EndsWith(".exe"));

            // List of returned assemblies path; to make it distinct cuz of Ref. assemblies.
            HashSet<string>? returnedAssemblies = new();

            Assembly? assembly = null;

            foreach (var path in assembliesPaths)
            {
                try
                {
                    assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
                }
                catch (Exception ex)
                {
                    this.failedAssemblyPaths.Add((path, ex));
                }

                if (assembly != null && !returnedAssemblies.Contains(assembly.Location))
                {
                    returnedAssemblies.Add(assembly.Location);
                    yield return assembly;
                }
            }
        }

        /// <inheritdoc />
        public IList<(string path, Exception exception)> GetErrors()
        {
            return this.failedAssemblyPaths.ToList();
        }

        /// <summary>
        /// Return true if assembly is from "ref" folder
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool IsRefAssembly(string path)
        {
            return path.Contains(Path.DirectorySeparatorChar + "ref" + Path.DirectorySeparatorChar);
        }
    }
}