// Depend on clipboard.js

var snippets = document.querySelectorAll("pre[class]");
[].forEach.call(snippets, function (snippet) {
    snippet.firstChild.insertAdjacentHTML("beforebegin", "<button class='btn float-right' data-placement='left' data-clipboard-code title='Copy to clipboard' aria-label='Copy to clipboard'><i class='far fa-copy'></i></button>");
});
var clipboardSnippets = new ClipboardJS("[data-clipboard-code]", {
    target: function (trigger) {
        return trigger.nextElementSibling;
    }
});
clipboardSnippets.on("success", function (e) {    
    $(e.trigger).attr("title", "Copied!").tooltip("_fixTitle").tooltip("show").attr("title", "Copy to clipboard").tooltip("_fixTitle");
    e.clearSelection();
});
clipboardSnippets.on("error", function (e) {
    $(e.trigger).attr("title", fallbackMessage(e.action)).tooltip("_fixTitle").tooltip("show").attr("title", "Copy to clipboard").tooltip("_fixTitle");
});
$("[data-clipboard-code]").tooltip();