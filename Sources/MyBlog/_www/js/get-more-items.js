My.ready(function () {
    var button = document.getElementById("btn-getmore");
    if (button) {
        button.onclick = function () {
            var action = button.getAttribute("data-action");
            var args = button.getAttribute("data-args");
            var container = document.getElementById("items");
            if (container) {
                Ajax.Post("/post/" + action + "getmore/" + args, undefined, function (data) {
                    container.innerHTML += data;
                });
            }
        };
    }
});
