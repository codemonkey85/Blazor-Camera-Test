using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CameraTest.Components
{
    public partial class CameraInputComponent
    {
        private const string jsModulePath = $"./{nameof(Components)}/{nameof(CameraInputComponent)}.razor.js";
        private ElementReference VideoElement;
        private string? errorMessage;

        private DotNetObjectReference<CameraInputComponent> objRef;

        protected override async Task OnInitializedAsync()
        {
            objRef = DotNetObjectReference.Create(this);
            await JsInteropService.InvokeVoidWithModuleAsync(jsModulePath, "getCameraFeed", VideoElement, objRef);
        }

        [JSInvokable]
        public async Task OnCameraStreaming()
        {
            Console.WriteLine("Hello from dotnet");
        }

        [JSInvokable]
        public async Task OnCameraStreamingError(string? msg)
        {
            errorMessage = msg;
        }

        public void Dispose()
        {
            objRef?.Dispose();
        }
    }
}
