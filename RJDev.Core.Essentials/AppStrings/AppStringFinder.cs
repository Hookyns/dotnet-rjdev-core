using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RJDev.Core.Reflection.AssemblyFinder;

namespace RJDev.Core.Essentials.AppStrings
{
    public class AppStringFinder : IAppStringFinder
    {
        private readonly IAssemblyFinder _assemblyFinder;

        private List<AppString> _appStrings = new();
        private Dictionary<string, AppString> _appStringsMap = new();

        private bool _initiated;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="assemblyFinder"></param>
        public AppStringFinder(IAssemblyFinder assemblyFinder)
        {
            _assemblyFinder = assemblyFinder;
        }

        private void EnsureInit(Assembly[] assemblies)
        {
            if (_initiated)
            {
                return;
            }

            _appStrings = GetAll(assemblies).ToList();
            _appStringsMap = _appStrings.ToDictionary(x => x.Id, x => x);
            _initiated = true;
        }

        /// <inheritdoc />
        public IEnumerable<AppString> GetAllAppStrings(bool cached = true, Assembly[]? assemblies = null)
        {
            assemblies ??= _assemblyFinder.GetAssemblies("*").ToArray();

            if (cached)
            {
                EnsureInit(assemblies);
                return _appStrings;
            }

            return GetAll(assemblies);
        }

        /// <inheritdoc />
        public AppString GetAppString(string id, Assembly[]? assemblies = null)
        {
            assemblies ??= _assemblyFinder.GetAssemblies("*").ToArray();
            EnsureInit(assemblies);
            return _appStringsMap.TryGetValue(id, out AppString? appString) ? appString : new AppString(string.Empty, string.Empty);
        }

        private IEnumerable<AppString> GetAll(Assembly[] assemblies)
        {
            Type[] staticClasses = assemblies.SelectMany(a => a.GetTypes())
                .Where(type => type.IsClass && type.IsSealed && type.IsAbstract)
                .ToArray();

            // All the fields of type AppString
            FieldInfo[] allResultMessageFields = staticClasses
                .SelectMany(type => type.GetFields())
                .Where(field => field.FieldType == typeof(AppString))
                .ToArray();

            foreach (FieldInfo field in allResultMessageFields)
            {
                if (field.GetValue(null) is AppString message)
                {
                    yield return message;
                }
            }
        }
    }
}