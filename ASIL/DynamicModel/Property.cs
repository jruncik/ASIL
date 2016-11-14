using System.ComponentModel;

namespace ASIL.DynamicModel
{
    public class Property : INotifyPropertyChanged
    {
        public Property(object value)
        {
            Value = value;
        }

        public object Value { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
