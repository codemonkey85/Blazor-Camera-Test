const constraints = (window.constraints = {
    audio: false,
    video: true,
});

export async function getCameraFeed(videoElement, dotnet) {
    console.log("initializing camera");
    try {
        let stream = await navigator.mediaDevices.getUserMedia(constraints);
        handleSuccess(stream, videoElement);
        dotnet.invokeMethodAsync("OnCameraStreaming");
    } catch (error) {
        handleError(error, dotnet);
    }
}

function handleSuccess(stream, video) {
    const videoTracks = stream.getVideoTracks();
    console.log("Got stream with constraints:", constraints);
    console.log(`Using video device: ${videoTracks[0].label}`);
    window.stream = stream;
    window.vid = video;
    video.srcObject = stream;
    video.play();
}

function handleError(error, dotnet) {
    if (error.name === "ConstraintNotSatisfiedError") {
        const v = constraints.video;
        errorMsg(`resolution`, error, dotnet);
    } else if (error.name === "") {
        errorMsg("permissions", error, dotnet);
    }
    errorMsg(`getUserMedia error: ${error.name}`, error, dotnet);
}

function errorMsg(msg, error, dotnet) {
    if (typeof error !== "undefined") {
        console.error(error);
    }
    dotnet.invokeMethodAsync("OnCameraStreamingError", msg);
}

// Animation stuff below here
var box = document.getElementById("camera-container"),
    win = window,
    ww = win.innerWidth,
    wh = win.innerHeight,
    translateX = Math.floor(Math.random() * ww + 1),
    translateY = Math.floor(Math.random() * wh + 1),
    boxWidth = 320,
    boxHeight = 320,
    boxTop = box.offsetTop,
    boxLeft = box.offsetLeft,
    xMin = -boxLeft,
    yMin = -boxTop,
    xMax = win.innerWidth - boxLeft - boxWidth,
    yMax = win.innerHeight - boxTop - boxHeight,
    request = null,
    direction = "se",
    speed = 4,
    timeout = null;

export function initializeBouncing() {
    // if (!document.getElementById('camera-container').classList.contains('camera-unavailable')) {
    init();
    // }
}

// reset constraints on resize
window.addEventListener(
    "resize",
    function (argument) {
        clearTimeout(timeout);
        timeout = setTimeout(update, 100);
    },
    false
);

function init() {
    request = requestAnimationFrame(init);
    move();
    // setInterval(function() {
    //   move();
    // }, 16.66);
}

// reset constraints
function update() {
    xMin = -boxLeft;
    yMin = -boxTop;
    xMax = win.innerWidth - boxLeft - boxWidth;
    yMax = win.innerHeight - boxTop - boxHeight;
}

function move() {
    setDirection();
    setStyle(box, {
        transform: "translate3d(" + translateX + "px, " + translateY + "px, 0)",
    });
}

function setDirection() {
    switch (direction) {
        case "ne":
            translateX += speed;
            translateY -= speed;
            break;
        case "nw":
            translateX -= speed;
            translateY -= speed;
            break;
        case "se":
            translateX += speed;
            translateY += speed;
            break;
        case "sw":
            translateX -= speed;
            translateY += speed;
            break;
    }
    setLimits();
}

function setLimits() {
    if (translateY <= yMin) {
        if (direction == "nw") {
            direction = "sw";
        } else if (direction == "ne") {
            direction = "se";
        }
    }
    if (translateY >= yMax) {
        if (direction == "se") {
            direction = "ne";
        } else if (direction == "sw") {
            direction = "nw";
        }
    }
    if (translateX <= xMin) {
        if (direction == "nw") {
            direction = "ne";
        } else if (direction == "sw") {
            direction = "se";
        }
    }
    if (translateX >= xMax) {
        if (direction == "ne") {
            direction = "nw";
        } else if (direction == "se") {
            direction = "sw";
        }
    }
}

function getVendor() {
    var ua = navigator.userAgent.toLowerCase(),
        match =
            /opera/.exec(ua) ||
            /msie/.exec(ua) ||
            /firefox/.exec(ua) ||
            /(chrome|safari)/.exec(ua) ||
            /trident/.exec(ua),
        vendors = {
            opera: "-o-",
            chrome: "-webkit-",
            safari: "-webkit-",
            firefox: "-moz-",
            trident: "-ms-",
            msie: "-ms-",
        };

    return vendors[match[0]];
}

function setStyle(element, properties) {
    var prefix = getVendor(),
        property,
        css = "";
    for (property in properties) {
        css += property + ": " + properties[property] + ";";
        css += prefix + property + ": " + properties[property] + ";";
    }
    element.style.cssText += css;
}
