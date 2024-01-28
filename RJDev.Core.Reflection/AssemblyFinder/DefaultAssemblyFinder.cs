using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
#if !NETSTANDARD2_0 && !NETSTANDARD2_1
using System.Runtime.Loader;
#endif

namespace RJDev.Core.Reflection.AssemblyFinder
{
    public class DefaultAssemblyFinder : IAssemblyFinder
    {
        /// <summary>
        /// List of assembly paths its loading failed.
        /// </summary>
        private readonly List<(string, Exception)> _failedAssemblyPaths = new();

        /// <inheritdoc />
        public IEnumerable<Assembly> GetAssemblies(string searchPattern)
        {
            IEnumerable<string> assembliesPaths = Directory.GetFiles(AppContext.BaseDirectory, searchPattern, SearchOption.AllDirectories)
                .Where(fileName => fileName.EndsWith(".dll") || fileName.EndsWith(".exe"));

            // List of returned assemblies path; to make it distinct cuz of Ref. assemblies.
            HashSet<string> returnedAssemblies = new();

            Assembly? assembly = null;

            foreach (var path in assembliesPaths)
            {
                try
                {
#if NETSTANDARD2_0 || NETSTANDARD2_1
                    assembly = Assembly.LoadFrom(path);
#else
                    assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
#endif
                }
                catch (Exception ex)
                {
                    _failedAssemblyPaths.Add((path, ex));
                }

                if (assembly != null && returnedAssemblies.Add(assembly.Location))
                {
                    yield return assembly;
                }
            }
        }

        /// <inheritdoc />
        public IList<(string path, Exception exception)> GetErrors()
        {
            return _failedAssemblyPaths.ToList();
        }

        /// <summary>
        /// Return true if assembly is from "ref" folder.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool IsRefAssembly(string path)
        {
            return path.Contains(Path.DirectorySeparatorChar + "ref" + Path.DirectorySeparatorChar);
        }
    }
}