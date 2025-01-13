namespace ApiClient.Configuration.Headers;

/// <summary>
/// Static Factory class to produce type safe values of common content-types,
/// Public constructor to allow for custom types and provide type safety when building the config.
/// </summary>
public record ContentType
{
    private const string JSON_MIME_TYPE = "application/json";
    private const string XML_MIME_TYPE = "application/xml";
    private const string JAVASCRIPT_MIME_TYPE = "text/javascript";

    public string Value { get; private init; }

    public ContentType(string contentType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(contentType);

        Value = contentType;
    }

    public static ContentType Json() => new ContentType(JSON_MIME_TYPE);

    public static ContentType JavaScript() => new ContentType(JAVASCRIPT_MIME_TYPE);

    public static ContentType Xml() => new ContentType(XML_MIME_TYPE);
}
