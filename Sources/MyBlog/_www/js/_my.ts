namespace My {
    // Call when document is ready
    export function ready(callback: () => void): void {
        if (document.readyState === "interactive" || document.readyState === "complete") {
            callback();
        } else {
            document.addEventListener('DOMContentLoaded', callback);
        }
    }
}