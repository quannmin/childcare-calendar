//window.showPreview = (inputId, imageId) => {
//    let fileInput = document.getElementById(inputId);
//    let image = document.getElementById(imageId);

//    if (!fileInput || !image) return;

//    if (fileInput.files && fileInput.files[0]) {
//        let reader = new FileReader();
//        reader.onload = function (e) {
//            image.src = e.target.result;
//            image.style.display = "block";
//        };
//        reader.readAsDataURL(fileInput.files[0]);
//    }
//};
var GLOBAL = {};
GLOBAL.DotNetReference = null;

GLOBAL.SetDotnetReference = function (pDotNetReference) {
    if (!pDotNetReference) {
        console.error("Received null DotNetReference. Check Blazor component initialization.");
        return;
    }
    console.log("DotNetReference set successfully.");
    GLOBAL.DotNetReference = pDotNetReference;
};

window.invokeBlazorOtp = async function () {
    console.log("Calling Blazor method to send OTP...");

    if (GLOBAL.DotNetReference) {
        await GLOBAL.DotNetReference.invokeMethodAsync("HandleNextClick");
    } else {
        console.error("DotNetReference is null. Ensure Blazor component initialized.");
    }
};
window.otpSentSuccess = function () {
    console.log("OTP Sent Successfully!");
    document.getElementById("registrationForm").classList.add("otpActive");
};


