﻿using System;
using System.Collections.Generic;
using YellowFlare.MessageProcessing.Aggregates;

namespace YellowFlare.MessageProcessing.SampleApplication.Infrastructure
{
    internal sealed class ShoppingCartRepository : IShoppingCartRepository, IUnitOfWork
    {        
        private readonly Dictionary<Guid, ShoppingCart> _carts;
        private int _flushCount;

        public ShoppingCartRepository()
        {            
            _carts = new Dictionary<Guid, ShoppingCart>(4);
        }

        public int FlushCount
        {
            get { return _flushCount; }
        }

        public string FlushGroup
        {
            get { return null; }
        }

        public bool CanBeFlushedAsynchronously
        {
            get { return false; }
        }

        bool IUnitOfWork.RequiresFlush()
        {
            return true;
        }

        void IUnitOfWork.Flush()
        {            
            _flushCount++;
        }        

        public void Add(ShoppingCart cart)
        {            
            _carts.Add((cart as IAggregate<Guid>).Key, cart);           

            UnitOfWorkContext.Enlist(this);
        }        

        public ShoppingCart GetById(Guid id)
        {            
            var cart = _carts[id];

            UnitOfWorkContext.Enlist(this);

            return cart;
        }        
    }
}
