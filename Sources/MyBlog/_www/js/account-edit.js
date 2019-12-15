My.ready(function () {
    var button = document.getElementById("sendvalidationmail");
    if (button) {
        button.onclick = function () {
            Dialog.Post(button.innerHTML, "/Account/SendValidationMail", null);
        };
    }
});
