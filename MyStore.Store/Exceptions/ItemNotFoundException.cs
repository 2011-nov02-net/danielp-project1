﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Store.Exceptions
{
    /// <summary>
    /// An Item is not registered with the Catalogue
    /// </summary>
    public class ItemNotFoundException : ArgumentException
    {
        public ItemNotFoundException()
        {
        }

        public ItemNotFoundException(string message) : base(message)
        {
        }
    }
}
