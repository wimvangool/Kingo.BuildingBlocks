﻿using System;
using Kingo.Messaging.Domain;

namespace Kingo.Messaging.SampleApplication
{
    internal sealed class ProductAddedToCartEvent : Message, IVersionedObject<Guid, int>
    {
        public Guid ShoppingCartId;
        public int ShoppingCartVersion;
        public int ProductId;
        public int OldQuantity;
        public int NewQuantity;

        Guid IKeyedObject<Guid>.Key
        {
            get { return ShoppingCartId; }
        }

        int IVersionedObject<Guid, int>.Version
        {
            get { return ShoppingCartVersion; }
        }        

        public override Message Copy()
        {
            return new ProductAddedToCartEvent
            {
                ShoppingCartId = ShoppingCartId,
                ShoppingCartVersion = ShoppingCartVersion,
                ProductId = ProductId,
                OldQuantity = OldQuantity,
                NewQuantity = NewQuantity
            };
        }
    }
}
