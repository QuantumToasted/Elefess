namespace Elefess.Test;

[Flags]
public enum RequestFlags
{
    None = 0,
    MissingAuthorizationHeader = 1 << 1,
    InvalidAcceptHeader = 1 << 2,
    InvalidContentTypeHeader = 1 << 3
}