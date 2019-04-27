/// <reference path="_my.ts" />

My.ready(() => {
    // Header scroll
    window.addEventListener('scroll', function (e) {
        let distanceY = window.pageYOffset || document.documentElement.scrollTop;
        let body = this.document.getElementsByTagName("body")[0] as HTMLBodyElement;

        if (distanceY > 300) {
            if (body.classList.contains("bigheader")) {
                body.classList.remove("bigheader");
            }
        } else {
            if (!body.classList.contains("bigheader")) {
                body.classList.add("bigheader");
            }
        }
    });
});

namespace App {
    export function Login(): void {
        My.PostAndDisplay(
            document.getElementById("login").innerHTML,
            "/Authentication/",
            { "returnUrl": window.location.href }
        );
    }
}