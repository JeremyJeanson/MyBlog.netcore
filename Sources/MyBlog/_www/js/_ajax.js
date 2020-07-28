var Ajax;
(function (Ajax) {
    // Post
    function Post(url, data, callback) {
        BlockUi();
        var request = new XMLHttpRequest();
        request.open("POST", url, true);
        // Callback        
        request.onload = function (ev) {
            UnBlockUi();
            if (request.status >= 200 && request.status < 400) {
                if (callback != undefined && callback != null) {
                    callback(request.response);
                }
            }
        };
        // Error
        request.onerror = function (ev) {
            UnBlockUi();
        };
        // Data to send?
        if (data != undefined && data != null) {
            // Set forms params
            var params = Object.keys(data).map(function (k) { return encodeURIComponent(k) + '=' + encodeURIComponent(data[k]); }).join('&');
            // Send with data
            request.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.send(params);
        }
        else {
            // Send without data
            request.send();
        }
    }
    Ajax.Post = Post;
    // Block the user interface
    function BlockUi() {
        // Test if the div container is available
        var container = document.getElementById("blockui");
        if (container)
            return;
        // Create the container
        container = document.createElement("div");
        // Add the id
        container.setAttribute("id", "blockui");
        container.innerHTML = "<div class='blockuibackground'></div><div class='blockuicontainer'><div><div>"
            + "<i class='fas fa-cog fa-spin fa-5x fa-fw'></i>"
            + "<i class='fas fa-cog fa-spin2 fa-5x fa-fw' style='margin:-37px;' ></i>"
            + "<i class='fas fa-cog fa-spin fa-5x fa-fw'></i>"
            + "</div><div tabindex='-1' role='status'>Chargement...</div></div></div>";
        // Append to the body
        var body = document.getElementsByTagName("body")[0];
        if (body)
            body.append(container);
    }
    // Unblock the user interface
    function UnBlockUi() {
        // Test if the div container is available
        var container = document.getElementById("blockui");
        // Remove the container
        if (container)
            container.remove();
    }
})(Ajax || (Ajax = {}));
//# sourceMappingURL=_ajax.js.map