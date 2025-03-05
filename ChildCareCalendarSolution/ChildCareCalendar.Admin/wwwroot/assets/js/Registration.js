window.registerFormEvents = function () {
    console.log("JavaScript Loaded!");

    // Lấy phần tử EditForm qua id "registrationForm"
    const registrationForm = document.getElementById("registrationForm");
    if (!registrationForm) {
        console.error("Registration form not found!");
        return;
    }

    // Lấy nút Next và Back
    const nextBtnFirstForm = document.getElementById("nextButton");
    const backBtnThirdForm = document.getElementById("backButton");
    const backBtnOTP = document.getElementById("backButtonOTP");
    const nextBtnOTP = document.getElementById("nextButtonOTP");
    const firstFormInputs = registrationForm.querySelectorAll(".form.first input:not([type='file']), .form.first select");

    if (backBtnOTP) {
        console.log("backBtnOTP!");
        backBtnOTP.removeEventListener("click", handleBackClickOTP);
        backBtnOTP.addEventListener("click", handleBackClickOTP);
    }
    // Gán sự kiện cho nút Next
    if (nextBtnFirstForm) {
        console.log("nextBtnFirstForm!");
        nextBtnFirstForm.removeEventListener("click", handleNextClickFirstForm);
        nextBtnFirstForm.addEventListener("click", handleNextClickFirstForm);
    }

    if (nextBtnOTP) {
        nextBtnOTP.removeEventListener("click", handleNextClickOTP);
        nextBtnOTP.addEventListener("click", handleNextClickOTP);
    }

    // Gán sự kiện cho nút Back nếu có
    if (backBtnThirdForm) {
        backBtnThirdForm.removeEventListener("click", handleBackClickThirdForm);
        backBtnThirdForm.addEventListener("click", handleBackClickThirdForm);
    }

    function handleNextClickFirstForm(event) {
        console.log("Next button first form clicked!");

        let isValid = true;

        // Validate first form inputs
        firstFormInputs.forEach(input => {
            if (!input.value.trim()) {
                isValid = false;
                input.classList.add("is-invalid");
                console.log(`Input ${input.name} is empty`);
            } else {
                input.classList.remove("is-invalid");
            }
        });

        if (isValid) {
            console.log("Valid form. Transitioning to OTP form!");
            window.invokeBlazorOtp();
        }
    }
   
    function handleNextClickOTP(event) {
       
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

    const inputs = document.querySelectorAll(".otp-input");
    if (inputs) {
        inputs.forEach((input, index) => {
            input.addEventListener("input", (e) => {
                let value = e.target.value.replace(/\D/g, ''); // Ensure only numbers
                e.target.value = value; // Limit input to one character

                // Move to next input if a number is entered
                if (value.length === 1 && index < inputs.length - 1) {
                    inputs[index + 1].focus();
                }
            });

            input.addEventListener("keydown", (e) => {
                // Allow only numbers, Backspace, ArrowLeft, and ArrowRight
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
                        inputs[index - 1].setSelectionRange(1, 1); // Move cursor to end
                    }, 0);
                }

                // Move right on ArrowRight and set cursor at end
                if (e.key === "ArrowRight" && index < inputs.length - 1) {
                    inputs[index + 1].focus();
                    setTimeout(() => {
                        inputs[index + 1].setSelectionRange(1, 1); // Move cursor to end
                    }, 0);
                }
            });
        });
    }

};
