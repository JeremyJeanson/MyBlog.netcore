/// <reference path="_dialog.ts" />
My.ready(function () {
    var button = document.getElementById("sendvalidationmail");
    if (button) {
        button.onclick = function () {
            Dialog.Post(button.innerHTML, "/Account/SendValidationMail", null);
        };
    }
});
//# sourceMappingURL=Account-Edit.js.map