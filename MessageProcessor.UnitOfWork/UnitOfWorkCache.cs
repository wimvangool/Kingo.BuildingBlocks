﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YellowFlare.MessageProcessing
{
    internal sealed class UnitOfWorkCache : CacheRelay
    {
        protected override bool TryGetCache(out ICache cache)
        {
            var context = UnitOfWorkContext.Current;
            if (context == null)
            {
                cache = null;
                return false;
            }
            cache = context.InternalCache;
            return true;
        }
    }
}
