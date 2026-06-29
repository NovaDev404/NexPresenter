using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace NexPresenter.Models;

public enum SectionType
{
    Slides,
    Song
}

public class Section
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public SectionType Type { get; set; }
    public ObservableCollection<Slide> Slides { get; set; } = new();

    // Ensure Slides collection is initialized on deserialization
    public Section()
    {
        Slides = new ObservableCollection<Slide>();
    }
}
