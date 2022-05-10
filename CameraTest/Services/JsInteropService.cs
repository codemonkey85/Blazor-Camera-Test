namespace CameraTest.Services;

public class JsInteropService : IJsInteropService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly IDictionary<string, Lazy<Task<IJSObjectReference>>> modules = new Dictionary<string, Lazy<Task<IJSObjectReference>>>();

    #region Constructor / Destructor

    public JsInteropService(IJSRuntime jsRuntime) => _jsRuntime = jsRuntime;

    public async ValueTask DisposeAsync()
    {
        foreach (var kvp in modules)
        {
            if (kvp.Value.IsValueCreated)
            {
                var module = await GetModule(kvp.Key);
                await module.DisposeAsync();
            }
        }
    }

    #endregion

    #region Private methods

    private bool ModuleInitialized(string jsPath) => modules.ContainsKey(jsPath);

    private void InitializeModule(string jsPath)
    {
        Lazy<Task<IJSObjectReference>> moduleTask = new();
        try
        {
            moduleTask = new(() => _jsRuntime.InvokeAsync<IJSObjectReference>("import", jsPath).AsTask());
            modules[jsPath] = moduleTask;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private async Task<IJSObjectReference> GetModule(string jsPath)
    {
        if (!ModuleInitialized(jsPath))
        {
            InitializeModule(jsPath);
        }
        return await modules[jsPath].Value;
    }

    public async Task<T> InvokeAsync<T>(string method, params object[] args) => await _jsRuntime.InvokeAsync<T>(method, args);

    public async Task InvokeVoidAsync(string method, params object[] args) => await _jsRuntime.InvokeVoidAsync(method, args);

    public async Task<T> InvokeWithModuleAsync<T>(string jsPath, string method, params object[] args)
    {
        var module = await GetModule(jsPath);
        return await module.InvokeAsync<T>(method, args);
    }

    public async Task InvokeVoidWithModuleAsync(string jsPath, string method, params object[] args)
    {
        var module = await GetModule(jsPath);
        await module.InvokeVoidAsync(method, args);
    }

    #endregion
}
