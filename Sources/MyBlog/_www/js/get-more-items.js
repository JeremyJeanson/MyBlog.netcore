/// <reference path="_ajax.ts" />
function getMoreItems(e) {
    // Get the caller button
    var button = e;
    var action = button.getAttribute("data-action");
    var args = button.getAttribute("data-args");
    var container = document.getElementById("items");
    if (container) {
        Ajax.Post("/post/" + action + "/" + args, undefined, function (data) {
            var doc = document.createElement("div");
            doc.innerHTML = data;
            var items = doc.getElementsByTagName("items")[0];
            container.innerHTML += items.innerHTML;
            var pagination = doc.getElementsByTagName("paginations")[0];
            var pagniationcontainer = document.getElementById("indexednavigation");
            pagniationcontainer.innerHTML = pagination.innerHTML;
        });
    }
}
//# sourceMappingURL=Get-More-Items.js.map