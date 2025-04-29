using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace DbTest;

public class TestHostedService : BackgroundService
{
    private readonly ILogger<TestHostedService> _logger;
    private readonly IConfiguration _configuration;

    public TestHostedService(IConfiguration configuration, ILogger<TestHostedService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Start background");
        await Task.Delay(10_000);
        string secretName = _configuration.GetSection("RdsSecret").Get<string>()!;
        string region = "eu-central-1";

        IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

        GetSecretValueRequest request = new GetSecretValueRequest
        {
            SecretId = secretName,
            VersionStage = "AWSCURRENT"
        };

        GetSecretValueResponse response;

        try
        {
            _logger.LogInformation("Send request");
            response = await client.GetSecretValueAsync(request, stoppingToken);
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            throw e;
        }

        string secret = response.SecretString;
        
        _logger.LogInformation("Success");
        
        _logger.LogInformation(secret);
    }
}