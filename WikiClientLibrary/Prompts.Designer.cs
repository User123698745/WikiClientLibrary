﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WikiClientLibrary {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Prompts {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Prompts() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WikiClientLibrary.Prompts", typeof(Prompts).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性
        ///   重写当前线程的 CurrentUICulture 属性。
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
        ///   查找类似 Argument should not contain pipe character ( | ). 的本地化字符串。
        /// </summary>
        internal static string ExceptionArgumentContainsPipe {
            get {
                return ResourceManager.GetString("ExceptionArgumentContainsPipe", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Either {0} or {1} (or both) should be true. 的本地化字符串。
        /// </summary>
        internal static string ExceptionArgumentExpectEitherBothTrue2 {
            get {
                return ResourceManager.GetString("ExceptionArgumentExpectEitherBothTrue2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Either {0} or {1} (or both) should be set to its default value (null or default(T)). 的本地化字符串。
        /// </summary>
        internal static string ExceptionArgumentExpectEitherDefault2 {
            get {
                return ResourceManager.GetString("ExceptionArgumentExpectEitherDefault2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Either {0} or {1} should be null. They cannot both, nor neither be null. 的本地化字符串。
        /// </summary>
        internal static string ExceptionArgumentExpectEitherNull2 {
            get {
                return ResourceManager.GetString("ExceptionArgumentExpectEitherNull2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Value is empty. 的本地化字符串。
        /// </summary>
        internal static string ExceptionArgumentIsEmpty {
            get {
                return ResourceManager.GetString("ExceptionArgumentIsEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 {0} is empty. 的本地化字符串。
        /// </summary>
        internal static string ExceptionArgumentIsEmpty1 {
            get {
                return ResourceManager.GetString("ExceptionArgumentIsEmpty1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Value cannot be null or empty. 的本地化字符串。
        /// </summary>
        internal static string ExceptionArgumentNullOrEmpty {
            get {
                return ResourceManager.GetString("ExceptionArgumentNullOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 {0} is empty or whitespace. 的本地化字符串。
        /// </summary>
        internal static string ExceptionArgumentNullOrWhitespace1 {
            get {
                return ResourceManager.GetString("ExceptionArgumentNullOrWhitespace1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The asynchronous initialization of {0} has been cancelled. 的本地化字符串。
        /// </summary>
        internal static string ExceptionAsyncInitCancelled1 {
            get {
                return ResourceManager.GetString("ExceptionAsyncInitCancelled1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The asynchronous initialization of {0} throws an Exception: {1} 的本地化字符串。
        /// </summary>
        internal static string ExceptionAsyncInitFaulted2 {
            get {
                return ResourceManager.GetString("ExceptionAsyncInitFaulted2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The asynchronous initialization of {0} has not completed. 的本地化字符串。
        /// </summary>
        internal static string ExceptionAsyncInitNotComplete1 {
            get {
                return ResourceManager.GetString("ExceptionAsyncInitNotComplete1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Cannot login/logout concurrently. 的本地化字符串。
        /// </summary>
        internal static string ExceptionCannotLoginLogoutConcurrently {
            get {
                return ResourceManager.GetString("ExceptionCannotLoginLogoutConcurrently", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Cannot upload the file before all the chunks has been stashed. 的本地化字符串。
        /// </summary>
        internal static string ExceptionCannotUploadBeforeStash {
            get {
                return ResourceManager.GetString("ExceptionCannotUploadBeforeStash", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Cannot upload empty stream. 的本地化字符串。
        /// </summary>
        internal static string ExceptionCannotUploadEmptyStream {
            get {
                return ResourceManager.GetString("ExceptionCannotUploadEmptyStream", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The change has already been patrolled. 的本地化字符串。
        /// </summary>
        internal static string ExceptionChangePatrolled {
            get {
                return ResourceManager.GetString("ExceptionChangePatrolled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Collection is read-only. 的本地化字符串。
        /// </summary>
        internal static string ExceptionCollectionReadOnly {
            get {
                return ResourceManager.GetString("ExceptionCollectionReadOnly", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Cannot concurrently upload two chunks. 的本地化字符串。
        /// </summary>
        internal static string ExceptionConcurrentStashing {
            get {
                return ResourceManager.GetString("ExceptionConcurrentStashing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 When devChannel is None, devVersion must be 0. 的本地化字符串。
        /// </summary>
        internal static string ExceptionDevVersionRequiresDevChannel {
            get {
                return ResourceManager.GetString("ExceptionDevVersionRequiresDevChannel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Stream content starts with &apos;&lt;&apos;. This usually indicates the server response is HTML rather than JSON. See https://github.com/CXuesong/WikiClientLibrary/wiki/Troubleshooting for more information. 的本地化字符串。
        /// </summary>
        internal static string ExceptionHtmlResponseHint {
            get {
                return ResourceManager.GetString("ExceptionHtmlResponseHint", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Invalid enum value. 的本地化字符串。
        /// </summary>
        internal static string ExceptionInvalidEnumValue {
            get {
                return ResourceManager.GetString("ExceptionInvalidEnumValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Invalid MediaWiki API endpoint url. 的本地化字符串。
        /// </summary>
        internal static string ExceptionInvalidMediaWikiApiEndpointUrl {
            get {
                return ResourceManager.GetString("ExceptionInvalidMediaWikiApiEndpointUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The specified JSON object does not contain MediaWiki page information. 的本地化字符串。
        /// </summary>
        internal static string ExceptionInvalidPageJson {
            get {
                return ResourceManager.GetString("ExceptionInvalidPageJson", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 values cannot contain pipe character &apos;|&apos; and Unit Separator &apos;\u001F&apos; at the same time. 的本地化字符串。
        /// </summary>
        internal static string ExceptionJoinValuesCannotContainPipeAndSeparator {
            get {
                return ResourceManager.GetString("ExceptionJoinValuesCannotContainPipeAndSeparator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Cannot parse JSON from an empty stream. 的本地化字符串。
        /// </summary>
        internal static string ExceptionJsonEmptyInput {
            get {
                return ResourceManager.GetString("ExceptionJsonEmptyInput", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Error while parsing JSON out of the stream content.  的本地化字符串。
        /// </summary>
        internal static string ExceptionJsonParsingFailed {
            get {
                return ResourceManager.GetString("ExceptionJsonParsingFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The login using the main account password (rather than a bot password) cannot proceed because user interaction is required. The clientlogin action should be used instead. 的本地化字符串。
        /// </summary>
        internal static string ExceptionLoginAborted {
            get {
                return ResourceManager.GetString("ExceptionLoginAborted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Cannot recover the field state (e.g. Stream position). 的本地化字符串。
        /// </summary>
        internal static string ExceptionMediaWikiFormCannotRecoverState {
            get {
                return ResourceManager.GetString("ExceptionMediaWikiFormCannotRecoverState", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Patrolling is disabled on this wiki. 的本地化字符串。
        /// </summary>
        internal static string ExceptionPatrolDisabled {
            get {
                return ResourceManager.GetString("ExceptionPatrolDisabled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Current version of site does not support patrol by RevisionId. 的本地化字符串。
        /// </summary>
        internal static string ExceptionPatrolledByRevisionNotSupported {
            get {
                return ResourceManager.GetString("ExceptionPatrolledByRevisionNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 You don&apos;t have permission to patrol your own changes. Only users with the autopatrol right can do this. 的本地化字符串。
        /// </summary>
        internal static string ExceptionPatrolNoAutoPatrol {
            get {
                return ResourceManager.GetString("ExceptionPatrolNoAutoPatrol", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 There is no change with rcid {0}. 的本地化字符串。
        /// </summary>
        internal static string ExceptionPatrolNoSuchRcid1 {
            get {
                return ResourceManager.GetString("ExceptionPatrolNoSuchRcid1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The requested operation has failed. 的本地化字符串。
        /// </summary>
        internal static string ExceptionRequestFailed {
            get {
                return ResourceManager.GetString("ExceptionRequestFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Operation requires {0} to be {1}. 的本地化字符串。
        /// </summary>
        internal static string ExceptionRequireParamValue2 {
            get {
                return ResourceManager.GetString("ExceptionRequireParamValue2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Scribunto console execution returned empty expression. 的本地化字符串。
        /// </summary>
        internal static string ExceptionScribuntoConsoleReturnEmpty {
            get {
                return ResourceManager.GetString("ExceptionScribuntoConsoleReturnEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Cannot validate Scribunto console properly. Lua _VERSION value is empty. 的本地化字符串。
        /// </summary>
        internal static string ExceptionScribuntoResetCannotValidate {
            get {
                return ResourceManager.GetString("ExceptionScribuntoResetCannotValidate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The content has been uploaded. 的本地化字符串。
        /// </summary>
        internal static string ExceptionStashingComplete {
            get {
                return ResourceManager.GetString("ExceptionStashingComplete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Expect [filekey] or [sessionkey] in upload result. Found none. 的本地化字符串。
        /// </summary>
        internal static string ExceptionStashingNoFileKey {
            get {
                return ResourceManager.GetString("ExceptionStashingNoFileKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The stream does not support seeking. 的本地化字符串。
        /// </summary>
        internal static string ExceptionStreamCannotSeek {
            get {
                return ResourceManager.GetString("ExceptionStreamCannotSeek", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Title contains illegal character: {0}. 的本地化字符串。
        /// </summary>
        internal static string ExceptionTitleIllegalCharacter1 {
            get {
                return ResourceManager.GetString("ExceptionTitleIllegalCharacter1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Title contains illegal character sequence: {0} . 的本地化字符串。
        /// </summary>
        internal static string ExceptionTitleIllegalCharacterSequence1 {
            get {
                return ResourceManager.GetString("ExceptionTitleIllegalCharacterSequence1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The title &quot;{0}&quot; does not contain page title. 的本地化字符串。
        /// </summary>
        internal static string ExceptionTitleIsEmpty1 {
            get {
                return ResourceManager.GetString("ExceptionTitleIsEmpty1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Unexpected data received. 的本地化字符串。
        /// </summary>
        internal static string ExceptionUnexpectedData {
            get {
                return ResourceManager.GetString("ExceptionUnexpectedData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Unexpected login result: {0} . 的本地化字符串。
        /// </summary>
        internal static string ExceptionUnexpectedLoginResult1 {
            get {
                return ResourceManager.GetString("ExceptionUnexpectedLoginResult1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Received unexpected parameter value {1} for {0}. 的本地化字符串。
        /// </summary>
        internal static string ExceptionUnexpectedParamValue2 {
            get {
                return ResourceManager.GetString("ExceptionUnexpectedParamValue2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Unexpected stream EOF met. 的本地化字符串。
        /// </summary>
        internal static string ExceptionUnexpectedStreamEof {
            get {
                return ResourceManager.GetString("ExceptionUnexpectedStreamEof", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Current user doesn&apos;t have the right: {0}. 的本地化字符串。
        /// </summary>
        internal static string ExceptionUserNotHaveRight1 {
            get {
                return ResourceManager.GetString("ExceptionUserNotHaveRight1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Current user is not in the group: {0}. 的本地化字符串。
        /// </summary>
        internal static string ExceptionUserNotInGroup1 {
            get {
                return ResourceManager.GetString("ExceptionUserNotInGroup1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Incomplete DevVersion expression. 的本地化字符串。
        /// </summary>
        internal static string ExceptionVersionIncompleteDevVersion {
            get {
                return ResourceManager.GetString("ExceptionVersionIncompleteDevVersion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Invalid DevVersion number. 的本地化字符串。
        /// </summary>
        internal static string ExceptionVersionInvalidDevVersion {
            get {
                return ResourceManager.GetString("ExceptionVersionInvalidDevVersion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Invalid version number component. 的本地化字符串。
        /// </summary>
        internal static string ExceptionVersionInvalidNumber {
            get {
                return ResourceManager.GetString("ExceptionVersionInvalidNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Version has too many components. 的本地化字符串。
        /// </summary>
        internal static string ExceptionVersionTooManyComponents {
            get {
                return ResourceManager.GetString("ExceptionVersionTooManyComponents", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Unrecognizable DevVersion prefix: {0}. 的本地化字符串。
        /// </summary>
        internal static string ExceptionVersionUnknownDevVersionPrefix1 {
            get {
                return ResourceManager.GetString("ExceptionVersionUnknownDevVersionPrefix1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 An error has occurred performing MediaWiki operation. 的本地化字符串。
        /// </summary>
        internal static string ExceptionWikiClientGeneral {
            get {
                return ResourceManager.GetString("ExceptionWikiClientGeneral", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Not supported when working with a HttpMessageHandler that is not a HttpClientHandler. 的本地化字符串。
        /// </summary>
        internal static string ExceptionWikiClientNonHttpClientHandler {
            get {
                return ResourceManager.GetString("ExceptionWikiClientNonHttpClientHandler", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Reached maximum count of retries. 的本地化字符串。
        /// </summary>
        internal static string ExceptionWikiClientReachedMaxRetries {
            get {
                return ResourceManager.GetString("ExceptionWikiClientReachedMaxRetries", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Cannot detect the JSON node containing list result. 的本地化字符串。
        /// </summary>
        internal static string ExceptionWikiListCannotFindResultRoot {
            get {
                return ResourceManager.GetString("ExceptionWikiListCannotFindResultRoot", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The page {0} does not exist. 的本地化字符串。
        /// </summary>
        internal static string ExceptionWikiPageNotExists1 {
            get {
                return ResourceManager.GetString("ExceptionWikiPageNotExists1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Cannot resolve circular redirect: {0}. 的本地化字符串。
        /// </summary>
        internal static string ExceptionWikiPageResolveCircularRedirect1 {
            get {
                return ResourceManager.GetString("ExceptionWikiPageResolveCircularRedirect1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 --- End of MediaWiki remote stack trace --- 的本地化字符串。
        /// </summary>
        internal static string MediaWikiRemoteStackTraceEnd {
            get {
                return ResourceManager.GetString("MediaWikiRemoteStackTraceEnd", resourceCulture);
            }
        }
    }
}
