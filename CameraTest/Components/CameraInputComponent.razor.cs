using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CameraTest.Components
{
    public partial class CameraInputComponent
    {
        private const string jsModulePath = $"./{nameof(Components)}/{nameof(CameraInputComponent)}.razor.js";
        private ElementReference VideoElement;
        private string? errorMessage;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }
            await JsInteropService.InvokeVoidWithModuleAsync(jsModulePath, "getCameraFeed", VideoElement, DotNetObjectReference.Create(this));
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
    }
}
