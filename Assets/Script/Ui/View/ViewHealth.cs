using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class ViewHealth : MonoBehaviour, INotifyPropertyChanged
{
    private string _health;

    public event PropertyChangedEventHandler PropertyChanged;

    [Binding]
    public string Health
    {
        get { return "MaxHP : " + _health; }
        set
        {
            if (_health == value) { return; }
            _health = value;
            OnPropertyChanged("Health");
        }
    }

    private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
