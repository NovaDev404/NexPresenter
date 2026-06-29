using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NexPresenter.Models;
using NexPresenter.Services;

namespace NexPresenter.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly DataStorageService _storageService;
    
    [ObservableProperty]
    private ObservableCollection<Presentation> _presentations = new();

    public HomeViewModel()
    {
        _storageService = new DataStorageService();
        LoadPresentations();
    }

    private void LoadPresentations()
    {
        var loaded = _storageService.LoadPresentations();
        Presentations.Clear();
        foreach (var presentation in loaded.OrderByDescending(p => p.ModifiedAt))
        {
            Presentations.Add(presentation);
        }
    }

    [RelayCommand]
    private void CreateNewPresentation()
    {
        var name = $"New Presentation {Presentations.Count + 1}";
        var presentation = _storageService.CreatePresentation(name);
        Presentations.Insert(0, presentation);
    }

    [RelayCommand]
    private void DeletePresentation(Guid id)
    {
        _storageService.DeletePresentation(id);
        var toRemove = Presentations.FirstOrDefault(p => p.Id == id);
        if (toRemove != null)
        {
            Presentations.Remove(toRemove);
        }
    }

    public event Action<Presentation>? PresentationSelected;

    [RelayCommand]
    private void SelectPresentation(Presentation presentation)
    {
        PresentationSelected?.Invoke(presentation);
    }
}
