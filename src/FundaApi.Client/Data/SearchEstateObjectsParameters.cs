namespace FundaApi.Client.Data
{
    internal record SearchEstateObjectsParameters(string City, string purchaseType, bool WithTuin, int Page, int PageSize)
    {

    }
}
