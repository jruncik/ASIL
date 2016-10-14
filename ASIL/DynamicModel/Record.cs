using System.Collections.ObjectModel;

namespace ASIL.DynamicModel
{
    public class Record
    {
        private readonly ObservableCollection<Property> properties = new ObservableCollection<Property>();

        public Record(params Property[] properties)
        {
            foreach (var property in properties)
                Properties.Add(property);
        }

        public ObservableCollection<Property> Properties
        {
            get { return properties; }
        }
    }
}
