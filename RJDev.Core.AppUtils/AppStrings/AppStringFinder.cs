using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RJDev.Core.Reflection.AssemblyFinder;

namespace RJDev.Core.AppUtils.AppStrings
{
    public class AppStringFinder : IAppStringFinder
    {
        private readonly IAssemblyFinder assemblyFinder;

        private List<AppString> appStrings = new();
        private Dictionary<string, AppString> appStringsMap = new();

        private bool initiated;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="assemblyFinder"></param>
        public AppStringFinder(IAssemblyFinder assemblyFinder)
        {
            this.assemblyFinder = assemblyFinder;
        }

        private void EnsureInit(Assembly[] assemblies)
        {
            if (this.initiated)
            {
                return;
            }

            this.appStrings = this.GetAll(assemblies).ToList();
            this.appStringsMap = this.appStrings.ToDictionary(x => x.Id, x => x);
            this.initiated = true;
        }

        /// <inheritdoc />
        public IEnumerable<AppString> GetAllAppStrings(bool cached = true, Assembly[]? assemblies = null)
        {
            assemblies ??= this.assemblyFinder.GetAssemblies("*").ToArray();

            if (cached)
            {
                this.EnsureInit(assemblies);
                return this.appStrings;
            }

            return this.GetAll(assemblies);
        }

        /// <inheritdoc />
        public AppString GetAppString(string id, Assembly[]? assemblies = null)
        {
            assemblies ??= this.assemblyFinder.GetAssemblies("*").ToArray();
            this.EnsureInit(assemblies);
            return this.appStringsMap.TryGetValue(id, out AppString? appString) ? appString : new AppString(string.Empty, string.Empty);
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