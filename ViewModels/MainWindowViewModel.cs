using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NexPresenter.Models;

namespace NexPresenter.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private HomeViewModel? _homeViewModel;
    private PresentationViewModel? _presentationViewModel;

    [ObservableProperty]
    private ViewModelBase? _currentView;

    [ObservableProperty]
    private bool _showSidebar;

    public bool ShowHomeLayout => !ShowSidebar;

    public MainWindowViewModel()
    {
        NavigateToHome();
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
    private void Settings()
    {
        // If in presentation view, toggle details
        if (_presentationViewModel != null)
        {
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
        NavigateToHome();
    }
}
