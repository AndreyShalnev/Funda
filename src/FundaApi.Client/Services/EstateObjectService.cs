using FundaApi.Client.Client.Interfaces;
using FundaApi.Client.Data;
using FundaApi.Client.Data.Dtos;

namespace FundaApi.Client.Services
{
    internal class EstateObjectService : IEstateObjectService
    {
        private readonly IEstateObjectsClient Client;
        private readonly int _pageSize;
        public EstateObjectService(IEstateObjectsClient client, int pageSize) 
        {
            Client = client;
            _pageSize = pageSize;
        }

        public async Task<ActionResult<IEnumerable<EstateObjectDto>>> GetEstateAgentsObjects(string City, string purchaseType, bool WithTuin, CancellationToken cancellationToken)
        {
            var lockObject = new Object();
            var resultList = new List<EstateObjectDto>();
            var searchParameters = new SearchEstateObjectsParameters(City, purchaseType, WithTuin, 1, _pageSize);

            var firstPageResult = await Client.GetEstateObjects(searchParameters, cancellationToken);
            if (!firstPageResult.IsSuccess)
            {
                return ActionResult<IEnumerable<EstateObjectDto>>.Failed(firstPageResult.Message, firstPageResult.Exception);
            }

            resultList.AddRange(firstPageResult.Result.Objects);

            await Parallel.ForAsync(2, firstPageResult.Result.Paging.TotalPages, cancellationToken,
                async (index, ct) =>
                {
                    var searchParameters = new SearchEstateObjectsParameters(City, purchaseType, WithTuin, index, _pageSize);
                    var result = await Client.GetEstateObjects(searchParameters, cancellationToken);
                    if (result.IsSuccess)
                    {
                        lock (lockObject)
                        {
                            resultList.AddRange(result.Result.Objects);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{result.Message}");
                    }
                });

            return ActionResult<IEnumerable<EstateObjectDto>>.Success(resultList);

        }
    }
}
