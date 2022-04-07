using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CameraTest.Components
{
    public partial class CameraInputComponent
    {
        private const string jsModulePath = $"./{nameof(Components)}/{nameof(CameraInputComponent)}.razor.js";
        private ElementReference VideoElement;
        private string? errorMessage;
        private bool isCameraStreaming;

        private string CssCameraWrapper => isCameraStreaming ? "camera-streaming" : "camera-unavailable";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }
            await JsInteropService.InvokeVoidWithModuleAsync(jsModulePath, "getCameraFeed", VideoElement, DotNetObjectReference.Create(this));
        }

        [JSInvokable]
        public async void OnCameraStreaming()
        {
            isCameraStreaming = true;
            await JsInteropService.InvokeVoidWithModuleAsync(jsModulePath, "initializeBouncing");
            StateHasChanged();
        }

        [JSInvokable]
        public void OnCameraStreamingError(string? msg)
        {
            isCameraStreaming = false;
            errorMessage = msg;
            StateHasChanged();
        }
    }
}
