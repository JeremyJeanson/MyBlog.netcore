// Add an icon inside links to external websites
var anchors = document.querySelectorAll("article div.content a");
if (anchors) {
    anchors.forEach(function (anchor) {
        if (anchor.hostname && anchor.hostname !== location.hostname) {
            anchor.innerHTML += " <i class='fa-solid fa-arrow-up-right-from-square fa-2xs'></i>";
        }
    });
}
//# sourceMappingURL=External-anchors.js.map