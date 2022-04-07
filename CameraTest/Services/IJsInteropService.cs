namespace CameraTest.Services
{
    public interface IJsInteropService
    {
        ValueTask DisposeAsync();
        bool ModuleInitialized(string jsPath);
        void InitializeModule(string jsPath);
        Task<T> InvokeAsync<T>(string method, params object[] args);
        Task InvokeVoidAsync(string method, params object[] args);
        Task<T> InvokeWithModuleAsync<T>(string jsPath, string method, params object[] args);
        Task InvokeVoidWithModuleAsync(string jsPath, string method, params object[] args);
    }
}
