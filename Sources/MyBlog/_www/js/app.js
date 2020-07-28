/// <reference path="_my.ts" />
/// <reference path="_dialog.ts" />
My.ready(function () {
    // Header scroll
    window.addEventListener("scroll", function (e) {
        var distanceY = window.pageYOffset || document.documentElement.scrollTop;
        var body = this.document.getElementsByTagName("body")[0];
        if (distanceY > 300) {
            if (body.classList.contains("bigheader")) {
                body.classList.remove("bigheader");
            }
        }
        else {
            if (!body.classList.contains("bigheader")) {
                body.classList.add("bigheader");
            }
        }
    });
});
var App;
(function (App) {
    // Login dialog
    function Login() {
        Dialog.Post(document.getElementById("login").innerHTML, "/Authentication/", { "returnUrl": window.location.href });
    }
    App.Login = Login;
})(App || (App = {}));
//# sourceMappingURL=App.js.map