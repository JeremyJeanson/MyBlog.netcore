var My;
(function (My) {
    function ready(callback) {
        if (document.readyState === "interactive" || document.readyState === "complete") {
            callback();
        }
        else {
            document.addEventListener('DOMContentLoaded', callback);
        }
    }
    My.ready = ready;
    function PostAndDisplay(title, url, data) {
        Post(url, data, function (view) {
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
            dialogJq.modal('show');
        });
    }
    My.PostAndDisplay = PostAndDisplay;
    function Post(url, data, callback) {
        BlockUi();
        var request = new XMLHttpRequest();
        request.open("POST", url, true);
        if (callback) {
            request.onload = function () {
                UnBlockUi();
                if (request.status >= 200 && request.status < 400) {
                    callback(request.response);
                }
            };
        }
        if (data) {
            var params = Object.keys(data).map(function (k) { return encodeURIComponent(k) + '=' + encodeURIComponent(data[k]); }).join('&');
            request.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.send(params);
        }
        else {
            request.send();
        }
    }
    My.Post = Post;
    function BlockUi() {
        var container = document.getElementById("blockui");
        if (container)
            return;
        container = document.createElement("div");
        container.setAttribute("id", "blockui");
        container.innerHTML = "<div class='blockuibackground'></div><div class='blockuicontainer'><div><div>"
            + "<i class='fas fa-cog fa-spin fa-5x fa-fw'></i>"
            + "<i class='fas fa-cog fa-spin2 fa-5x fa-fw' style='margin:-37px;' ></i>"
            + "<i class='fas fa-cog fa-spin fa-5x fa-fw'></i>"
            + "</div><div>Chargement...</div></div></div>";
        var body = document.getElementsByTagName("body")[0];
        if (body)
            body.append(container);
    }
    My.BlockUi = BlockUi;
    function UnBlockUi() {
        var container = document.getElementById("blockui");
        if (container)
            container.remove();
    }
    My.UnBlockUi = UnBlockUi;
})(My || (My = {}));
My.ready(function () {
    var doc = $(document);
    doc.ajaxStop(My.UnBlockUi);
    doc.ajaxStart(My.BlockUi);
});
