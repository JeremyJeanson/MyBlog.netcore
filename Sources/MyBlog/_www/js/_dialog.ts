/// <reference path="../../node_modules/@types/bootstrap/index.d.ts" />
/// <reference path="_ajax.ts" />
/// <reference path="localizations/localization.ts" />

module Dialog {
    // Post and display content inside a bootstrap modal dialog
    export function Post(title: string, url: string, data?: any, large: boolean = false): void {
        Ajax.Post(url, data, (view) => {
            const dialogSize = large === false ? '' : ' modal-lg';

            // Create dialog via Vanilla JS to be ready when bootstrap 5 will drop Jquery
            const dialog = document.createElement("div");
            dialog.classList.add("modal","fade");
            dialog.setAttribute("tabindex", "-1");
            dialog.setAttribute("role", "dialog");
            dialog.innerHTML = `<div class='modal-dialog${dialogSize}' role='document'><div class='modal-content'><div class='modal-header'>
                <h5 class='modal-title'>${title}</h5><button type='button' class='btn-close' data-bs-dismiss='modal' aria-label='${L10n.Close}'></button></div>
                <div class='modal-body'>${view}</div>
                <div class='modal-footer'><button type='button' class='btn btn-primary' data-bs-dismiss='modal'>${L10n.Close}<span class='visually-hidden'> ${L10n.CloseSuffix}</span></button></div>
                </div></div>`;

            let modal = new bootstrap.Modal(dialog);
            
            // Remove dialog on close
            dialog.addEventListener("hidden.bs.modal", () => {
                modal.dispose();
                dialog.remove();
            });

            // Show
            modal.show();
        });
    }
}