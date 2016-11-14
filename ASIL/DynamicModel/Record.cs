using System.Collections.ObjectModel;

namespace ASIL.DynamicModel
{
    public class Record
    {
        private readonly ObservableCollection<object> properties = new ObservableCollection<object>();

        public Record(object[] properties)
        {
            foreach (object property in properties)
            {
                Properties.Add(property);
            }
        }

        public void AddProperty(object property)
        {
            Properties.Add(property);
        }

        public ObservableCollection<object> Properties
        {
            get { return properties; }
        }
    }
}
