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
        ///   Looks up a localized string similar to The specified object of type &apos;{0}&apos; cannot be compared to instance of type &apos;{1}&apos;..
        /// </summary>
        internal static string Comparable_IncomparableType {
            get {
                return ResourceManager.GetString("Comparable_IncomparableType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified value of type &apos;{0}&apos; is not supported by constraint of type &apos;{1}&apos;; expected value of type &apos;{2}&apos;..
        /// </summary>
        internal static string Constraint_UnsupportedType {
            get {
                return ResourceManager.GetString("Constraint_UnsupportedType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot start this scope because a more restrive scope is still active..
        /// </summary>
        internal static string Context_IllegalScopeStarted {
            get {
                return ResourceManager.GetString("Context_IllegalScopeStarted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The scopes were not nested correctly..
        /// </summary>
        internal static string ContextScope_IncorrectNesting {
            get {
                return ResourceManager.GetString("ContextScope_IncorrectNesting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid time span specified: [{0}, {1}]..
        /// </summary>
        internal static string DateTimeSpan_InvalidTimeSpan {
            get {
                return ResourceManager.GetString("DateTimeSpan_InvalidTimeSpan", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; is not allowed to be zero..
        /// </summary>
        internal static string DateTimeSpan_TimeSpanZeroNotAllowed {
            get {
                return ResourceManager.GetString("DateTimeSpan_TimeSpanZeroNotAllowed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified value ({0}) should not be equal to {1}..
        /// </summary>
        internal static string Ensure_ValueIsEqualTo {
            get {
                return ResourceManager.GetString("Ensure_ValueIsEqualTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified value ({0}) should be equal to {1}..
        /// </summary>
        internal static string Ensure_ValueIsNotEqualTo {
            get {
                return ResourceManager.GetString("Ensure_ValueIsNotEqualTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified value ({0}) should be greater than {1}..
        /// </summary>
        internal static string Ensure_ValueIsNotGreaterThan {
            get {
                return ResourceManager.GetString("Ensure_ValueIsNotGreaterThan", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified value ({0}) should be greater than or equal to {1}..
        /// </summary>
        internal static string Ensure_ValueIsNotGreaterThanOrEqualTo {
            get {
                return ResourceManager.GetString("Ensure_ValueIsNotGreaterThanOrEqualTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot obtain single attribute of type &apos;{0}&apos; declared on &apos;{1}&apos; because multiple attributes are of type &apos;{0}&apos;..
        /// </summary>
        internal static string MemberInfoExtensions_AmbiguousAttributeMatch {
            get {
                return ResourceManager.GetString("MemberInfoExtensions_AmbiguousAttributeMatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is no element at index &apos;{0}&apos; (Count = {1})..
        /// </summary>
        internal static string ReadOnlyList_IndexOutOfRange {
            get {
                return ResourceManager.GetString("ReadOnlyList_IndexOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not deserialize instance of type &apos;{0}&apos;. See inner exception for details..
        /// </summary>
        internal static string Serializer_DeserializationFailed {
            get {
                return ResourceManager.GetString("Serializer_DeserializationFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not serialize instance of type &apos;{0}&apos;. See inner exception for details..
        /// </summary>
        internal static string Serializer_SerializationFailed {
            get {
                return ResourceManager.GetString("Serializer_SerializationFailed", resourceCulture);
            }
        }
    }
}
