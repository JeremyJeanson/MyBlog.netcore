/// <reference path="../../node_modules/@types/bootstrap/index.d.ts" />
/// <reference path="../../node_modules//clipboard/src/clipboard.d.ts" />
/// <reference path="localizations/localization.ts" />
var prismClipboard;
(function (prismClipboard) {
    // Add buttons to all <pre><code>...
    // Get Pre
    var pre = document.getElementsByTagName("pre");
    for (var i = pre.length - 1; i >= 0; i--) {
        // Try to get a Code
        var code = pre[i].getElementsByTagName("code");
        if (code !== undefined) {
            // Create a clipboard button
            var button = document.createElement("button");
            // Icon & accessibility
            button.innerHTML = "<i class='far fa-copy'></i>";
            button.setAttribute("arial-label", L10n.Copy);
            // Style
            button.classList.add("btn", "float-end");
            // Tooltip
            new bootstrap.Tooltip(button, { title: L10n.Copy, placement: "top" });
            // Clipboard
            var clipboard = new ClipboardJS(button, {
                target: clipboardTarget
            });
            clipboard.on("success", clipboardSuccess);
            clipboard.on("error", clipboardError);
            // Insert the button before the code
            pre[i].insertBefore(button, code[0]);
        }
    }
    // Target of ClipboadJS
    function clipboardTarget(trigger) {
        return trigger.nextElementSibling;
    }
    // Copy sucess
    function clipboardSuccess(e) {
        var tooltip = bootstrap.Tooltip.getInstance(e.trigger);
        e.trigger.setAttribute("data-bs-original-title", L10n.Copied);
        tooltip.show();
        e.trigger.setAttribute("data-bs-original-title", L10n.Copy);
        e.clearSelection();
    }
    // Copy error
    function clipboardError(e) {
        var tooltip = bootstrap.Tooltip.getInstance(e.trigger);
        e.trigger.setAttribute("data-bs-original-title", "Error :(");
        tooltip.show();
        e.trigger.setAttribute("data-bs-original-title", L10n.Copy);
    }
})(prismClipboard || (prismClipboard = {}));
//# sourceMappingURL=prism-clipboard.js.map