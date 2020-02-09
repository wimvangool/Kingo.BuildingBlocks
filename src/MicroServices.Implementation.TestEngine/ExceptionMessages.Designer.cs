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
        ///   Looks up a localized string similar to The specified expected length of the stream is not valid: {0}..
        /// </summary>
        internal static string MessageHandlerOperationResult_InvalidStreamLengthSpecified {
            get {
                return ResourceManager.GetString("MessageHandlerOperationResult_InvalidStreamLengthSpecified", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The length of the stream ({0}) doesn&apos;t match the expected length ({1})..
        /// </summary>
        internal static string MessageHandlerOperationResult_UnexpectedStreamLength {
            get {
                return ResourceManager.GetString("MessageHandlerOperationResult_UnexpectedStreamLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to One or more assertions for the published messages failed. See inner exception for details. .
        /// </summary>
        internal static string MessageHandlerOperationTestResult_AssertionOfMessageStreamFailed {
            get {
                return ResourceManager.GetString("MessageHandlerOperationTestResult_AssertionOfMessageStreamFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Index cannot be negative: {0}..
        /// </summary>
        internal static string MessageStream_IndexOutOfRange {
            get {
                return ResourceManager.GetString("MessageStream_IndexOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Message of type &apos;{0}&apos; was not found at index &apos;{1}&apos;; stream contains {2} message(s) of type &apos;{0}&apos;..
        /// </summary>
        internal static string MessageStream_MessageNotFound {
            get {
                return ResourceManager.GetString("MessageStream_MessageNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot invoke &apos;{0}&apos; at this point: the processor has already been configured..
        /// </summary>
        internal static string MicroProcessorConfiguration_ProcessorAlreadyConfigured {
            get {
                return ResourceManager.GetString("MicroProcessorConfiguration_ProcessorAlreadyConfigured", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot invoke &apos;{0}&apos; at this point: the processor has not yet been configured..
        /// </summary>
        internal static string MicroProcessorConfiguration_ProcessorNotYetConfigured {
            get {
                return ResourceManager.GetString("MicroProcessorConfiguration_ProcessorNotYetConfigured", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot invoke &apos;{0}&apos; at this point: the service collection has already been configured..
        /// </summary>
        internal static string MicroProcessorConfiguration_ServicesAlreadyConfigured {
            get {
                return ResourceManager.GetString("MicroProcessorConfiguration_ServicesAlreadyConfigured", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot move to state &apos;{0}&apos; because the test was not in the expected state &apos;{1}&apos;..
        /// </summary>
        internal static string MicroProcessorTest_CannotMoveToState {
            get {
                return ResourceManager.GetString("MicroProcessorTest_CannotMoveToState", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot invoke method &apos;{0}.{1}()&apos; while the test is in state &apos;{2}&apos;. Make sure you correctly follow the builder pattern provided by the framework..
        /// </summary>
        internal static string MicroProcessorTest_InvalidOperation {
            get {
                return ResourceManager.GetString("MicroProcessorTest_InvalidOperation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot invoke &apos;{0}.{1}()&apos; while the test is in state &apos;{2}&apos;. Make sure &apos;{0}.{1}.()&apos; is invoked only once before the test is configured and executed..
        /// </summary>
        internal static string MicroProcessorTest_SetupFailed {
            get {
                return ResourceManager.GetString("MicroProcessorTest_SetupFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot invoke &apos;{0}.{1}()&apos; while the test is in state &apos;{2}&apos;. Make sure &apos;{0}.{1}()&apos; is invoked only once after the test has ran to completion..
        /// </summary>
        internal static string MicroProcessorTest_TearDownFailed {
            get {
                return ResourceManager.GetString("MicroProcessorTest_TearDownFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Test failed because an exception of type &apos;{0}&apos; was thrown while configuring an operation..
        /// </summary>
        internal static string MicroProcessorTestContext_ConfigureOperationFailed {
            get {
                return ResourceManager.GetString("MicroProcessorTestContext_ConfigureOperationFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not resolve message-handler of type &apos;{0}&apos;. Please make sure all message-handers are registered in the service-collection..
        /// </summary>
        internal static string MicroProcessorTestContext_CouldNotResolveMessageHandler {
            get {
                return ResourceManager.GetString("MicroProcessorTestContext_CouldNotResolveMessageHandler", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Test failed because message of type &apos;{0}&apos; was not set by the configurator..
        /// </summary>
        internal static string MicroProcessorTestContext_MessageNotSet {
            get {
                return ResourceManager.GetString("MicroProcessorTestContext_MessageNotSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Test failed because request of type &apos;{0}&apos; was not set by the configurator..
        /// </summary>
        internal static string MicroProcessorTestContext_RequestNotSet {
            get {
                return ResourceManager.GetString("MicroProcessorTestContext_RequestNotSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot save the MessageStream of &apos;{0}&apos; because a previous result of this test has already been saved..
        /// </summary>
        internal static string MicroProcessorTestContext_TestAlreadyRun {
            get {
                return ResourceManager.GetString("MicroProcessorTestContext_TestAlreadyRun", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot retrieve the result of &apos;{0}&apos; because its results haven&apos;t been saved in this context..
        /// </summary>
        internal static string MicroProcessorTestContext_TestResultNotFound {
            get {
                return ResourceManager.GetString("MicroProcessorTestContext_TestResultNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to One or more assertions for exception of type &apos;{0}&apos; failed. See inner exception for details..
        /// </summary>
        internal static string MicroProcessorTestResult_AssertionOfExceptionFailed {
            get {
                return ResourceManager.GetString("MicroProcessorTestResult_AssertionOfExceptionFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expected an exception of type &apos;{0}&apos;, but encountered an exception of type &apos;{1}&apos; instead..
        /// </summary>
        internal static string MicroProcessorTestResult_ExceptionNotOfExpectedType {
            get {
                return ResourceManager.GetString("MicroProcessorTestResult_ExceptionNotOfExpectedType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The expected exception of type &apos;{0}&apos; was not thrown..
        /// </summary>
        internal static string MicroProcessorTestResult_ExceptionNotThrown {
            get {
                return ResourceManager.GetString("MicroProcessorTestResult_ExceptionNotThrown", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unexpected exception of type &apos;{0}&apos; was thrown..
        /// </summary>
        internal static string MicroProcessorTestResult_ExceptionThrown {
            get {
                return ResourceManager.GetString("MicroProcessorTestResult_ExceptionThrown", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exception of type &apos;{0}&apos; was expected to have an inner-exception of type &apos;{1}&apos;, but did not have any inner-exception..
        /// </summary>
        internal static string MicroProcessorTestResult_InnerExceptionNull {
            get {
                return ResourceManager.GetString("MicroProcessorTestResult_InnerExceptionNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Test failed by default because its result was not verified..
        /// </summary>
        internal static string MicroProcessorTestResult_ResultNotVerified {
            get {
                return ResourceManager.GetString("MicroProcessorTestResult_ResultNotVerified", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot set time to &apos;{0}&apos; here because the current time is set to &apos;{1}&apos; and you are not allowed to go backwards in time during a test..
        /// </summary>
        internal static string MicroProcessorTestTimeline_InvalidTime {
            get {
                return ResourceManager.GetString("MicroProcessorTestTimeline_InvalidTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot move the clock backwards in a test by specifying a negative value ({0})..
        /// </summary>
        internal static string MicroProcessorTestTimeline_NegativeTimeSpan {
            get {
                return ResourceManager.GetString("MicroProcessorTestTimeline_NegativeTimeSpan", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot move the clock backwards in a test by specifying a negative value ({0})..
        /// </summary>
        internal static string MicroProcessorTestTimeline_NegativeTimeUnit {
            get {
                return ResourceManager.GetString("MicroProcessorTestTimeline_NegativeTimeUnit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot set time to &apos;{0}&apos; here because the test already runs in relative time, which may cause a jump backwards in time..
        /// </summary>
        internal static string MicroProcessorTestTimeline_SpecificTimeNotAllowed {
            get {
                return ResourceManager.GetString("MicroProcessorTestTimeline_SpecificTimeNotAllowed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot move timeline to state &apos;{0}&apos; because the timeline was not in the expected state &apos;{1}&apos;..
        /// </summary>
        internal static string MicroProcessorTestTimeline_UnexpectedState {
            get {
                return ResourceManager.GetString("MicroProcessorTestTimeline_UnexpectedState", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Test &apos;{0}&apos; did not produce any result. Please verify that the processor provided as an argument to the WhenAsync-method has been used to handle a message or execute a query..
        /// </summary>
        internal static string NullTestResult_MissingResult {
            get {
                return ResourceManager.GetString("NullTestResult_MissingResult", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Test &apos;{0}&apos; threw unexpected exception of type &apos;{1}&apos;. See inner exception for details..
        /// </summary>
        internal static string UnexpectedExceptionResult_UnexpectedException {
            get {
                return ResourceManager.GetString("UnexpectedExceptionResult_UnexpectedException", resourceCulture);
            }
        }
    }
}
