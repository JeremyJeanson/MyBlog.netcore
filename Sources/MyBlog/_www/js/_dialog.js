/// <reference path="../../node_modules/@types/jquery/jquery.d.ts" />
/// <reference path="../../node_modules/@types/bootstrap/index.d.ts" />
/// <reference path="_ajax.ts" />
/// <reference path="localizations/localization.ts" />
var Dialog;
(function (Dialog) {
    // Post and display content inside a bootstrap modal dialog
    function Post(title, url, data) {
        Ajax.Post(url, data, function (view) {
            // Create dialog via Vanilla JS to be ready when bootstrap 5 will drop Jquery
            var dialog = document.createElement("div");
            dialog.setAttribute("class", "modal fade");
            dialog.setAttribute("tabindex", "-1");
            dialog.setAttribute("role", "dialog");
            dialog.innerHTML = "<div class='modal-dialog' role='document'><div class='modal-content'><div class='modal-header'>\n                <h5 class='modal-title'>" + title + "</h5><button type='button' class='close' data-dismiss='modal' aria-label='" + L10n.Close + "'><span aria-hidden='true'>&times;</span></button></div>\n                <div class='modal-body'>" + view + "</div>\n                <div class='modal-footer'><button type='button' class='btn btn-primary' data-dismiss='modal'>" + L10n.Close + "<span class='sr-only'> " + L10n.CloseSuffix + "</span></button></div>\n                </div></div>";
            var dialogJq = $(dialog);
            // Remove dialog on close
            dialogJq.on("hidden.bs.modal", function () {
                dialogJq.remove();
            });
            // Show
            dialogJq.modal("show");
        });
    }
    Dialog.Post = Post;
})(Dialog || (Dialog = {}));
//# sourceMappingURL=_dialog.js.map