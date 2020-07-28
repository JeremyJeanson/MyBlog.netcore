/// <reference path="_dialog.ts" />

My.ready(() => {
    const button = document.getElementById("sendvalidationmail");
    if (button) {
        button.onclick = () => {
            Dialog.Post(button.innerHTML, "/Account/SendValidationMail", null);
        };
    }
});