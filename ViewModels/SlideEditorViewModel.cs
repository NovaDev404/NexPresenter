using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NexPresenter.Models;
using NexPresenter.Services;

namespace NexPresenter.ViewModels;

public partial class SlideEditorViewModel : ViewModelBase
{
    private readonly DataStorageService _storageService;
    private readonly Presentation _presentation;

    [ObservableProperty]
    private ObservableCollection<Section> _sections = new();

    public SlideEditorViewModel(Guid presentationId)
    {
        _storageService = new DataStorageService();
        _presentation = _storageService.GetPresentations().FirstOrDefault(p => p.Id == presentationId) ?? throw new ArgumentException("Presentation not found");
        
        // Clear any existing sections
        Sections.Clear();
        
        // Load sections from presentation
        foreach (var section in _presentation.Sections)
        {
            Sections.Add(section);
            
            // Subscribe to slide content changes for auto-save
            foreach (var slide in section.Slides)
            {
                if (slide is INotifyPropertyChanged notifySlide)
                {
                    notifySlide.PropertyChanged += OnSlidePropertyChanged;
                }
            }
        }
    }

    private void OnSlidePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Slide.Content))
        {
            _storageService.UpdatePresentation(_presentation);
        }
    }

    [RelayCommand]
    private void AddSection(SectionType type)
    {
        var section = new Section
        {
            Id = Guid.NewGuid(),
            Name = type == SectionType.Song ? "New Song" : "New Section",
            Type = type
        };
        Sections.Add(section);
        _presentation.Sections.Add(section);
        _storageService.UpdatePresentation(_presentation);
    }

    [RelayCommand]
    private void AddSlide(Section section)
    {
        var slide = new Slide
        {
            Id = Guid.NewGuid(),
            Content = ""
        };
        section.Slides.Add(slide);
        
        // Subscribe to slide content changes for auto-save
        if (slide is INotifyPropertyChanged notifySlide)
        {
            notifySlide.PropertyChanged += OnSlidePropertyChanged;
        }
        
        _storageService.UpdatePresentation(_presentation);
    }

    [RelayCommand]
    private void DeleteSection(Section section)
    {
        Sections.Remove(section);
        _presentation.Sections.Remove(section);
        _storageService.UpdatePresentation(_presentation);
    }

    [RelayCommand]
    private void DeleteSlide((Section Section, Slide Slide) data)
    {
        // Unsubscribe from property changes before deleting
        if (data.Slide is INotifyPropertyChanged notifySlide)
        {
            notifySlide.PropertyChanged -= OnSlidePropertyChanged;
        }
        
        data.Section.Slides.Remove(data.Slide);
        _storageService.UpdatePresentation(_presentation);
    }
}
