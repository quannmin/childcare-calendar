window.registerFormEvents = function () {
    console.log("JavaScript Loaded!");

    // Lấy phần tử EditForm qua id "registrationForm"
    const registrationForm = document.getElementById("registrationForm");
    if (!registrationForm) {
        console.error("Registration form not found!");
        return;
    }

    // Lấy nút Next và Back
    const nextBtn = document.getElementById("nextButton");
    const backBtn = document.querySelector(".backBtn");

    // Lấy tất cả các input và select trong form-first, loại trừ input file
    const firstFormInputs = registrationForm.querySelectorAll(".form.first input:not([type='file']), .form.first select");

    if (!nextBtn) {
        console.error("Next button not found!");
        return;
    }

    // Gán sự kiện cho nút Next
    nextBtn.removeEventListener("click", handleNextClick);
    nextBtn.addEventListener("click", handleNextClick);

    // Gán sự kiện cho nút Back nếu có
    if (backBtn) {
        backBtn.removeEventListener("click", handleBackClick);
        backBtn.addEventListener("click", handleBackClick);
    }

    function handleNextClick(event) {
        console.log("Next button clicked!");

        let isValid = true;

        // Kiểm tra tất cả input và select bên form-first
        firstFormInputs.forEach(input => {
            if (!input.value.trim()) {
                isValid = false;
                // Thêm class Bootstrap để hiển thị thông báo lỗi
                input.classList.add("is-invalid");
                console.log(`Input ${input.name} is empty`);
            } else {
                input.classList.remove("is-invalid");
            }
        });

        if (isValid) {
            console.log("Form is valid, switching to second form!");
            // Thêm class 'secActive' vào registrationForm để chuyển sang hiển thị form-second
            registrationForm.classList.add('secActive');
        }
    }
};
