using System;

namespace RJDev.Core.AppUtils.AppStrings
{
    /// <summary>
    /// Object representing string inside an application.
    /// </summary>
    /// <remarks>
    /// The <see cref="Id"/> property is important. This whole object can be sent from API to the FE, external services etc.
    /// It is possible to identify each AppString and create custom localizations for each unique <see cref="Id"/>.
    /// <see cref="Description"/> contains default message eg. in english.
    /// </remarks>
    /// <param name="Id">Text/message/error identifier which should be unique. Can be used as localization key.</param>
    /// <param name="Description">Default text of the message.</param>
    /// <exception cref="ArgumentNullException">If <see cref="Id"/> is null.</exception>
    public record AppString(string Id, string? Description = null)
    {
        /// <summary/>
        public string Id { get; } = Id ?? throw new ArgumentNullException(nameof(Id));
        
        /// <summary/>
        public string Description { get; } = Description ?? string.Empty;
    }
}