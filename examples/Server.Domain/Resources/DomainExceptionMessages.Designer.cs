﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kingo.Samples.Chess.Resources {
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
    public class DomainExceptionMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal DomainExceptionMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Kingo.Samples.Chess.Resources.DomainExceptionMessages", typeof(DomainExceptionMessages).Assembly);
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
        ///   Looks up a localized string similar to A challenge cannot be accepted or rejected when it has already been accepted..
        /// </summary>
        public static string Challenges_AlreadyAccepted {
            get {
                return ResourceManager.GetString("Challenges_AlreadyAccepted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A challenge cannot be accepted or rejected when it has already been rejected..
        /// </summary>
        public static string Challenges_AlreadyRejected {
            get {
                return ResourceManager.GetString("Challenges_AlreadyRejected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Player &apos;{0}&apos; cannot accept the specified challenge because he is not the receiver of that challenge..
        /// </summary>
        public static string Challenges_PlayerCannotAcceptChallenge {
            get {
                return ResourceManager.GetString("Challenges_PlayerCannotAcceptChallenge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot perform the operation because the game already ended..
        /// </summary>
        public static string Game_GameAlreadyEnded {
            get {
                return ResourceManager.GetString("Game_GameAlreadyEnded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot perform the operation because the sender ({0}) is no player of the game..
        /// </summary>
        public static string Game_SenderNoPlayer {
            get {
                return ResourceManager.GetString("Game_SenderNoPlayer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Player &apos;{0}&apos; attempted to challenge himself..
        /// </summary>
        public static string Players_PlayerCannotChallengeHimself {
            get {
                return ResourceManager.GetString("Players_PlayerCannotChallengeHimself", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot register player &apos;{0}&apos; because another player with the same name has already been registered..
        /// </summary>
        public static string Players_PlayerNameAlreadyRegistered {
            get {
                return ResourceManager.GetString("Players_PlayerNameAlreadyRegistered", resourceCulture);
            }
        }
    }
}