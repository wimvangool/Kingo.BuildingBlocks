﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YellowFlare.MessageProcessing
{
    [TestClass]
    public sealed class PerUnitOfWorkLifetimeManagerTest
    {
        #region [====== Setup and Teardown ======]

        private PerUnitOfWorkLifetimeManager _lifetimeManager;

        [TestInitialize]
        public void Setup()
        {
            _lifetimeManager = new PerUnitOfWorkLifetimeManager();
        }

        #endregion

        #region [====== Tests ======]

        [TestMethod]
        public void GetValue_ReturnsNull_IfCalledOutsideContextScope()
        {
            Assert.IsNull(_lifetimeManager.GetValue());
        }

        [TestMethod]
        public void GetValue_ReturnsNull_IfNoValueHasBeenStored()
        {
            Processor.Handle(new object(), message =>            
                Assert.IsNull(_lifetimeManager.GetValue())             
            );
        }

        [TestMethod]
        public void GetValue_ReturnsStoredValue_IfValueWasStoredInCache()
        {
            var instance = new object();

            Processor.Handle(instance, message =>
            {
                _lifetimeManager.SetValue(instance);

                Assert.AreSame(instance, _lifetimeManager.GetValue());                
            });
            Assert.IsNull(_lifetimeManager.GetValue());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveValue_Throws_IfCalledOutsideContextScope()
        {
            _lifetimeManager.SetValue(new object());
        }

        [TestMethod]
        public void RemoveValue_RemovesValueFromTheCache_IfValueWasStoredInCache()
        {
            var instance = new object();

            Processor.Handle(instance, message =>
            {
                _lifetimeManager.SetValue(instance);
                _lifetimeManager.RemoveValue();

                Assert.IsNull(_lifetimeManager.GetValue());                
            });
            Assert.IsNull(_lifetimeManager.GetValue());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetValue_Throws_IfCalledOutsideContextScope()
        {
            _lifetimeManager.SetValue(new object());
        }

        #endregion

        private static UnityTestProcessor Processor
        {
            get { return UnityTestProcessor.Instance; }
        }
    }
}