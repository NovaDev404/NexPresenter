using CommunityToolkit.Mvvm.ComponentModel;
using NexPresenter.Models;

namespace NexPresenter.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private HomeViewModel? _homeViewModel;
    private PresentationViewModel? _presentationViewModel;

    [ObservableProperty]
    private ViewModelBase? _currentView;

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
    }

    private void OnPresentationSelected(Presentation presentation)
    {
        _presentationViewModel = new PresentationViewModel(presentation);
        _presentationViewModel.GoBack += OnGoBack;
        CurrentView = _presentationViewModel;
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
