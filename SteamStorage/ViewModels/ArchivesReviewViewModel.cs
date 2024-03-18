using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models;
using SteamStorage.ViewModels.Tools;
using SteamStorage.ViewModels.UtilityViewModels;

namespace SteamStorage.ViewModels;

public class ArchivesReviewViewModel : ViewModelBase
{
    #region Fields

    private readonly ArchivesReviewModel _archivesReviewModel;

    #endregion Fields

    #region Properties

    public int Count
    {
        get => _archivesReviewModel.Count;
    }

    public string BuySumString
    {
        get => _archivesReviewModel.BuySumString;
    }

    public string SoldSumString
    {
        get => _archivesReviewModel.SoldSumString;
    }

    public bool? IsTitleOrdering
    {
        get => _archivesReviewModel.IsTitleOrdering;
        set => _archivesReviewModel.IsTitleOrdering = value;
    }

    public bool? IsCountOrdering
    {
        get => _archivesReviewModel.IsCountOrdering;
        set => _archivesReviewModel.IsCountOrdering = value;
    }

    public bool? IsBuySumOrdering
    {
        get => _archivesReviewModel.IsBuySumOrdering;
        set => _archivesReviewModel.IsBuySumOrdering = value;
    }

    public bool? IsSoldSumOrdering
    {
        get => _archivesReviewModel.IsSoldSumOrdering;
        set => _archivesReviewModel.IsSoldSumOrdering = value;
    }

    public bool? IsChangeOrdering
    {
        get => _archivesReviewModel.IsChangeOrdering;
        set => _archivesReviewModel.IsChangeOrdering = value;
    }

    public IEnumerable<ArchiveGroupViewModel> ArchiveGroupModels
    {
        get => _archivesReviewModel.ArchiveGroupModels;
    }

    public ArchiveGroupViewModel? SelectedArchiveGroupModel
    {
        get => _archivesReviewModel.SelectedArchiveGroupModel;
        set => _archivesReviewModel.SelectedArchiveGroupModel = value;
    }

    public string? NotFoundText
    {
        get => _archivesReviewModel.NotFoundText;
    }

    public bool IsLoading
    {
        get => _archivesReviewModel.IsLoading;
    }

    #endregion Properties

    #region Commands

    public RelayCommand AttachedToVisualTreeCommand
    {
        get => _archivesReviewModel.AttachedToVisualTreeCommand;
    }

    #endregion Commands

    #region Constructor

    public ArchivesReviewViewModel(
        ArchivesReviewModel archivesReviewModel)
    {
        _archivesReviewModel = archivesReviewModel;
        archivesReviewModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
