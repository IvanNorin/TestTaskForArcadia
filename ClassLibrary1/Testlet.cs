using System.Collections.Generic;
using System.Linq;
using ClassLibrary1.Models;
using ClassLibrary1.Providers;

namespace ClassLibrary1
{
    public class Testlet : ITestlet
    {
        public string TestletId;
        private List<Item> Items;
        private readonly IRandomNumberProvider _provider;

        public Testlet(string testletId, List<Item> items, IRandomNumberProvider provider)
        {
            TestletId = testletId;
            Items = items;
            _provider = provider;
        }
        public IEnumerable<Item> Randomize()
        {
            var twoPretestItems = Items
                .Where(item => item.ItemType == ItemTypeEnum.Pretest)
                .OrderBy(item => _provider.Get())
                .Take(2)
                .ToList();
            var nextItems = Items
                .Except(twoPretestItems)
                .OrderBy(item => _provider.Get())
                .ToList();
            return twoPretestItems.Concat(nextItems);
        }
    }
}