using Adapter.FundaApi.Client;
using Domain.Data.SearchParameters;
using FundaApi.Client;
using FundaApi.Client.Configuration;
using Host.Console.Configuration;
using Domain.Data;
using Domain.Factories;
using Domain.Services.Interfaces;

public class Program
{
    private const string ApplicationName = "FundaApi";
    private static IEstateAgentService AgentService;

    private static int _printTopAgents = 10;

    public static async Task Main(string[] args)
    {
        InitializeEstateAgentService();
        
        var cts = new CancellationTokenSource();

        var estateObjectParameters = new EstateObjectParameters("Amsterdam", PurchaseType.koop, false);
        await GetAgentsAndWriteResultToConsole(estateObjectParameters, cts.Token);

        var estateObjectParameters2 = new EstateObjectParameters("Amsterdam", PurchaseType.koop, true);
        await GetAgentsAndWriteResultToConsole(estateObjectParameters2, cts.Token);
    }

    private static async Task GetAgentsAndWriteResultToConsole(EstateObjectParameters parameters, CancellationToken cancellationToken)
    {
        Console.WriteLine("Get EstaeAgents for parameters");
        Console.WriteLine($"{parameters.City}, {parameters.PurchaseType}, Garden {parameters.WithGarden}");

        var agentsDitionary = await AgentService.GetAgentsWithEstateObjectsCountOrderedByCount(parameters, cancellationToken);

        if (agentsDitionary.Count == 0)
        {
            Console.WriteLine("No agents fount");
            return;
        }

        Console.WriteLine($"+---------+------------------------------------------+-------+");
        Console.WriteLine($"|   Id    |                 Name                     | Count |");
        Console.WriteLine($"+---------+------------------------------------------+-------+");

        int printCounter = 0;
        foreach (var item in agentsDitionary) 
        {
            var agent = item.Key;
            Console.WriteLine($"|{agent.EstateAgentId,8:D} | {agent.EstateAgentName,40:D} | {item.Value,5:D} |");

            if (++printCounter >= _printTopAgents)
            {
                Console.WriteLine($"+---------+------------------------------------------+-------+");
                break;
            }
        }
    }

    private static void InitializeEstateAgentService()
    {
        var settings = GetSettings();

        var clientFactory = new FundaApiClientFactory(ApplicationName, settings);
        var adapterFactory = new FundaApiAdapterFactory(clientFactory);
        var domainFactory = new DomainFactory(adapterFactory);

        AgentService = domainFactory.CreateEstateAgentService();
    }

    private static FundaApiClientSettings GetSettings() 
    {
        var applicationConfiguration = new ApplicationConfiguration().CreateBuilder("settings.yaml").Build();
        var settings = applicationConfiguration.ToSettings<FundaApiClientSettings>(ApplicationName);
        return settings;
    }
}
