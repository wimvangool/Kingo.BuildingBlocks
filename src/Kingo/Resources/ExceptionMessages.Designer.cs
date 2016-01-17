﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kingo.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Kingo.Resources.ExceptionMessages", typeof(ExceptionMessages).Assembly);
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
        ///   Looks up a localized string similar to WithErrorMessage is not supported by constraint &apos;{0}&apos;..
        /// </summary>
        internal static string Constraint_WithErrorMessageNotSupported {
            get {
                return ResourceManager.GetString("Constraint_WithErrorMessageNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to WithName is not supported by constraint &apos;{0}&apos;..
        /// </summary>
        internal static string Constraint_WithNameNotSupported {
            get {
                return ResourceManager.GetString("Constraint_WithNameNotSupported", resourceCulture);
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
        ///   Looks up a localized string similar to Value of member &apos;{0}.{1}&apos; of type &apos;{2}&apos; could not be converted to an instance of type &apos;{3}&apos;..
        /// </summary>
        internal static string DomainEvent_IncompatibleMemberType {
            get {
                return ResourceManager.GetString("DomainEvent_IncompatibleMemberType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not resolve member of type &apos;{0}&apos; on event of type &apos;{1}&apos;. Consider overriding the Key and Version properties..
        /// </summary>
        internal static string DomainEvent_MemberNotFound {
            get {
                return ResourceManager.GetString("DomainEvent_MemberNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not decide on a member of type &apos;{0}&apos; on event of type &apos;{1}&apos; because multiple candidate members were found. Consider overriding the Key and Version properties..
        /// </summary>
        internal static string DomainEvent_MultipleCandidateMembersFound {
            get {
                return ResourceManager.GetString("DomainEvent_MultipleCandidateMembersFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Command &apos;{0}&apos; failed..
        /// </summary>
        internal static string DomainModelException_CommandFailed {
            get {
                return ResourceManager.GetString("DomainModelException_CommandFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while processing event &apos;{0}&apos;..
        /// </summary>
        internal static string DomainModelException_EventFailed {
            get {
                return ResourceManager.GetString("DomainModelException_EventFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Instance &apos;{0}&apos; of type &apos;{1}&apos; is not supported because it is not an Enum type..
        /// </summary>
        internal static string EnumIsInRangeOfValidValues_UnsupportedValue {
            get {
                return ResourceManager.GetString("EnumIsInRangeOfValidValues_UnsupportedValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ErrorLevel must be 0 or higher: {0}..
        /// </summary>
        internal static string ErrorLevel_InvalidErrorLevel {
            get {
                return ResourceManager.GetString("ErrorLevel_InvalidErrorLevel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified node or expression is not supported inside a member expression: &apos;{0}&apos;..
        /// </summary>
        internal static string ExpressionBuilder_ExpressionNotSupported {
            get {
                return ResourceManager.GetString("ExpressionBuilder_ExpressionNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Specified expression is not supported: &apos;{0}&apos;..
        /// </summary>
        internal static string ExpressionExtensions_UnsupportedExpression {
            get {
                return ResourceManager.GetString("ExpressionExtensions_UnsupportedExpression", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Instance of type &apos;{0}&apos; does not contain an indexer with argument types &apos;[{1}]&apos;..
        /// </summary>
        internal static string HasItemFilter_IndexerNotFound {
            get {
                return ResourceManager.GetString("HasItemFilter_IndexerNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified values are not valid to index an array: {0}..
        /// </summary>
        internal static string HasItemFilter_InvalidArrayIndexValues {
            get {
                return ResourceManager.GetString("HasItemFilter_InvalidArrayIndexValues", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot create an empty identifier..
        /// </summary>
        internal static string Identifier_EmptyIdentifier {
            get {
                return ResourceManager.GetString("Identifier_EmptyIdentifier", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid identifier specified: &apos;{0}&apos;..
        /// </summary>
        internal static string Identifier_InvalidIdentifier {
            get {
                return ResourceManager.GetString("Identifier_InvalidIdentifier", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Two or more attributes declared on message &apos;{0}&apos; are assignable to &apos;{1}&apos;..
        /// </summary>
        internal static string Message_AmbiguousAttributeMatch {
            get {
                return ResourceManager.GetString("Message_AmbiguousAttributeMatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified range is not valid: {0}..
        /// </summary>
        internal static string Range_InvalidRange {
            get {
                return ResourceManager.GetString("Range_InvalidRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified key &apos;{0}&apos; is not a valid index for this collection..
        /// </summary>
        internal static string ReadOnlyDictionary_KeyNotFound {
            get {
                return ResourceManager.GetString("ReadOnlyDictionary_KeyNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot complete this scope because it is not the current scope..
        /// </summary>
        internal static string Scope_CannotCompleteScope {
            get {
                return ResourceManager.GetString("Scope_CannotCompleteScope", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The scopes were not nested correctly..
        /// </summary>
        internal static string Scope_IncorrectNesting {
            get {
                return ResourceManager.GetString("Scope_IncorrectNesting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The scope has already completed..
        /// </summary>
        internal static string Scope_ScopeAlreadyCompleted {
            get {
                return ResourceManager.GetString("Scope_ScopeAlreadyCompleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid format string specified: &apos;{0}&apos;. Missing closing brace of variable at index {1}..
        /// </summary>
        internal static string StringTemplate_MissingClosingBrace {
            get {
                return ResourceManager.GetString("StringTemplate_MissingClosingBrace", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid format string specified: &apos;{0}&apos;. Missing identifier at index {1}..
        /// </summary>
        internal static string StringTemplate_MissingIdentifier {
            get {
                return ResourceManager.GetString("StringTemplate_MissingIdentifier", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid format string specified: &apos;{0}&apos;. Unexpected character &apos;{1}&apos; at index {2}..
        /// </summary>
        internal static string StringTemplate_UnexpectedCharacter {
            get {
                return ResourceManager.GetString("StringTemplate_UnexpectedCharacter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Instance of type &apos;{0}&apos; does not contain public field or property &apos;{1}&apos;..
        /// </summary>
        internal static string StringTemplateVariable_MemberNotFound {
            get {
                return ResourceManager.GetString("StringTemplateVariable_MemberNotFound", resourceCulture);
            }
        }
    }
}
