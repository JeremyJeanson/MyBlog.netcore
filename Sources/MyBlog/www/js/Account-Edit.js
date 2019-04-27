My.ready(function () {
    var button = document.getElementById("sendvalidationmail");
    if (button) {
        button.onclick = function () {
            My.PostAndDisplay(button.innerHTML, "/Account/SendValidationMail", null);
        };
    }
});
