using Microsoft.Azure.Cosmos;
using RC2K.DataAccess.Dynamic.Repositories;
using RC2K.DataAccess.Interfaces;
using RC2K.Presentation.Blazor.ViewModels;
using Db = Microsoft.Azure.Cosmos.Database;

namespace RC2K.Presentation.Blazor.Database;

public class MfmiSummariesRepository(Db database, MfmiSummariesContestInfoViewModelMapper mapper, IEnvironmentProvider envProvider)
    : CosmosRepository<MfmiSummariesContestInfoViewModel, MfmiSummariesModel, MfmiSummariesContestInfoViewModelMapper>(database, mapper, envProvider)
{
    public override string EntityName => "Statistics";

    public async Task<MfmiSummariesContestInfoViewModel?> Get(string edition, int day)
    {
        var query = new QueryDefinition(@"
            SELECT * FROM c WHERE c.type = 'mfmi-summary' AND c.edition = @edition AND c.day = @day")
            .WithParameter("@edition", edition)
            .WithParameter("@day", day);

        var lst = await FetchAll(query, CancellationToken.None);
        return lst.FirstOrDefault();
    }

    public async Task Set(MfmiSummariesContestInfoViewModel? obj, string edition, int day)
    {
        var cosmosModel = Mapper.ToCosmosModel(obj);
        cosmosModel.Edition = edition;
        cosmosModel.Day = day;

        cosmosModel.Id = await GetIdIfExistsOrCreateNew(edition, day);

        await Container.UpsertItemAsync(cosmosModel);
    }

    private async Task<Guid> GetIdIfExistsOrCreateNew(string edition, int day)
    {
        var query = new QueryDefinition(@"
            SELECT VALUE c.id 
            FROM c 
            WHERE c.type = 'mfmi-summary' AND c.edition = @edition AND c.day = @day")
           .WithParameter("@edition", edition)
           .WithParameter("@day", day);

        Guid? guid = null;
        using var it = Container.GetItemQueryIterator<Guid>(
            query,
            requestOptions: new QueryRequestOptions { MaxItemCount = 1 });

        while (it.HasMoreResults)
        {
            var res = await it.ReadNextAsync();
            if (res.FirstOrDefault() is Guid foundGuid && foundGuid != Guid.Empty)
            {
                guid = foundGuid;
                break;
            }
        }

        guid ??= Guid.NewGuid();

        return guid.Value;
    }
}
