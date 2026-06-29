using Avalonia.Controls;
using Avalonia.Interactivity;
using NexPresenter.Models;
using NexPresenter.ViewModels;

namespace NexPresenter.Views;

public partial class SlideEditorView : UserControl
{
    public SlideEditorView()
    {
        InitializeComponent();
    }

    private void OnDeleteSectionClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is Section section && DataContext is SlideEditorViewModel viewModel)
        {
            viewModel.DeleteSectionCommand.Execute(section);
        }
    }

    private void OnDeleteSlideClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is Slide slide && DataContext is SlideEditorViewModel viewModel)
        {
            // Find the parent section by walking up the visual tree
            var parent = button.Parent;
            while (parent != null)
            {
                if (parent is Border border && border.DataContext is Section section)
                {
                    viewModel.DeleteSlideCommand.Execute((section, slide));
                    break;
                }
                parent = (parent as Control)?.Parent;
            }
        }
    }

    private void OnAddSlideClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is Section section && DataContext is SlideEditorViewModel viewModel)
        {
            viewModel.AddSlideCommand.Execute(section);
        }
    }
}
