﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kingo {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ExceptionMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExceptionMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Kingo.ExceptionMessages", typeof(ExceptionMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Name &apos;{0}&apos; contains illegal character &apos;{1}&apos; encountered at index &apos;{2}&apos;..
        /// </summary>
        internal static string EndpointNameFormat_IllegalCharacter {
            get {
                return ResourceManager.GetString("EndpointNameFormat_IllegalCharacter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Name &apos;{0}&apos; is missing the closing bracket for placeholder started at index &apos;{1}&apos;..
        /// </summary>
        internal static string EndpointNameFormat_MissingClosingBracket {
            get {
                return ResourceManager.GetString("EndpointNameFormat_MissingClosingBracket", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown placeholder specified: &apos;{0}&apos;..
        /// </summary>
        internal static string EndpointNameFormat_UnknownPlaceholderName {
            get {
                return ResourceManager.GetString("EndpointNameFormat_UnknownPlaceholderName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot connect endpoint &apos;{0}&apos; (Name = {1}) to the service-bus because it is not supported by this client..
        /// </summary>
        internal static string MicroServiceBusClient_EndpointNotSupported {
            get {
                return ResourceManager.GetString("MicroServiceBusClient_EndpointNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Controller of type &apos;{0}&apos; cannot be started because it has already been started..
        /// </summary>
        internal static string MicroServiceBusController_AlreadyStarted {
            get {
                return ResourceManager.GetString("MicroServiceBusController_AlreadyStarted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Controller of type &apos;{0}&apos; cannot publish the specified event(s) because it has not been (fully) started yet..
        /// </summary>
        internal static string MicroServiceBusController_CannotPublishEvents {
            get {
                return ResourceManager.GetString("MicroServiceBusController_CannotPublishEvents", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Controller of type &apos;{0}&apos; cannot send the specified command(s) because it has not been (fully) started yet..
        /// </summary>
        internal static string MicroServiceBusController_CannotSendCommands {
            get {
                return ResourceManager.GetString("MicroServiceBusController_CannotSendCommands", resourceCulture);
            }
        }
    }
}
