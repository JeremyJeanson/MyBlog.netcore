/// <reference path="_dialog.ts" />

My.ready(() => {
    let button = document.getElementById("sendvalidationmail");
    if (button) {
        button.onclick = () => {
            Dialog.Post(button.innerHTML, "/Account/SendValidationMail", null);
        };
    }
});