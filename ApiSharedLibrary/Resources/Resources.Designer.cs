﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApiSharedLibrary.Resources {
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
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ApiSharedLibrary.Resources.Resources", typeof(Resources).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 4567.
        /// </summary>
        public static string ErrorCode_BankingApiUnexpectedError {
            get {
                return ResourceManager.GetString("ErrorCode_BankingApiUnexpectedError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 3456.
        /// </summary>
        public static string ErrorCode_BankingApiUnsuccesfulResponse {
            get {
                return ResourceManager.GetString("ErrorCode_BankingApiUnsuccesfulResponse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 9876.
        /// </summary>
        public static string ErrorCode_InternalServerErrorCatchAll {
            get {
                return ResourceManager.GetString("ErrorCode_InternalServerErrorCatchAll", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 5432.
        /// </summary>
        public static string ErrorCode_MappingError_BankApiToPaymentApi {
            get {
                return ResourceManager.GetString("ErrorCode_MappingError_BankApiToPaymentApi", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 2345.
        /// </summary>
        public static string ErrorCode_MappingError_PaymentApiToBankApi {
            get {
                return ResourceManager.GetString("ErrorCode_MappingError_PaymentApiToBankApi", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 7890.
        /// </summary>
        public static string ErrorCode_UnauthenticatedIncorrectCredentials {
            get {
                return ResourceManager.GetString("ErrorCode_UnauthenticatedIncorrectCredentials", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 6789.
        /// </summary>
        public static string ErrorCode_UnauthenticatedInvalidAuthenticationHeader {
            get {
                return ResourceManager.GetString("ErrorCode_UnauthenticatedInvalidAuthenticationHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 5678.
        /// </summary>
        public static string ErrorCode_UnauthenticatedMissingAuthenticationHeader {
            get {
                return ResourceManager.GetString("ErrorCode_UnauthenticatedMissingAuthenticationHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1234.
        /// </summary>
        public static string ErrorCode_Validation {
            get {
                return ResourceManager.GetString("ErrorCode_Validation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please refer to the status code in the API documentation at http://paymentgateway.com/api/documentation/errors. Also see http://paymentgateway.com/api/issues to see if any know issues match your circumstances (and a timeline for their fixes)..
        /// </summary>
        public static string ErrorDescription_Generic {
            get {
                return ResourceManager.GetString("ErrorDescription_Generic", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please refer to the expected input formats in the API documentation at http://paymentgateway.com/api/documentation/errors..
        /// </summary>
        public static string ErrorDescription_Validation {
            get {
                return ResourceManager.GetString("ErrorDescription_Validation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unexpected internal server error occurred when calling the banking API..
        /// </summary>
        public static string ErrorMessage_BankingApiUnexpectedError {
            get {
                return ResourceManager.GetString("ErrorMessage_BankingApiUnexpectedError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The following internal server occurred when calling the banking API: {0} - {1}.
        /// </summary>
        public static string ErrorMessage_BankingApiUnsuccesfulResponse {
            get {
                return ResourceManager.GetString("ErrorMessage_BankingApiUnsuccesfulResponse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An enexpected error occurred whilst processing your request..
        /// </summary>
        public static string ErrorMessage_InternalServerErrorCatchAll {
            get {
                return ResourceManager.GetString("ErrorMessage_InternalServerErrorCatchAll", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An internal server error occurred when mapping from the banking API response DTO to the this API&apos;s response DTO..
        /// </summary>
        public static string ErrorMessage_MappingError_BankApiToPaymentApi {
            get {
                return ResourceManager.GetString("ErrorMessage_MappingError_BankApiToPaymentApi", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An internal server error occurred when mapping from the request DTO to the banking API request DTO..
        /// </summary>
        public static string ErrorMessage_MappingError_PaymentApiToBankApi {
            get {
                return ResourceManager.GetString("ErrorMessage_MappingError_PaymentApiToBankApi", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Authentication failed: The credentials received in the Authentication header were incorrect..
        /// </summary>
        public static string ErrorMessage_UnauthenticatedIncorrectCredentials {
            get {
                return ResourceManager.GetString("ErrorMessage_UnauthenticatedIncorrectCredentials", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Authentication failed: The request received contained an invalid Authentication header. Please try using Basic Authentication..
        /// </summary>
        public static string ErrorMessage_UnauthenticatedInvalidAuthenticationHeader {
            get {
                return ResourceManager.GetString("ErrorMessage_UnauthenticatedInvalidAuthenticationHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Authentication failed: The request received was missing an Authentication header. Please retry using Basic Authentication..
        /// </summary>
        public static string ErrorMessage_UnauthenticatedMissingAuthenticationHeader {
            get {
                return ResourceManager.GetString("ErrorMessage_UnauthenticatedMissingAuthenticationHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to One or more validation errors were found in the request..
        /// </summary>
        public static string ErrorMessage_Validation {
            get {
                return ResourceManager.GetString("ErrorMessage_Validation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The BankingService encountered the following unexpected error: {0}.
        /// </summary>
        public static string Logging_BankingServiceUnexpected {
            get {
                return ResourceManager.GetString("Logging_BankingServiceUnexpected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The DtoMapper could not map from a null {0}.
        /// </summary>
        public static string Logging_DtoMapperNullInput {
            get {
                return ResourceManager.GetString("Logging_DtoMapperNullInput", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The application encountered the following error and will response to the client accordingly: {0} .
        /// </summary>
        public static string Logging_GlobalExceptionHandler {
            get {
                return ResourceManager.GetString("Logging_GlobalExceptionHandler", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Model validation failed due to the errors with the following request properties: {0}.
        /// </summary>
        public static string Logging_ModelValidationError {
            get {
                return ResourceManager.GetString("Logging_ModelValidationError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The CVV must be 3 or 4 digits between 0 and 9..
        /// </summary>
        public static string Validation_CvvRegex {
            get {
                return ResourceManager.GetString("Validation_CvvRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The expirationMonth must be between 01 and 12, and the combination of expirationMonth and expirationYear must be a valid date and not be in the past..
        /// </summary>
        public static string Validation_ExpirationMonth {
            get {
                return ResourceManager.GetString("Validation_ExpirationMonth", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The expirationYear must not be in the past or more than 15 years in the future..
        /// </summary>
        public static string Validation_ExpirationYear {
            get {
                return ResourceManager.GetString("Validation_ExpirationYear", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The transactionId must be in the format of a GUID - a 32 digit unique ID made up of hexadecimal characters separated by hyphens like so: 8-4-4-4-12..
        /// </summary>
        public static string Validation_TransactionId {
            get {
                return ResourceManager.GetString("Validation_TransactionId", resourceCulture);
            }
        }
    }
}