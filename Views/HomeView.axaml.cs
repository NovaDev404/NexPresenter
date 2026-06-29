using Avalonia.Controls;
using Avalonia.Interactivity;
using NexPresenter.Models;
using NexPresenter.ViewModels;

namespace NexPresenter.Views;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
    }

    private void OnOpenClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is Presentation presentation)
        {
            if (DataContext is HomeViewModel viewModel)
            {
                viewModel.SelectPresentationCommand.Execute(presentation);
            }
        }
    }

    private void OnDeleteClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is Guid presentationId)
        {
            if (DataContext is HomeViewModel viewModel)
            {
                viewModel.DeletePresentationCommand.Execute(presentationId);
            }
        }
    }
}
