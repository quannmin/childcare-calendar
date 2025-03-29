console.log("JavaScript Loaded!");

// Lấy phần tử EditForm qua id "registrationForm"
const registrationForm = document.getElementById("registrationForm");
function registerFormEvents() {
    console.log("JavaScript Loaded!");

    if (!registrationForm) {
        console.error("Registration form not found!");
        return; // ✅ Now it's inside a function and will work!
    }

    // More logic here...
}
const backBtnThirdForm = document.getElementById("backBtnThirdForm");
backBtnThirdForm.addEventListener("click", handleBackClickThird)
if (backBtnThirdForm) {
    console.log("sdkjhsdgdsngkjj");
}
function handleBackClickThird() {
    console.log("Third back button clicked!");
    if (!registrationForm) {
        console.error("Registration form not found!");
        return;
    }
    registrationForm.classList.remove('secActive');
}
// Lấy nút Next và Back
const backBtnOTP = document.getElementById("backButtonOTP");
const firstFormInputs = registrationForm.querySelectorAll(".form.first input:not([type='file']), .form.first select");

if (backBtnOTP) {
    console.log("backBtnOTP!");
    backBtnOTP.removeEventListener("click", handleBackClickOTP);
    backBtnOTP.addEventListener("click", handleBackClickOTP);
}

//if (nextBtnOTP) {
//    nextBtnOTP.removeEventListener("click", handleNextClickOTP);
//    nextBtnOTP.addEventListener("click", handleNextClickOTP);
//}

 export function handleNextClickFirstForm() {
    let isValid = true;

    firstFormInputs.forEach(input => {
        if (!input.value.trim()) {
            isValid = false;
            input.classList.add("is-invalid");
            console.log(`Input ${input.name} is empty`);
        } else {
            input.classList.remove("is-invalid");
        }
    });

    return isValid;
}

 export function showOTP() {
    const registrationForm = document.getElementById("registrationForm");
    if (!registrationForm) {
        console.error("Registration form not found!");
        return;
    }
    registrationForm.classList.add("otpActive");
}

 function handleBackClickOTP(event) {
    console.log("Back button clicked!");
    if (!registrationForm) {
        console.error("Registration form not found!");
        return;
    }
    registrationForm.classList.remove('otpActive');
    registrationForm.classList.remove('secActive');
}

const otpInputs = document.querySelectorAll(".otp-input");

if (otpInputs) {
    otpInputs.forEach((input, index) => {
        input.addEventListener("input", (e) => {
            let value = e.target.value.replace(/\D/g, ''); // Only allow digits
            if (value.length > 1) {
                value = value.charAt(0); // Ensure only one digit is stored
            }
            e.target.value = value; // Assign cleaned value

            // Move to next input if a number is entered and it's not the last input
            if (value && index < otpInputs.length - 1) {
                otpInputs[index + 1].focus();
            }
        });
   
        input.addEventListener("keydown", (e) => {
            if (!/^[0-9]$/.test(e.key) && !["Backspace", "ArrowLeft", "ArrowRight"].includes(e.key)) {
                e.preventDefault();
            }

            // Move back on Backspace if field is empty
            if (e.key === "Backspace" && !e.target.value && index > 0) {
                otpInputs[index - 1].focus();
            }

            // Move left on ArrowLeft and set cursor at end
            if (e.key === "ArrowLeft" && index > 0) {
                otpInputs[index - 1].focus();
                setTimeout(() => {
                    otpInputs[index - 1].setSelectionRange(1, 1);
                }, 0);
            }

            // Move right on ArrowRight and set cursor at end
            if (e.key === "ArrowRight" && index < otpInputs.length - 1) {
                otpInputs[index + 1].focus();
                setTimeout(() => {
                    otpInputs[index + 1].setSelectionRange(1, 1);
                }, 0);
            }
        });
    });
}

const nextButtonOTP = document.getElementById("nextButtonOTP");
if (nextButtonOTP) {
    nextButtonOTP.addEventListener("click", verifyOtpFromJS);
}
let blazorInstance;

export function registerBlazorInstance(instance) {
    blazorInstance = instance;
}

export async function verifyOtpFromJS() {
    const inputsOTP = document.querySelectorAll(".otp-input");
    const otpValue = Array.from(inputsOTP).map(input => input.value).join("");
    console.log(otpValue);
    if (otpValue.length !== 4) {
        console.log(otpValue);
        Swal.fire("Lỗi", "Vui lòng nhập đủ 4 số OTP!", "warning");
        return;
    }

    console.log("Sending OTP to Blazor:", otpValue);

    if (blazorInstance) {
        await blazorInstance.invokeMethodAsync("VerifyOTP", otpValue);
    } else {
        console.error("Blazor instance not initialized!");
    }
}



