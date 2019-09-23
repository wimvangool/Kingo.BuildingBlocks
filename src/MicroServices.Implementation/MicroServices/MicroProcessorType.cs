﻿using System;

namespace Kingo.MicroServices
{
    internal sealed class MicroProcessorType : MicroProcessorComponent
    {
        private MicroProcessorType(MicroProcessorComponent component, params Type[] serviceTypes) :
            base(component, serviceTypes) { }        

        public static bool IsMicroProcessorType(Type type, out MicroProcessorType processor)
        {
            if (IsMicroProcessorComponent(type, out var component))
            {
                return IsMicroProcessorType(component, out processor);
            }
            processor = null;
            return false;
        }

        private static bool IsMicroProcessorType(MicroProcessorComponent component, out MicroProcessorType processor)
        {
            if (typeof(MicroProcessor).IsAssignableFrom(component.Type))
            {
                processor = new MicroProcessorType(component, typeof(IMicroProcessor));
                return true;
            }
            processor = null;
            return false;
        }
    }
}
