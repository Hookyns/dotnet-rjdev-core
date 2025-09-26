using System;
using System.Diagnostics.CodeAnalysis;

namespace RJDev.Core.Essentials.AppStrings
{
    /// <summary>
    /// Object representing string inside an application.
    /// </summary>
    /// <remarks>
    /// The <see cref="Id"/> property is important. This whole object can be sent from API to the FE, external services etc.
    /// It is possible to identify each AppString and create custom localizations for each unique <see cref="Id"/>.
    /// <see cref="Description"/> contains default message e.g. in english.
    /// </remarks>
    /// <exception cref="ArgumentNullException">If <see cref="Id"/> is null.</exception>
    public record AppString
    {
        /// <summary></summary>
        [SetsRequiredMembers]
        public AppString(string id, string? description = null)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Description = description ?? string.Empty;
        }

        /// <summary></summary>
        public AppString()
        {
        }

        /// <summary/>
        public required string Id { get; init; }
        
        /// <summary/>
        public required string Description { get; init; }

        /// <summary></summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public void Deconstruct(out string id, out string? description)
        {
            id = Id;
            description = Description;
        }
    }
}