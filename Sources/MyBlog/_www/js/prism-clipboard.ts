/// <reference path="../../node_modules/@types/bootstrap/index.d.ts" />
/// <reference path="../../node_modules//clipboard/src/clipboard.d.ts" />
/// <reference path="localizations/localization.ts" />

module prismClipboard
{
    // Add buttons to all <pre><code>...
    // Get Pre
    const pre = document.getElementsByTagName("pre");
    for (let i = pre.length - 1; i >= 0; i--) {
        // Try to get a Code
        const code = pre[i].getElementsByTagName("code");
        if (code !== undefined) {
            // Create a clipboard button
            const button = document.createElement("button");
            // Icon & accessibility
            button.innerHTML = "<i class='far fa-copy'></i>";
            button.setAttribute("arial-label", L10n.Copy);
            // Style
            button.classList.add("btn", "float-end");
            // Tooltip
            new bootstrap.Tooltip(button, { title: L10n.Copy, placement: "top" });
            // Clipboard
            const clipboard = new ClipboardJS(button, {
                target: clipboardTarget
            });
            clipboard.on("success", clipboardSuccess);
            clipboard.on("error", clipboardError);

            // Insert the button before the code
            pre[i].insertBefore(button, code[0]);
        }
    }

    // Target of ClipboadJS
    function clipboardTarget(trigger: Element): Element {
        return trigger.nextElementSibling;
    }

    // Copy sucess
    function clipboardSuccess(e: ClipboardJS.Event) {
        const tooltip = bootstrap.Tooltip.getInstance(e.trigger);
        e.trigger.setAttribute("data-bs-original-title", L10n.Copied);
        tooltip.show();
        e.trigger.setAttribute("data-bs-original-title", L10n.Copy);
        e.clearSelection();
    }

    // Copy error
    function clipboardError(e: ClipboardJS.Event) {
        const tooltip = bootstrap.Tooltip.getInstance(e.trigger);
        e.trigger.setAttribute("data-bs-original-title", "Error :(");
        tooltip.show();
        e.trigger.setAttribute("data-bs-original-title", L10n.Copy);
    }
}