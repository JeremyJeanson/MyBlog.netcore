var Ajax;
(function (Ajax) {
    function Post(url, data, callback) {
        BlockUi();
        var request = new XMLHttpRequest();
        request.open("POST", url, true);
        request.onload = function (ev) {
            UnBlockUi();
            if (request.status >= 200 && request.status < 400) {
                if (callback != undefined && callback != null) {
                    callback(request.response);
                }
            }
        };
        request.onerror = function (ev) {
            UnBlockUi();
        };
        if (data != undefined && data != null) {
            var params = Object.keys(data).map(function (k) { return encodeURIComponent(k) + '=' + encodeURIComponent(data[k]); }).join('&');
            request.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.send(params);
        }
        else {
            request.send();
        }
    }
    Ajax.Post = Post;
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
            + "</div><div tabindex='-1' role='status'>Chargement...</div></div></div>";
        var body = document.getElementsByTagName("body")[0];
        if (body)
            body.append(container);
    }
    function UnBlockUi() {
        var container = document.getElementById("blockui");
        if (container)
            container.remove();
    }
})(Ajax || (Ajax = {}));
