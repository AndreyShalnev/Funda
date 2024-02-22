using Domain.Data;
using Domain.Data.SearchParameters;
using Domain.Factories;
using Domain.Factories.Interfaces;
using Domain.Repositories;
using Domain.Services.Interfaces;
using Moq;

namespace Domain.Tests
{
    public class EstateAgentServiceTests
    {
        private IEstateAgentService Sut;
        private Mock<IEstateObjectRepositoryFactory> EstateObjectRepositoryFactoryMock;
        private Mock<IEstateObjectRepository> EstateObjectRepositoryMock;

        private const string _city = "Amsterdam";
        private const PurchaseType _purchaseType = PurchaseType.koop;

        private CancellationToken ct;

        [SetUp]
        public void Setup()
        {
            InitializeEstateObjectRepositoryMock();
            InitializeEstateObjectRepositoryFactoryMock();
            
            Sut = GetEstateAgentService();
        }

        [Test]
        public async Task ShouldReturnEmptyList_WhenRepositoryHasNoData()
        {
            var parameters = GetParameters();

            var resuult = await Sut.GetAgentsWithEstateObjectsCountOrderedByCount(parameters, ct);

            CollectionAssert.IsEmpty(resuult);
        }

        [Test]
        public async Task ShouldReturnEstateAgentsInCorrectOrder_WhenRepositoryReturnsDifferentAgents()
        {
            SetupEstateObjectRepositoryByEstateAgentIds([3, 2, 1, 2, 1, 1]);
            var parameters = GetParameters();

            var resuult = await Sut.GetAgentsWithEstateObjectsCountOrderedByCount(parameters, ct);

            var expectedCollection = new Dictionary<EstateAgent, int>()
            {
                { GenerateEstateAgent(1), 3 },
                { GenerateEstateAgent(2), 2 },
                { GenerateEstateAgent(3), 1 },
            };
            CollectionAssert.AreEqual(expectedCollection, resuult);
        }

        [Test]
        public async Task ShouldReturnCountEstateObject_WhenRepositoryReturnsEstateObjectWithSameAgent()
        {
            SetupEstateObjectRepositoryByEstateAgentIds([1, 1, 1, 1]);
            var parameters = GetParameters();

            var resuult = await Sut.GetAgentsWithEstateObjectsCountOrderedByCount(parameters, ct);

            var expectedCollection = new Dictionary<EstateAgent, int>()
            {
                { GenerateEstateAgent(1), 4 },
            };
            CollectionAssert.AreEqual(expectedCollection, resuult);
        }

        private EstateObjectParameters GetParameters(bool withGarden = false)
        {
            return new EstateObjectParameters(_city, _purchaseType, withGarden);
        }

        private void SetupEstateObjectRepositoryByEstateAgentIds(IEnumerable<int> ids)
        {
            var estateObjects = GenerateEstateObjectsWithIds(ids);
            SetupEstateObjectRepository(estateObjects);
        }

        private void SetupEstateObjectRepository(IEnumerable<EstateObject> estateObjects)
        {
            EstateObjectRepositoryMock
                .Setup(i => i.Get(It.IsAny<EstateObjectParameters>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(estateObjects));
        }

        private IEnumerable<EstateObject> GenerateEstateObjectsWithIds(IEnumerable<int> ids)
        {
            return ids.Select(i => GenerateEstateObject(i)).ToList();
        }

        private EstateObject GenerateEstateObject(int id)
        {
            return new EstateObject(id, $"a{id}");
        }
        private EstateAgent GenerateEstateAgent(int id)
        {
            return new EstateAgent(id, $"a{id}");
        }

        private void InitializeEstateObjectRepositoryMock()
        {
            EstateObjectRepositoryMock = new Mock<IEstateObjectRepository>();
        }

        private void InitializeEstateObjectRepositoryFactoryMock()
        {
            EstateObjectRepositoryFactoryMock = new Mock<IEstateObjectRepositoryFactory>();
            EstateObjectRepositoryFactoryMock
                .Setup(i => i.CreateEstateObjectRepository())
                .Returns(EstateObjectRepositoryMock.Object);
        }

        private IEstateAgentService GetEstateAgentService()
        {
            var factory = new DomainFactory(EstateObjectRepositoryFactoryMock.Object);
            return factory.CreateEstateAgentService();
        }
    }
}