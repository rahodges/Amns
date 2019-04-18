﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Amns.GreyFox.Localization {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SecurityStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SecurityStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Amns.GreyFox.Localization.SecurityStrings", typeof(SecurityStrings).Assembly);
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
        ///   Looks up a localized string similar to Disabled.
        /// </summary>
        internal static string AccountDisabled {
            get {
                return ResourceManager.GetString("AccountDisabled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Last Access.
        /// </summary>
        internal static string LastAccess {
            get {
                return ResourceManager.GetString("LastAccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Login Count.
        /// </summary>
        internal static string LoginCount {
            get {
                return ResourceManager.GetString("LoginCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password.
        /// </summary>
        internal static string Password {
            get {
                return ResourceManager.GetString("Password", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{6,15})$.
        /// </summary>
        internal static string PasswordRegEx {
            get {
                return ResourceManager.GetString("PasswordRegEx", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Leave password empty to keep user&apos;s currently encrypted password. All passwords are encrypted using strong encryption. Passwords must be 6 to 15 characters long..
        /// </summary>
        internal static string PasswordTextBoxNote {
            get {
                return ResourceManager.GetString("PasswordTextBoxNote", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Describe the role&apos;s use and duties here. Be sure to annotate security features..
        /// </summary>
        internal static string Role_DescriptionTooltip {
            get {
                return ResourceManager.GetString("Role_DescriptionTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Disables the role globally. Any features that require the role will also be disabled..
        /// </summary>
        internal static string Role_DisabledTooltip {
            get {
                return ResourceManager.GetString("Role_DisabledTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use a role name that is easily identifiable. Use backslashes to represent inheritance..
        /// </summary>
        internal static string Role_NameTooltip {
            get {
                return ResourceManager.GetString("Role_NameTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Roles.
        /// </summary>
        internal static string Roles {
            get {
                return ResourceManager.GetString("Roles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Separate roles with carriage return..
        /// </summary>
        internal static string RolesTextBoxNote {
            get {
                return ResourceManager.GetString("RolesTextBoxNote", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Security.
        /// </summary>
        internal static string SecurityTab {
            get {
                return ResourceManager.GetString("SecurityTab", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User.
        /// </summary>
        internal static string User {
            get {
                return ResourceManager.GetString("User", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Username.
        /// </summary>
        internal static string Username {
            get {
                return ResourceManager.GetString("Username", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{6,15})$.
        /// </summary>
        internal static string UsernameRegEx {
            get {
                return ResourceManager.GetString("UsernameRegEx", resourceCulture);
            }
        }
    }
}
