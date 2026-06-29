using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NexPresenter.Models;

public class Slide : INotifyPropertyChanged
{
    private string _content = string.Empty;
    
    public Guid Id { get; set; }
    
    public string Content
    {
        get => _content;
        set
        {
            if (_content != value)
            {
                _content = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
