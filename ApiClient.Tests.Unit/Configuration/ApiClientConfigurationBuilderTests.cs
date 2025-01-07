using ApiClient.Configuration;
using JetBrains.Annotations;

namespace ApiClient.Tests.Unit.Configuration;

[TestSubject(typeof(ApiClientConfigurationBuilder))]
public class ApiClientConfigurationBuilderTests
{

    private ApiClientConfigurationBuilder _sut;
    
    public ApiClientConfigurationBuilderTests()
    {
        _sut = new ApiClientConfigurationBuilder();
    }

    [Fact]
    public void Build_WithNullConfiguration_ThrowsArgumentNullException()
    {
        var exception = Record.Exception(() => _sut.Build());
        
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void WithBaseUrl_WhenUrlIsInvalid_ThrowsArgumentException(string invalidUrl)
    {
        var exception = Record.Exception(() => _sut.WithBaseUrl(invalidUrl));
    
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact(Skip = "Waiting for other build options first")]
    public void Build_WithoutSettingBaseUrl_ThrowsArgumentException()
    {
        var exception = Record.Exception(() => _sut.Build());

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
    }
}