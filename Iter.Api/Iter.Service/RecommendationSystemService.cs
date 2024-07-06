using AutoMapper;
using Iter.Core;
using Iter.Core.EntityModels;
using Iter.Repository.Interface;
using Iter.Services.Interface;
using Microsoft.ML;
using Microsoft.ML.Data;
using static Iter.Services.RecommendationSystemService;
using static Microsoft.ML.Trainers.MatrixFactorizationTrainer;

namespace Iter.Services
{
    public class RecommendationSystemService : IRecommendationSystemService
    {
        static MLContext mlContext;
        static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        static ITransformer model = null;
        private readonly IReservationRepository reservationRepository;
        private readonly IArrangementRepository arrangementRepository;
        private readonly IUserAuthenticationService userAuthenticationService;
        private readonly IMapper mapper;

        private const double THRESHOLD = 0.5;

        public RecommendationSystemService(IReservationRepository reservationRepository, IArrangementRepository arrangementRepository, IMapper mapper, IUserAuthenticationService userAuthenticationService)
        {
            this.reservationRepository = reservationRepository;
            this.arrangementRepository = arrangementRepository;
            this.mapper = mapper;
            this.userAuthenticationService = userAuthenticationService;
        }

        public async Task<List<ArrangementSearchResponse>> RecommendArrangement(Guid arrangementId)
        {
            try
            {
                await semaphore.WaitAsync();

                InitializeMlContext();

                var arrangement = await arrangementRepository.GetById(arrangementId);
                List<int> destinationCityIds = arrangement.Destinations.Select(x => x.CityId).ToList();
                var reservations = await reservationRepository.GetArrangementsByDestinationCityNames(destinationCityIds);

                var data = GenerateTrainingData(reservations);

                var trainData = mlContext.Data.LoadFromEnumerable(data);

                var options = GetMatrixFactorizationTrainerOptions(nameof(ServiceEntry.ServiceID), nameof(ServiceEntry.CoPurchaseServiceID));
                var estimator = mlContext.Recommendation().Trainers.MatrixFactorization(options);

                model = estimator.Fit(trainData);
                var predictedDestinations = PredictDestinations(destinationCityIds, await arrangementRepository.GetAllDestinations());

                var cities = predictedDestinations.OrderByDescending(x => x.Item2).Select(x => x.Item1).ToList();
                var arrangements = await GetArrangementsByDestinationNames(cities);

                return arrangements;
            }
            catch (Exception ex)
            {
                mlContext = null;
                Console.WriteLine($"Exception: {ex.Message}");
                return new List<ArrangementSearchResponse>();
            }
            finally
            {
                semaphore.Release();
            }
        }

        private void InitializeMlContext()
        {
            if (mlContext == null)
            {
                mlContext = new MLContext();
            }
        }

        private List<ServiceEntry> GenerateTrainingData(IEnumerable<Reservation> reservations)
        {
            var data = new List<ServiceEntry>();

            foreach (var reservation in reservations)
            {
                foreach (var destination in reservation.Arrangement.Destinations)
                {
                    foreach (var reservation2 in reservations.Where(x => x.Id != reservation.Id))
                    {
                        foreach (var otherDestination in reservation2.Arrangement.Destinations.Where(d => d.CityId != destination.CityId))
                        {
                            data.Add(new ServiceEntry
                            {
                                ServiceID = (uint)destination.CityId,
                                CoPurchaseServiceID = (uint)otherDestination.CityId,
                                Label = 1
                            });
                        }
                    }
                }
            }

            Console.WriteLine($"Total training data entries: {data.Count}");
            return data;
        }

        private List<Tuple<int, float>> PredictDestinations(List<int> destinationCityIds, List<Destination> allDestinations)
        {
            var predictedDestinations = new List<Tuple<int, float>>();

            foreach (var destination in destinationCityIds)
            {
                var predictionEngine = mlContext.Model.CreatePredictionEngine<ServiceEntry, CopurchasePrediction>(model);
                foreach (var otherDestination in allDestinations.Select(x => x.CityId).Where(d => !destinationCityIds.Contains(d)))
                {
                    var prediction = predictionEngine.Predict(new ServiceEntry { ServiceID = (uint)destination, CoPurchaseServiceID = (uint)otherDestination });

                    if (predictedDestinations.Any(x => x.Item1 == otherDestination))
                    {
                        continue;
                    }

                    if (prediction.Score > THRESHOLD)
                    {
                        predictedDestinations.Add(new Tuple<int, float>(otherDestination, prediction.Score));
                    }
                }
            }

            return predictedDestinations;
        }

        private async Task<List<ArrangementSearchResponse>> GetArrangementsByDestinationNames(List<int> cities)
        {
            try
            {
                var currentUser = await this.userAuthenticationService.GetCurrentUserAsync();

                var data = await arrangementRepository.GetRecommendedArrangementsByDestinationNames(cities, currentUser.ClientId);

                return this.mapper.Map<List<ArrangementSearchResponse>>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching arrangements: {ex.Message}");
                throw;
            }
        }

        private Options GetMatrixFactorizationTrainerOptions(
            string matrixColumnIndexColumnName,
            string matrixRowIndexColumnName,
            string labelColumnName = "Label",
            LossFunctionType lossFunctionType = LossFunctionType.SquareLossOneClass,
            double alpha = 0.01,
            double lambda = 0.025,
            int numberOfIterations = 100,
            double c = 0.00001)
        {
            return new Options
            {
                MatrixColumnIndexColumnName = matrixColumnIndexColumnName,
                MatrixRowIndexColumnName = matrixRowIndexColumnName,
                LabelColumnName = labelColumnName,
                LossFunction = lossFunctionType,
                Alpha = alpha,
                Lambda = lambda,
                NumberOfIterations = numberOfIterations,
                C = c
            };
        }

        public class CopurchasePrediction
        {
            public float Score { get; set; }
        }

        public class ServiceEntry
        {
            [KeyType(count: 1000)]
            public uint ServiceID { get; set; }

            [KeyType(count: 1000)]
            public uint CoPurchaseServiceID { get; set; }

            public float Label { get; set; }
        }

    }
}
