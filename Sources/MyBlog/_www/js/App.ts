/// <reference path="_my.ts" />
/// <reference path="_dialog.ts" />
/// <reference path="localizations/localization.ts" />

// Document is ready
My.ready(() => {
    // Header scroll
    window.addEventListener("scroll", function (e) {
        const distanceY = window.pageYOffset || document.documentElement.scrollTop;
        const body = this.document.getElementsByTagName("body")[0] as HTMLBodyElement;

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

    // Tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    })
});

module App {
    // Login dialog
    export function Login(): void {
        Dialog.Post(
            `<i class="fas fa-sign-in-alt"></i> ${L10n.LogInOrSignUp}`,
            "/Authentication/",
            { "returnUrl": window.location.href }
        );
    }
}