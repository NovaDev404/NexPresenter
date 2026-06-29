using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NexPresenter.Models;
using NexPresenter.Services;

namespace NexPresenter.ViewModels;

public partial class PresentationViewModel : ViewModelBase
{
    private readonly DataStorageService _storageService;
    private readonly Presentation _presentation;
    
    [ObservableProperty]
    private string _name = string.Empty;
    
    [ObservableProperty]
    private string _description = string.Empty;
    
    [ObservableProperty]
    private DateTime _createdAt;
    
    [ObservableProperty]
    private DateTime _modifiedAt;

    [ObservableProperty]
    private bool _showSlides = true;

    [ObservableProperty]
    private bool _showDetails = false;

    [ObservableProperty]
    private ObservableCollection<Slide> _allSlides = new();

    public Guid Id => _presentation.Id;

    public event Action? GoBack;

    public PresentationViewModel(Presentation presentation)
    {
        _storageService = new DataStorageService();
        _presentation = presentation;
        
        Name = presentation.Name;
        Description = presentation.Description;
        CreatedAt = presentation.CreatedAt;
        ModifiedAt = presentation.ModifiedAt;

        // Load all slides from all sections
        LoadSlides();
    }

    partial void OnNameChanged(string value)
    {
        _presentation.Name = value;
        _storageService.UpdatePresentation(_presentation);
        ModifiedAt = _presentation.ModifiedAt;
    }

    partial void OnDescriptionChanged(string value)
    {
        _presentation.Description = value;
        _storageService.UpdatePresentation(_presentation);
        ModifiedAt = _presentation.ModifiedAt;
    }

    public void LoadSlides()
    {
        // Reload presentation from storage to get latest changes
        var updatedPresentation = _storageService.GetPresentation(_presentation.Id);
        if (updatedPresentation != null)
        {
            _presentation.Sections.Clear();
            foreach (var section in updatedPresentation.Sections)
            {
                _presentation.Sections.Add(section);
            }
        }
        
        AllSlides.Clear();
        foreach (var section in _presentation.Sections)
        {
            foreach (var slide in section.Slides)
            {
                AllSlides.Add(slide);
            }
        }
    }

    [RelayCommand]
    private void Back()
    {
        GoBack?.Invoke();
    }

    public void ToggleDetails()
    {
        ShowSlides = !ShowSlides;
        ShowDetails = !ShowDetails;
    }
}
