using System;
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
    }

    [RelayCommand]
    private void Save()
    {
        _presentation.Name = Name;
        _presentation.Description = Description;
        _storageService.UpdatePresentation(_presentation);
        ModifiedAt = _presentation.ModifiedAt;
    }

    [RelayCommand]
    private void Back()
    {
        GoBack?.Invoke();
    }
}
