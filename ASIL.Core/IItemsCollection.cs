namespace ASIL.Core
{
    internal interface IItemsCollection
    {
        object GetOrAddAsObject(string itemValue);
        object GetAsObject(string itemValue);

        int Count { get; }
    }
}
