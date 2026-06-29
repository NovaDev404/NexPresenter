using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NexPresenter.Models;

namespace NexPresenter.ViewModels;

public partial class SectionTypeDialogViewModel : ViewModelBase
{
    public event Action<SectionType?>? SectionSelected;

    [RelayCommand]
    private void SelectSong()
    {
        SectionSelected?.Invoke(SectionType.Song);
    }

    [RelayCommand]
    private void Cancel()
    {
        SectionSelected?.Invoke(null);
    }
}
