var My;
(function (My) {
    // Call when document is ready
    function ready(callback) {
        if (document.readyState === "interactive" || document.readyState === "complete") {
            callback();
        }
        else {
            document.addEventListener('DOMContentLoaded', callback);
        }
    }
    My.ready = ready;
})(My || (My = {}));
//# sourceMappingURL=_my.js.map