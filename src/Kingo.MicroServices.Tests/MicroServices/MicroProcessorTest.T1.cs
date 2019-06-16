﻿using System;
using System.Collections.Generic;
using System.Linq;
using Kingo.MicroServices.Endpoints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kingo.MicroServices
{    
    public class MicroProcessorTest<TProcessor>
        where TProcessor : MicroProcessor
    {
        #region [===== InstanceCollector ======]        

        private sealed class InstanceCollector : IInstanceCollector
        {
            private readonly List<object> _instances;

            public InstanceCollector()
            {
                _instances = new List<object>();
            }

            public void Add(object instance)
            {
                _instances.Add(instance);
            }

            public void AssertInstanceCountIs(int count) =>
                Assert.AreEqual(count, _instances.Distinct().Count());
        }

        #endregion

        private readonly MicroProcessorBuilder<TProcessor> _builder;
        private readonly IServiceCollection _services;

        public MicroProcessorTest()
        {
            _builder = new MicroProcessorBuilder<TProcessor>();
            _services = new ServiceCollection();
            _services.AddSingleton<IInstanceCollector, InstanceCollector>();
        }

        public IMicroProcessorBuilder ProcessorBuilder =>
            _builder;        

        public IMicroProcessor CreateProcessor() =>
            BuildServiceProvider().GetRequiredService<IMicroProcessor>();

        public IServiceProvider BuildServiceProvider() =>
            BuildServiceCollection().BuildServiceProvider();

        public IServiceCollection BuildServiceCollection() =>
            _builder.BuildServiceCollection(_services);

        // The default service count specifies how many services are registered by default
        // in the service collection. This number can be used to verify whether or not
        // the appropriate services from a test were registered or not.
        // The default services are:
        // - IMicroProcessor + TProcessor
        // - MicroProcessorOptions
        // - IHandleAsyncMethodFactory
        // - IMicroServiceBus
        // - IInstanceCollector
        protected virtual int DefaultServiceCount =>
            6;              
    }
}
