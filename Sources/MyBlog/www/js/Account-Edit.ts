/// <reference path="_my.ts" />
/// <reference path="../../node_modules/@types/jquery/jquery.d.ts" />
/// <reference path="../../node_modules/@types/bootstrap/index.d.ts" />

My.ready(() => {
    let button = document.getElementById("sendvalidationmail");
    if (button) {
        button.onclick = () => {
            My.PostAndDisplay(button.innerHTML, "/Account/SendValidationMail", null);
        };
    }
});