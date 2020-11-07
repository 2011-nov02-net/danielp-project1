﻿namespace MyStore.Store
{
    public class Customer
    {
        //order history
        OrderHistory orderHistory;

        public struct Name
        {
            string First;
            char? MiddleInitial;
            string Last;
        };

        readonly Name CustomerName;

        //NOTE: optional req
        #nullable enable
        Location? DefaultStore;
    }
}