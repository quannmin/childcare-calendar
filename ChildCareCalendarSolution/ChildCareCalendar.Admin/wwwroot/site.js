window.showPreview = (inputId, imageId) => {
    let fileInput = document.getElementById(inputId);
    let image = document.getElementById(imageId);

    if (!fileInput || !image) return;

    if (fileInput.files && fileInput.files[0]) {
        let reader = new FileReader();
        reader.onload = function (e) {
            image.src = e.target.result;
            image.style.display = "block";
        };
        reader.readAsDataURL(fileInput.files[0]);
    }
};

