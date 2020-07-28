﻿using Cybtans.Entities;
using System;

namespace Cybtans.Test.Domain
{
    [GenerateMessage]
    public class OrderItem: Entity<int>
    {
        public string ProductName { get; set; }

        public float Price { get; set; }

        public float Discount { get; set; }

        public Guid OrderId { get; set; }
    }
}
