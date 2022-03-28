/// <reference path="../../node_modules/@types/bootstrap/index.d.ts" />
/// <reference path="_ajax.ts" />
/// <reference path="localizations/localization.ts" />
var Dialog;
(function (Dialog) {
    // Post and display content inside a bootstrap modal dialog
    function Post(title, url, data, large) {
        if (large === void 0) { large = false; }
        Ajax.Post(url, data, function (view) {
            var dialogSize = large === false ? '' : ' modal-lg';
            // Create dialog via Vanilla JS to be ready when bootstrap 5 will drop Jquery
            var dialog = document.createElement("div");
            dialog.classList.add("modal", "fade");
            dialog.setAttribute("tabindex", "-1");
            dialog.setAttribute("role", "dialog");
            dialog.innerHTML = "<div class='modal-dialog".concat(dialogSize, "' role='document'><div class='modal-content'><div class='modal-header'>\n                <h5 class='modal-title'>").concat(title, "</h5><button type='button' class='btn-close' data-bs-dismiss='modal' aria-label='").concat(L10n.Close, "'></button></div>\n                <div class='modal-body'>").concat(view, "</div>\n                <div class='modal-footer'><button type='button' class='btn btn-primary' data-bs-dismiss='modal'>").concat(L10n.Close, "<span class='visually-hidden'> ").concat(L10n.CloseSuffix, "</span></button></div>\n                </div></div>");
            var modal = new bootstrap.Modal(dialog);
            // Remove dialog on close
            dialog.addEventListener("hidden.bs.modal", function () {
                modal.dispose();
                dialog.remove();
            });
            // Show
            modal.show();
        });
    }
    Dialog.Post = Post;
})(Dialog || (Dialog = {}));
//# sourceMappingURL=_dialog.js.map