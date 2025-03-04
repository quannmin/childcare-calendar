console.log("JavaScript Loaded!");

// Lấy phần tử EditForm qua id "registrationForm"
export function registerFormEvents() {
    console.log("JavaScript Loaded!");

    const registrationForm = document.getElementById("registrationForm");
    if (!registrationForm) {
        console.error("Registration form not found!");
        return; // ✅ Now it's inside a function and will work!
    }

    // More logic here...
}

// Lấy nút Next và Back
const backBtnThirdForm = document.getElementById("backButton");
const backBtnOTP = document.getElementById("backButtonOTP");
const nextBtnOTP = document.getElementById("nextButtonOTP");
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

// Gán sự kiện cho nút Back nếu có
//if (backBtnThirdForm) {
//    backBtnThirdForm.removeEventListener("click", handleBackClickThirdForm);
//    backBtnThirdForm.addEventListener("click", handleBackClickThirdForm);
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


export function handleBackClickOTP(event) {
    console.log("Back button clicked!");
    if (!registrationForm) {
        console.error("Registration form not found!");
        return;
    }
    registrationForm.classList.remove('otpActive');
    registrationForm.classList.remove('secActive');
}

//document.addEventListener("input", (e) => {
//    if (e.target.classList.contains("otp-input")) {
//        let value = e.target.value.replace(/\D/g, '');
//        e.target.value = value.charAt(0); // Limit to 1 digit

//        // Move to next input if available
//        const nextInput = e.target.nextElementSibling;
//        if (nextInput && nextInput.classList.contains("otp-input")) {
//            nextInput.focus();
//        }
//    }
//});

const inputs = document.querySelectorAll(".otp-input");
if (inputs) {
    inputs.forEach((input, index) => {
        input.addEventListener("input", (e) => {
            let value = e.target.value.replace(/\D/g, ''); // Only allow digits
            if (value.length > 1) {
                value = value.charAt(0); // Ensure only one digit is stored
            }
            e.target.value = value; // Assign cleaned value

            // Move to next input if a number is entered and it's not the last input
            if (value && index < inputs.length - 1) {
                inputs[index + 1].focus();
            }
        });

        input.addEventListener("keydown", (e) => {
            if (!/^[0-9]$/.test(e.key) && !["Backspace", "ArrowLeft", "ArrowRight"].includes(e.key)) {
                e.preventDefault();
            }

            // Move back on Backspace if field is empty
            if (e.key === "Backspace" && !e.target.value && index > 0) {
                inputs[index - 1].focus();
            }

            // Move left on ArrowLeft and set cursor at end
            if (e.key === "ArrowLeft" && index > 0) {
                inputs[index - 1].focus();
                setTimeout(() => {
                    inputs[index - 1].setSelectionRange(1, 1);
                }, 0);
            }

            // Move right on ArrowRight and set cursor at end
            if (e.key === "ArrowRight" && index < inputs.length - 1) {
                inputs[index + 1].focus();
                setTimeout(() => {
                    inputs[index + 1].setSelectionRange(1, 1);
                }, 0);
            }
        });
    });
}


