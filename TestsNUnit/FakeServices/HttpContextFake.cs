using System.Text;
using Microsoft.AspNetCore.Http;

namespace TestsNUnit.FakeServices;

public static class HttpContextFake
{
    public static HttpContext GetHttpContextFake()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Session = new FakeSession();
        return httpContext;
    }
}

public class FakeSession : ISession
{
    private readonly Dictionary<string, byte[]> _sessionData = new Dictionary<string, byte[]>();

    public IEnumerable<string> Keys => _sessionData.Keys;

    public string Id { get; set; }

    public Task LoadAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task CommitAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public bool TryGetValue(string key, out byte[] value)
    {
        return _sessionData.TryGetValue(key, out value);
    }

    public void Set(string key, byte[] value)
    {
        _sessionData[key] = value;
    }

    public void Remove(string key)
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool IsAvailable { get; }

    // Implement other methods of ISession if needed for your tests
}