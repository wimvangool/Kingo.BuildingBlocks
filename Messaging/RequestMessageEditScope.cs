﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Messaging.Resources;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace System.ComponentModel.Messaging
{
    public sealed class RequestMessageEditScope : ITransactionalScope
    {
        private readonly RequestMessageEditScope _parentScope;
        private readonly IRequestMessage _message;
        private readonly IRequestMessage _messageBackup;        
        private readonly bool _suppressValidation;
        private readonly object _state;

        private readonly HashSet<string> _changedProperties;
        private bool _messageValidationWasFired;
        private bool _hasCompleted;
        private bool _isDisposed;        

        private RequestMessageEditScope(IRequestMessage message, IRequestMessage messageBackup, bool suppressValidation, object state)
        {
            _message = message;
            _message.PropertyChanged += HandleMessagePropertyChanged;
            _messageBackup = messageBackup;
            
            _changedProperties = new HashSet<string>();
            _suppressValidation = suppressValidation;
            _state = state;
        }

        private RequestMessageEditScope(RequestMessageEditScope parentScope, IRequestMessage messageBackup, bool suppressValidation, object state)
        {
            _parentScope = parentScope;
            _message = parentScope._message;
            _message.PropertyChanged += HandleMessagePropertyChanged;
            _messageBackup = messageBackup;

            _changedProperties = new HashSet<string>();
            _suppressValidation = suppressValidation;
            _state = state;
        }

        private void HandleMessagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var propertyName = e.PropertyName;
            if (propertyName == null)
            {
                return;
            }
            _changedProperties.Add(propertyName);            
        }

        private RequestMessageEditScope CreateNestedScope(bool suppressValidation, object state)
        {
            return new RequestMessageEditScope(this, _message.Copy(true), suppressValidation, state); ;
        }

        private RequestMessageEditScope ParentScope
        {
            get { return _parentScope; }
        }

        private IRequestMessage Message
        {
            get { return _message; }
        }

        public IRequestMessage MessageBackup
        {
            get { return _messageBackup; }
        }        

        public object State
        {
            get { return _state; }
        }

        private bool SuppressesValidation()
        {
            _messageValidationWasFired = true;

            return _suppressValidation | (_parentScope != null && _parentScope.SuppressesValidation());
        }

        public void Complete()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(typeof(RequestMessageEditScope).Name);
            }
            if (_hasCompleted)
            {
                throw NewScopeAlreadyCompletedException();
            }
            if (IsNotCurrentScope())
            {
                throw NewCannotCompleteScopeException();
            }            
            if (_messageValidationWasFired && _suppressValidation && (_parentScope == null || !_parentScope.SuppressesValidation()))
            {
                _message.Validate();
            }
            _changedProperties.Clear();
            _hasCompleted = true;
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }            
            if (IsNotCurrentScope())
            {
                throw NewIncorrectNestingOfScopesException();
            }
            _message.PropertyChanged -= HandleMessagePropertyChanged;
            _isDisposed = true;

            if (_hasCompleted)
            {
                EndEdit(_message);
            }
            else
            {
                CancelEdit(_message);
            }
        }

        private bool IsNotCurrentScope()
        {
            RequestMessageEditScope editScope;

            if (MessagesInEditMode.TryGetValue(Message, out editScope))
            {
                return !ReferenceEquals(this, editScope);
            }
            return true;
        }

        private void RestoreBackup()
        {
            foreach (var property in PropertiesToRestore())
            {
                RestorePropertyValue(property);
            }
            _changedProperties.Clear();
        }

        private IEnumerable<PropertyInfo> PropertiesToRestore()
        {
            if (_changedProperties.Count == 0)
            {
                return Enumerable.Empty<PropertyInfo>();
            }
            return from property in _message.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                   where _changedProperties.Contains(property.Name)
                   select property;
        }

        private void RestorePropertyValue(PropertyInfo property)
        {
            var propertySetter = property.GetSetMethod(true);
            if (propertySetter != null)
            {
                propertySetter.Invoke(_message, new [] { property.GetValue(_messageBackup, null) });
            }            
        }

        #region [====== Exception Factory Methods ======]

        private static Exception NewScopeAlreadyCompletedException()
        {
            return new InvalidOperationException(ExceptionMessages.TransactionScope_ScopeAlreadyCompleted);
        }

        private static Exception NewCannotCompleteScopeException()
        {
            return new InvalidOperationException(ExceptionMessages.TransactionScope_CannotCompleteScope);
        }

        private static Exception NewIncorrectNestingOfScopesException()
        {
            return new InvalidOperationException(ExceptionMessages.Scope_IncorrectNesting);
        }

        #endregion

        #region [====== BeginEdit, CancelEdit & EndEdit ======]

        private static readonly ThreadLocal<Dictionary<IRequestMessage, RequestMessageEditScope>> _MessagesInEditMode;

        static RequestMessageEditScope()
        {
            _MessagesInEditMode = new ThreadLocal<Dictionary<IRequestMessage, RequestMessageEditScope>>(CreateScopeDictionary);
        }   
     
        private static Dictionary<IRequestMessage, RequestMessageEditScope> CreateScopeDictionary()
        {
            return new Dictionary<IRequestMessage, RequestMessageEditScope>();
        }

        private static Dictionary<IRequestMessage, RequestMessageEditScope> MessagesInEditMode
        {
            get { return _MessagesInEditMode.Value; }
        }

        internal static bool IsInEditMode(IRequestMessage message)
        {
            return MessagesInEditMode.ContainsKey(message);
        }

        internal static object GetEditScopeState(IRequestMessage message)
        {
            RequestMessageEditScope editScope;

            if (MessagesInEditMode.TryGetValue(message, out editScope))
            {
                return editScope.State;
            }
            return null;
        }

        internal static bool IsValidationSuppressed(IRequestMessage message)
        {
            RequestMessageEditScope editScope;

            if (MessagesInEditMode.TryGetValue(message, out editScope))
            {
                return editScope.SuppressesValidation();
            }
            return false;
        }

        internal static RequestMessageEditScope BeginEdit(IRequestMessage message)
        {
            return BeginEdit(message, false, null, false);
        }        

        internal static RequestMessageEditScope BeginEdit(IRequestMessage message, bool suppressValidation, object state, bool createNewScope)
        {
            RequestMessageEditScope editScope;

            if (MessagesInEditMode.TryGetValue(message, out editScope))
            {
                if (createNewScope)
                {
                    MessagesInEditMode[message] = editScope = editScope.CreateNestedScope(suppressValidation, state);
                }
            }
            else
            {
                MessagesInEditMode.Add(message, editScope = new RequestMessageEditScope(message, message.Copy(true), suppressValidation, state));
            }
            return editScope;
        }

        internal static void CancelEdit(IRequestMessage message)
        {
            RequestMessageEditScope editScope;

            if (MessagesInEditMode.TryGetValue(message, out editScope))
            {
                editScope.RestoreBackup();
                editScope.Dispose();

                EndScope(editScope);
            }
        }

        internal static void EndEdit(IRequestMessage message)
        {
            RequestMessageEditScope editScope;

            if (MessagesInEditMode.TryGetValue(message, out editScope))
            {                
                editScope.Dispose();

                EndScope(editScope);
            }
        }

        private static void EndScope(RequestMessageEditScope editScope)
        {
            if (editScope.ParentScope == null)
            {
                MessagesInEditMode.Remove(editScope.Message);
            }
            else
            {
                MessagesInEditMode[editScope.Message] = editScope.ParentScope;
            }
        }

        #endregion
    }
}
