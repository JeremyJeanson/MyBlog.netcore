var Dialog;
(function (Dialog) {
    function Post(title, url, data) {
        Ajax.Post(url, data, function (view) {
            var dialog = document.createElement("div");
            dialog.setAttribute("class", "modal fade");
            dialog.setAttribute("tabindex", "-1");
            dialog.setAttribute("role", "dialog");
            dialog.innerHTML = "<div class='modal-dialog' role='document'><div class='modal-content'><div class='modal-header'>"
                + "<h5 class='modal-title'>" + title + "</h5><button type='button' class='close' data-dismiss='modal' aria-label='Close'><span aria-hidden='true'>&times;</span></button></div>"
                + "<div class='modal-body'>" + view + "</div>"
                + "<div class='modal-footer'><button type='button' class='btn btn-primary' data-dismiss='modal'>Ok</button></div>"
                + "</div></div>";
            var dialogJq = $(dialog);
            dialogJq.on("hidden.bs.modal", function () {
                dialogJq.remove();
            });
            dialogJq.modal("show");
        });
    }
    Dialog.Post = Post;
})(Dialog || (Dialog = {}));
