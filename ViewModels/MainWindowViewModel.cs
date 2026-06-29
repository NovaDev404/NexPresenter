using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NexPresenter.Models;

namespace NexPresenter.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private HomeViewModel? _homeViewModel;
    private PresentationViewModel? _presentationViewModel;
    private SlideEditorViewModel? _slideEditorViewModel;
    private Window? _mainWindow;

    [ObservableProperty]
    private ViewModelBase? _currentView;

    [ObservableProperty]
    private bool _showSidebar;

    public bool ShowHomeLayout => !ShowSidebar;

    public MainWindowViewModel()
    {
        NavigateToHome();
    }

    public void SetMainWindow(Window window)
    {
        _mainWindow = window;
    }

    public void NavigateToHome()
    {
        if (_homeViewModel == null)
        {
            _homeViewModel = new HomeViewModel();
            _homeViewModel.PresentationSelected += OnPresentationSelected;
        }
        CurrentView = _homeViewModel;
        ShowSidebar = false;
    }

    [RelayCommand]
    private void NavigateHome()
    {
        NavigateToHome();
    }

    private void OnPresentationSelected(Presentation presentation)
    {
        _presentationViewModel = new PresentationViewModel(presentation);
        _presentationViewModel.GoBack += OnGoBack;
        CurrentView = _presentationViewModel;
        ShowSidebar = true;
    }

    [RelayCommand]
    private void NavigatePresentation()
    {
        if (_presentationViewModel != null)
        {
            // Reload slides from storage to get latest changes
            _presentationViewModel.LoadSlides();
            CurrentView = _presentationViewModel;
        }
    }

    [RelayCommand]
    private void NavigateSlideEditor()
    {
        if (_presentationViewModel != null && _slideEditorViewModel == null)
        {
            _slideEditorViewModel = new SlideEditorViewModel(_presentationViewModel.Id);
        }
        if (_slideEditorViewModel != null)
        {
            CurrentView = _slideEditorViewModel;
        }
    }

    [RelayCommand]
    private async void AddSection()
    {
        if (_mainWindow == null) return;
        
        // Ensure we're in a presentation context
        if (_presentationViewModel == null) return;

        // Create SlideEditorViewModel if it doesn't exist
        if (_slideEditorViewModel == null)
        {
            _slideEditorViewModel = new SlideEditorViewModel(_presentationViewModel.Id);
        }

        // Navigate to Slide Editor view
        CurrentView = _slideEditorViewModel;
        
        var dialog = new Views.SectionTypeDialog
        {
            DataContext = new SectionTypeDialogViewModel()
        };

        var viewModel = (SectionTypeDialogViewModel)dialog.DataContext;
        
        viewModel.SectionSelected += (sectionType) =>
        {
            dialog.Close(sectionType);
        };

        var result = await dialog.ShowDialog<SectionType?>(_mainWindow);
        
        if (result.HasValue && _slideEditorViewModel != null)
        {
            _slideEditorViewModel.AddSectionCommand.Execute(result.Value);
        }
    }

    [RelayCommand]
    private void Settings()
    {
        // If in presentation view, toggle details
        if (_presentationViewModel != null)
        {
            // Navigate to presentation view first if not already there
            if (CurrentView != _presentationViewModel)
            {
                NavigatePresentationCommand.Execute(null);
            }
            _presentationViewModel.ToggleDetails();
        }
    }

    private void OnGoBack()
    {
        if (_presentationViewModel != null)
        {
            _presentationViewModel.GoBack -= OnGoBack;
            _presentationViewModel = null;
        }
        _slideEditorViewModel = null;
        NavigateToHome();
    }
}
