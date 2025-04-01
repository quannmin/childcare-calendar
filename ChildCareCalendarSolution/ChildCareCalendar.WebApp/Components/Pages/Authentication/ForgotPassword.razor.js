console.log("ForgotPassword JavaScript Loaded!");

// Lấy phần tử EditForm qua id "forgotPasswordForm"
const forgotPasswordForm = document.getElementById("forgotPasswordForm");

// Lấy nút Back cho form thứ hai
const backBtnSecondForm = document.getElementById("backBtnSecondForm");
if (backBtnSecondForm) {
    backBtnSecondForm.addEventListener("click", handleBackClickSecond);
}

function handleBackClickSecond() {
    console.log("Second back button clicked!");
    if (!forgotPasswordForm) {
        console.error("Forgot password form not found!");
        return;
    }
    forgotPasswordForm.classList.remove('secActive');
    forgotPasswordForm.classList.add('otpActive');
}

// Lấy nút Back cho form OTP
const backBtnOTP = document.getElementById("backButtonOTP");
const firstFormInputs = forgotPasswordForm ? forgotPasswordForm.querySelectorAll(".form.first input") : [];

if (backBtnOTP) {
    console.log("backBtnOTP found!");
    backBtnOTP.removeEventListener("click", handleBackClickOTP);
    backBtnOTP.addEventListener("click", handleBackClickOTP);
}

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
    const forgotPasswordForm = document.getElementById("forgotPasswordForm");
    if (!forgotPasswordForm) {
        console.error("Forgot password form not found!");
        return;
    }
    forgotPasswordForm.classList.add("otpActive");
}

function handleBackClickOTP(event) {
    console.log("Back button clicked!");
    if (!forgotPasswordForm) {
        console.error("Forgot password form not found!");
        return;
    }
    forgotPasswordForm.classList.remove('otpActive');
}

// Xử lý ô input OTP
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

// Xử lý nút Next cho OTP
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

    if (otpValue.length !== 4) {
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

// Đảm bảo event listeners được đăng ký khi DOM load xong
document.addEventListener("DOMContentLoaded", function () {
    console.log("DOM fully loaded");

    // Re-select elements if needed
    const forgotPasswordForm = document.getElementById("forgotPasswordForm");
    const backBtnSecondForm = document.getElementById("backBtnSecondForm");
    const backBtnOTP = document.getElementById("backButtonOTP");
    const nextButtonOTP = document.getElementById("nextButtonOTP");

    if (backBtnSecondForm) {
        backBtnSecondForm.addEventListener("click", handleBackClickSecond);
    }

    if (backBtnOTP) {
        backBtnOTP.addEventListener("click", handleBackClickOTP);
    }

    if (nextButtonOTP) {
        nextButtonOTP.addEventListener("click", verifyOtpFromJS);
    }
});