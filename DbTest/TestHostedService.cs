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
            response = await client.GetSecretValueAsync(request, stoppingToken);
        }
        catch (Exception e)
        {
            throw e;
        }

        string secret = response.SecretString;
        
        _logger.LogInformation(secret);
    }
}