using System;
using System.Collections.ObjectModel;

namespace NexPresenter.Models;

public class Presentation
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public string ThumbnailPath { get; set; } = string.Empty;
    public ObservableCollection<Section> Sections { get; set; } = new();

    // Ensure Sections collection is initialized on deserialization
    public Presentation()
    {
        Sections = new ObservableCollection<Section>();
    }
}
