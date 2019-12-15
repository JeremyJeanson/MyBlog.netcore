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
})(My || (My = {}));
