﻿namespace IntegrationsCore.Domain.Entities.Extensions
{
    public static class Helpers
    {
        public static T Apply<T>(this T self, Action<T> lambda)
        {
            lambda(self);
            return self;
        }
    }
}
