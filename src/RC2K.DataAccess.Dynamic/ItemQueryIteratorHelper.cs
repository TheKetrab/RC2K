using Microsoft.Azure.Cosmos;

namespace RC2K.DataAccess.Dynamic;

public class ItemQueryIteratorHelper
{
    public async Task<(List<U>, double)> FetchAll<T,U>(QueryDefinition query, FeedIterator<T> it, Func<T,U> map, CancellationToken ct = default)
    {
        double totalRu = 0;
        List<U> result = new();
        while (it.HasMoreResults)
        {
            var response = await it.ReadNextAsync(ct);
            totalRu += response.RequestCharge;

            var val = response.Resource;
            result.AddRange(val.Select(map));
        }

        return (result, totalRu);
    }
}
