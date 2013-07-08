﻿using System;

namespace YellowFlare.MessageProcessing.SampleApplication
{
    internal interface IShoppingCartRepository
    {
        void Add(ShoppingCart cart);

        ShoppingCart GetById(Guid id);
    }
}