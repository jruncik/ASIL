using System.Diagnostics;

namespace ASIL.Core
{
    internal class UnknowLogItems : IItemsCollection
    {
        public object GetOrAddAsObject(string itemValue)
        {
            Debug.Fail("Call log Item on Unknown log item type!");
            return itemValue;
        }

        public object GetAsObject(string itemValue)
        {
            return null;
        }

        public int Count
        {
            get { return 0; }
        }
    }
}