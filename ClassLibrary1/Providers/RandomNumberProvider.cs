using System;

namespace ClassLibrary1.Providers
{
    class RandomNumberProvider : IRandomNumberProvider
    {
        public int Get()
        {
            return new Random().Next(0, 100);
        }
    }
}