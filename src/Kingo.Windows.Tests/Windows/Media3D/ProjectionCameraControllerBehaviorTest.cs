﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Kingo.Windows.Media3D
{
    [TestClass]
    public sealed class ProjectionCameraControllerBehaviorTest
    {
        private ProjectionCameraControllerBehavior _behavior;

        [TestInitialize]
        public void Setup()
        {
            _behavior = new ProjectionCameraControllerBehavior();            
        }

        #region [====== Initial State =====]

        [TestMethod]
        public void InputSource_IsNull_IfBehaviorIsJustCreated()
        {            
            Assert.IsNull(_behavior.InputSource);
        }

        [TestMethod]
        public void Controller_IsNotNull_IfBehaviorIsJustCreated()
        {
            Assert.IsNotNull(_behavior.Controller);
        }

        [TestMethod]
        public void ControlModes_IsEmpty_IfBehaviorIsJustCreated()
        {            
            Assert.IsNotNull(_behavior.ControlModes);
            Assert.AreEqual(0, _behavior.ControlModes.Count);
        }

        #endregion

        #region [====== Attach ======]  

        [TestMethod]
        public void Attach_DoesNotActivateControlMode_IfControllerIsNull()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();
            
            controlMode.InputBindings.Add(inputBinding);

            _behavior.Controller = null;
            _behavior.ControlModes.Add(controlMode);
            _behavior.Attach(new PerspectiveCamera());

            inputBinding.AssertActivateBindingCallCountIs(0);
        }

        [TestMethod]
        public void Attach_DoesNotActivateControlMode_IfKeysDontMatch()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();

            controlMode.Key = new object();
            controlMode.InputBindings.Add(inputBinding);

            _behavior.ControlModes.Add(controlMode);
            _behavior.Attach(new PerspectiveCamera());

            inputBinding.AssertActivateBindingCallCountIs(0);
        }

        [TestMethod]
        public void Attach_ActivatesControlMode_IfControllerIsNotNull_And_NullKeysMatch()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();
            
            controlMode.InputBindings.Add(inputBinding);

            _behavior.ControlModes.Add(controlMode);
            _behavior.Attach(new PerspectiveCamera());

            inputBinding.AssertActivateBindingCallCountIs(1);            
        }

        [TestMethod]
        public void Attach_ActivatesControlMode_IfControllerIsNotNull_And_NonNullKeysMatch()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();
            var key = Guid.NewGuid();

            controlMode.Key = key;
            controlMode.InputBindings.Add(inputBinding);

            _behavior.ActiveModeKey = key;
            _behavior.ControlModes.Add(controlMode);
            _behavior.Attach(new PerspectiveCamera());

            inputBinding.AssertActivateBindingCallCountIs(1);
        }

        #endregion

        #region [====== Detach ======]

        [TestMethod]
        public void Detach_DoesNotDeactivateControlMode_IfControlModeWasNotActive()
        {
            // ControlMode is not activated because keys don't match.
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();

            controlMode.Key = new object();
            controlMode.InputBindings.Add(inputBinding);

            _behavior.ControlModes.Add(controlMode);
            _behavior.Attach(new PerspectiveCamera());
            _behavior.Detach();

            inputBinding.AssertDeactivateBindingCallCountIs(0);
        }

        [TestMethod]
        public void Detach_DeactivatesControlMode_IfControlModeWasActive()
        {            
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();
           
            controlMode.InputBindings.Add(inputBinding);

            _behavior.ControlModes.Add(controlMode);
            _behavior.Attach(new PerspectiveCamera());
            _behavior.Detach();

            inputBinding.AssertDeactivateBindingCallCountIs(1);
        }

        #endregion

        #region [====== InputSource Changes ======]

        [TestMethod]
        public void InputSourceChange_HasNoEffect_IfBehaviorIsInDetachedState()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();

            controlMode.Key = new object();
            controlMode.InputBindings.Add(inputBinding);

            _behavior.ControlModes.Add(controlMode);
            _behavior.InputSource = CreateInputSource();

            inputBinding.AssertActivateBindingCallCountIs(0);
            inputBinding.AssertDeactivateBindingCallCountIs(0);
        }

        [TestMethod]
        public void InputSourceChange_HasNoEffect_IfBehaviorIsInPassiveState()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();

            controlMode.Key = new object();
            controlMode.InputBindings.Add(inputBinding);

            _behavior.ControlModes.Add(controlMode);
            _behavior.Attach(new PerspectiveCamera());
            _behavior.InputSource = CreateInputSource();

            inputBinding.AssertActivateBindingCallCountIs(0);
            inputBinding.AssertDeactivateBindingCallCountIs(0);
        }

        [TestMethod]
        public void InputSourceChange_ReactivatesActiveControlMode_IfBehaviorIsInActiveState()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();
            
            controlMode.InputBindings.Add(inputBinding);

            _behavior.ControlModes.Add(controlMode);
            _behavior.Attach(new PerspectiveCamera());
            _behavior.InputSource = CreateInputSource();

            inputBinding.AssertActivateBindingCallCountIs(2);
            inputBinding.AssertDeactivateBindingCallCountIs(1);
        }

        #endregion

        #region [====== Controller Changes ======]

        [TestMethod]
        public void ControllerChange_HasNoEffect_IfBehaviorIsInDetachedState()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();

            controlMode.Key = new object();
            controlMode.InputBindings.Add(inputBinding);

            _behavior.ControlModes.Add(controlMode);
            _behavior.Controller = CreateController();

            inputBinding.AssertActivateBindingCallCountIs(0);
            inputBinding.AssertDeactivateBindingCallCountIs(0);
        }

        [TestMethod]
        public void ControllerChange_HasNoEffect_IfBehaviorIsInPassiveState()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();

            controlMode.Key = new object();
            controlMode.InputBindings.Add(inputBinding);

            _behavior.ControlModes.Add(controlMode);
            _behavior.Attach(new PerspectiveCamera());
            _behavior.Controller = CreateController();

            inputBinding.AssertActivateBindingCallCountIs(0);
            inputBinding.AssertDeactivateBindingCallCountIs(0);
        }

        [TestMethod]
        public void ControllerChange_DeactivatesActiveControlMode_IfBehaviorIsInActiveState_And_NewControllerIsNull()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();

            controlMode.InputBindings.Add(inputBinding);

            _behavior.ControlModes.Add(controlMode);
            _behavior.Attach(new PerspectiveCamera());
            _behavior.Controller = null;

            inputBinding.AssertActivateBindingCallCountIs(1);
            inputBinding.AssertDeactivateBindingCallCountIs(1);
        }

        [TestMethod]
        public void ControllerChange_ReactivatesActiveControlMode_IfBehaviorIsInActiveState_And_NewControllerIsNotNull()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();

            controlMode.InputBindings.Add(inputBinding);

            _behavior.ControlModes.Add(controlMode);
            _behavior.Attach(new PerspectiveCamera());
            _behavior.Controller = CreateController();

            inputBinding.AssertActivateBindingCallCountIs(2);
            inputBinding.AssertDeactivateBindingCallCountIs(1);
        }

        #endregion

        #region [====== ActiveModeKey Changes ======]

        [TestMethod]
        public void ActiveModeKeyChange_HasNoEffect_IfBehaviorIsInDetachedState()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();
            
            controlMode.InputBindings.Add(inputBinding);

            _behavior.ControlModes.Add(controlMode);
            _behavior.ActiveModeKey = new object();

            inputBinding.AssertActivateBindingCallCountIs(0);
            inputBinding.AssertDeactivateBindingCallCountIs(0);
        }

        [TestMethod]
        public void ActiveModeKeyChange_HasNoEffect_IfBehaviorIsInPassiveState_And_NewKeyDoesNotMatchAnyControlModeKey()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();

            controlMode.InputBindings.Add(inputBinding);
            controlMode.Key = new object();

            _behavior.ControlModes.Add(controlMode);
            _behavior.Attach(new PerspectiveCamera());
            _behavior.ActiveModeKey = new object();

            inputBinding.AssertActivateBindingCallCountIs(0);
            inputBinding.AssertDeactivateBindingCallCountIs(0);
        }

        [TestMethod]
        public void ActiveModeKeyChange_MovesBehaviorIntoActiveState_IfBehaviorIsInPassiveState_And_NewKeyMatchesAnyControlModeKey()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();
            var controlModeKey = new object();

            controlMode.InputBindings.Add(inputBinding);
            controlMode.Key = controlModeKey;

            _behavior.ControlModes.Add(controlMode);
            _behavior.Attach(new PerspectiveCamera());
            _behavior.ActiveModeKey = controlModeKey;

            inputBinding.AssertActivateBindingCallCountIs(1);
            inputBinding.AssertDeactivateBindingCallCountIs(0);
        }

        [TestMethod]
        public void ActiveModeKeyChange_MovesBehaviorIntoPassiveState_IfBehaviorIsInActiveState_And_NewKeyDoesNotMatchAnyControlModeKey()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();            

            controlMode.InputBindings.Add(inputBinding);            

            _behavior.ControlModes.Add(controlMode);
            _behavior.Attach(new PerspectiveCamera());
            _behavior.ActiveModeKey = new object();

            inputBinding.AssertActivateBindingCallCountIs(1);
            inputBinding.AssertDeactivateBindingCallCountIs(1);
        }

        [TestMethod]
        public void ActiveModeKeyChange_MovesBehaviorIntoPassiveAndThenBackIntoActiveState_IfBehaviorIsInActiveState_And_NewKeyMatchesOtherControlModeKey()
        {
            var inputBindingA = new ControlModeInputBindingSpy();
            var controlModeA = new ControlMode();

            controlModeA.InputBindings.Add(inputBindingA);

            var inputBindingB = new ControlModeInputBindingSpy();
            var controlModeB = new ControlMode();

            controlModeB.Key = new object();
            controlModeB.InputBindings.Add(inputBindingB);

            _behavior.ControlModes.Add(controlModeA);
            _behavior.ControlModes.Add(controlModeB);
            _behavior.Attach(new PerspectiveCamera());

            inputBindingA.AssertActivateBindingCallCountIs(1);
            inputBindingA.AssertDeactivateBindingCallCountIs(0);

            inputBindingB.AssertActivateBindingCallCountIs(0);
            inputBindingB.AssertDeactivateBindingCallCountIs(0);

            _behavior.ActiveModeKey = controlModeB.Key;

            inputBindingA.AssertActivateBindingCallCountIs(1);
            inputBindingA.AssertDeactivateBindingCallCountIs(1);

            inputBindingB.AssertActivateBindingCallCountIs(1);
            inputBindingB.AssertDeactivateBindingCallCountIs(0);
        }

        #endregion

        #region [====== ControlMode.Key Changes ======]

        [TestMethod]
        public void ControlModeKeyChange_HasNoEffect_IfBehaviorIsInDetachedState()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();

            controlMode.InputBindings.Add(inputBinding);

            _behavior.ControlModes.Add(controlMode);
            
            controlMode.Key = new object();            

            inputBinding.AssertActivateBindingCallCountIs(0);
            inputBinding.AssertDeactivateBindingCallCountIs(0);
        }

        [TestMethod]
        public void ControlModeKeyChange_HasNoEffect_IfBehaviorIsInPassiveState_And_NewKeyDoesNotMatchActiveModeKey()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();

            controlMode.InputBindings.Add(inputBinding);

            _behavior.ControlModes.Add(controlMode);
            _behavior.ActiveModeKey = new object();
            _behavior.Attach(new PerspectiveCamera());            

            controlMode.Key = new object();

            inputBinding.AssertActivateBindingCallCountIs(0);
            inputBinding.AssertDeactivateBindingCallCountIs(0);
        }

        [TestMethod]
        public void ControlModeKeyChange_MovesBehaviorIntoActiveState_IfBehaviorIsInPassiveState_And_NewKeyMatchesActiveModeKey()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();

            controlMode.InputBindings.Add(inputBinding);

            _behavior.ControlModes.Add(controlMode);
            _behavior.ActiveModeKey = new object();
            _behavior.Attach(new PerspectiveCamera());

            controlMode.Key = _behavior.ActiveModeKey;

            inputBinding.AssertActivateBindingCallCountIs(1);
            inputBinding.AssertDeactivateBindingCallCountIs(0);
        }

        [TestMethod]
        public void ControlModeKeyChange_HasNoEffect_IfBehaviorIsInActiveState_And_KeyOfNonActiveModeChanged()
        {
            var inputBindingA = new ControlModeInputBindingSpy();
            var controlModeA = new ControlMode();

            controlModeA.InputBindings.Add(inputBindingA);

            var inputBindingB = new ControlModeInputBindingSpy();
            var controlModeB = new ControlMode();

            controlModeB.Key = new object();
            controlModeB.InputBindings.Add(inputBindingB);

            _behavior.ControlModes.Add(controlModeA);
            _behavior.ControlModes.Add(controlModeB);
            _behavior.Attach(new PerspectiveCamera());

            controlModeB.Key = new object();

            inputBindingA.AssertActivateBindingCallCountIs(1);
            inputBindingA.AssertDeactivateBindingCallCountIs(0);

            inputBindingB.AssertActivateBindingCallCountIs(0);
            inputBindingB.AssertDeactivateBindingCallCountIs(0);
        }

        [TestMethod]
        public void ControlModeKeyChange_MovesBehaviorIntoPassiveState_IfBehaviorIsInActiveState_And_KeyOfActiveModeChanged()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();

            controlMode.InputBindings.Add(inputBinding);

            _behavior.ControlModes.Add(controlMode);            
            _behavior.Attach(new PerspectiveCamera());

            controlMode.Key = new object();

            inputBinding.AssertActivateBindingCallCountIs(1);
            inputBinding.AssertDeactivateBindingCallCountIs(1);
        }

        #endregion

        #region [====== ControlModes.Add ======]

        [TestMethod]
        public void ControlModesAdd_HasNoEffect_IfBehaviorIsInDetachedState()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();

            controlMode.InputBindings.Add(inputBinding);

            _behavior.ControlModes.Add(controlMode);
            _behavior.ControlModes.Add(new ControlMode());            

            inputBinding.AssertActivateBindingCallCountIs(0);
            inputBinding.AssertDeactivateBindingCallCountIs(0);
        }        

        [TestMethod]
        public void ControlModesAdd_HasNoEffect_IfBehaviorIsInPassiveState_And_KeyOfAddedModeDoesNotMatchActiveModeKey()
        {            
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();

            controlMode.Key = new object();
            controlMode.InputBindings.Add(inputBinding);

            _behavior.Attach(new PerspectiveCamera());
            _behavior.ControlModes.Add(controlMode);            

            inputBinding.AssertActivateBindingCallCountIs(0);
            inputBinding.AssertDeactivateBindingCallCountIs(0);
        }

        [TestMethod]
        public void ControlModesAdd_MovesBehaviorIntoActiveMode_IfBehaviorIsInPassiveState_And_KeyOfAddedModeMatchesActiveModeKey()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();
            
            controlMode.InputBindings.Add(inputBinding);

            _behavior.Attach(new PerspectiveCamera());
            _behavior.ControlModes.Add(controlMode);

            inputBinding.AssertActivateBindingCallCountIs(1);
            inputBinding.AssertDeactivateBindingCallCountIs(0);
        }

        [TestMethod]
        public void ControlModesAdd_HasNoEffect_IfBehaviorIsInActiveState()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();

            controlMode.InputBindings.Add(inputBinding);

            _behavior.ControlModes.Add(controlMode);
            _behavior.Attach(new PerspectiveCamera());
            _behavior.ControlModes.Add(new ControlMode());

            inputBinding.AssertActivateBindingCallCountIs(1);
            inputBinding.AssertDeactivateBindingCallCountIs(0);
        }

        #endregion

        #region [====== ControlModes.Remove ======]

        [TestMethod]
        public void ControlModesRemove_HasNoEffect_IfBehaviorIsInDetachedState()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();

            controlMode.InputBindings.Add(inputBinding);

            _behavior.ControlModes.Add(controlMode);
            _behavior.ControlModes.Remove(controlMode);

            inputBinding.AssertActivateBindingCallCountIs(0);
            inputBinding.AssertDeactivateBindingCallCountIs(0);
        }

        [TestMethod]
        public void ControlModesRemove_HasNoEffect_IfBehaviorIsInPassiveState()
        {
            var inputBinding = new ControlModeInputBindingSpy();
            var controlMode = new ControlMode();

            controlMode.Key = new object();
            controlMode.InputBindings.Add(inputBinding);

            _behavior.ControlModes.Add(controlMode);
            _behavior.Attach(new PerspectiveCamera());
            _behavior.ControlModes.Remove(controlMode);

            inputBinding.AssertActivateBindingCallCountIs(0);
            inputBinding.AssertDeactivateBindingCallCountIs(0);
        }

        [TestMethod]
        public void ControlModesRemove_HasNoEffect_IfBehaviorIsInActiveState_And_RemovedControlModeWasNotActive()
        {
            var inputBindingA = new ControlModeInputBindingSpy();
            var controlModeA = new ControlMode();
            
            controlModeA.InputBindings.Add(inputBindingA);

            var inputBindingB = new ControlModeInputBindingSpy();
            var controlModeB = new ControlMode();

            controlModeB.Key = new object();
            controlModeB.InputBindings.Add(inputBindingB);            

            _behavior.ControlModes.Add(controlModeA);
            _behavior.ControlModes.Add(controlModeB);
            _behavior.Attach(new PerspectiveCamera());
            _behavior.ControlModes.Remove(controlModeB);

            inputBindingA.AssertActivateBindingCallCountIs(1);
            inputBindingA.AssertDeactivateBindingCallCountIs(0);

            inputBindingB.AssertActivateBindingCallCountIs(0);
            inputBindingB.AssertDeactivateBindingCallCountIs(0);
        }

        [TestMethod]
        public void ControlModesRemove_MovesBehaviorIntoPassiveState_IfBehaviorIsInActiveState_And_RemovedControlModeWasActive()
        {
            var inputBindingA = new ControlModeInputBindingSpy();
            var controlModeA = new ControlMode();

            controlModeA.InputBindings.Add(inputBindingA);

            var inputBindingB = new ControlModeInputBindingSpy();
            var controlModeB = new ControlMode();

            controlModeB.Key = new object();
            controlModeB.InputBindings.Add(inputBindingB);

            _behavior.ControlModes.Add(controlModeA);
            _behavior.ControlModes.Add(controlModeB);
            _behavior.Attach(new PerspectiveCamera());
            _behavior.ControlModes.Remove(controlModeA);

            inputBindingA.AssertActivateBindingCallCountIs(1);
            inputBindingA.AssertDeactivateBindingCallCountIs(1);

            inputBindingB.AssertActivateBindingCallCountIs(0);
            inputBindingB.AssertDeactivateBindingCallCountIs(0);
        }

        #endregion

        private static UIElement CreateInputSource()
        {
            return new Label();
        }

        private static IProjectionCameraController CreateController()
        {
            return new Mock<IProjectionCameraController>().Object;
        }
    }
}
