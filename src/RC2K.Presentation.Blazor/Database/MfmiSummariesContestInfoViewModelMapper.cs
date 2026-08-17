using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DomainModel;
using RC2K.Presentation.Blazor.ViewModels;
using RC2K.Utils;

namespace RC2K.Presentation.Blazor.Database;

public class MfmiSummariesContestInfoViewModelMapper : IModelMapper<MfmiSummariesContestInfoViewModel, MfmiSummariesModel>
{
    public MfmiSummariesModel ToCosmosModel(MfmiSummariesContestInfoViewModel viewModel)
    {
        MfmiSummariesModel cosmosModel = new();
        foreach (var rally in viewModel.Rallies)
        {
            MfmiSummaryRallyModel rallyCosmosModel = new()
            {
                RallyCode = rally.RallyCode is null ? -1 : (int)rally.RallyCode
            };

            foreach (var entry in rally.Entries)
            {
                MfmiRankingEntryModel entryCosmosModel = new()
                {
                    CarId = entry.CarId,
                    DriverFriendlyName = entry.DriverFriendlyName,
                    DriverNationality = entry.DriverNationality,
                    Nr = entry.Nr,
                    Group = entry.Group,
                    Points = entry.Points,
                    Rank = entry.Rank,
                    TimeCentiseconds = Utils.Utils.TimeSpanToCentiseconds(entry.Time),
                    TimeWcbCentiseconds = Utils.Utils.TimeSpanToCentiseconds(entry.TimeWcb)
                };
                rallyCosmosModel.Entries.Add(entryCosmosModel);
            }
            cosmosModel.Rallies.Add(rallyCosmosModel);
        }
        return cosmosModel;
    }

    public MfmiSummariesContestInfoViewModel ToDomainModel(MfmiSummariesModel cosmosModel)
    {
        MfmiSummariesContestInfoViewModel viewModel = new();
        foreach (var rally in cosmosModel.Rallies)
        {
            RallyContestInfoViewModel rallyViewModel = new();
            RallyCode? rc = rally.RallyCode == -1 ? null : (RallyCode)rally.RallyCode;
            if (rc is null)
            {
                rallyViewModel.RallyName = "TODO";
                rallyViewModel.RallyImage = "TODO";
                rallyViewModel.RallyCode = rc;
                rallyViewModel.IsFinalSummary = true;
            }
            else
            {
                rallyViewModel.RallyName = LevelHelper.RallyCodeToRallyName((RallyCode)rally.RallyCode);
                rallyViewModel.RallyImage = LevelHelper.RallyCodeToRallyImageName((RallyCode)rally.RallyCode);
                rallyViewModel.RallyCode = rc;
                rallyViewModel.IsFinalSummary = false;
            }
            foreach (var entry in rally.Entries)
            {
                EntryListItemViewModel entryViewModel = new()
                {
                    Group = entry.Group,
                    Nr = entry.Nr,
                    DriverFriendlyName = entry.DriverFriendlyName,
                };

                entryViewModel.Rank = entry.Rank;
                entryViewModel.DriverNationality = entry.DriverNationality;
                entryViewModel.CarId = entry.CarId;
                entryViewModel.Time = Utils.Utils.CentisecondsToTimeSpan(entry.TimeCentiseconds);
                entryViewModel.TimeWcb = Utils.Utils.CentisecondsToTimeSpan(entry.TimeWcbCentiseconds);
                entryViewModel.Points = entry.Points;

                rallyViewModel.Entries.Add(entryViewModel);
            }
            viewModel.Rallies.Add(rallyViewModel);
        }
        return viewModel;
    }
}