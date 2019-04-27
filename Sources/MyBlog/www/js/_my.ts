/// <reference path="../../node_modules/@types/jquery/jquery.d.ts" />
/// <reference path="../../node_modules/@types/bootstrap/index.d.ts" />

namespace My {
    // Call when document is ready
    export function ready(callback: () => void): void {
        if (document.readyState === "interactive" || document.readyState === "complete") {
            callback();
        } else {
            document.addEventListener('DOMContentLoaded', callback);
        }
    }

    // Dialog
    export function PostAndDisplay(title, url, data): void {
        Post(url, data, (view) => {
            // Create dialog via Vanilla JS to be ready when bootstrap 5 will drop Jquery
            let dialog = document.createElement("div");
            dialog.setAttribute("class", "modal fade");
            dialog.setAttribute("tabindex", "-1");
            dialog.setAttribute("role", "dialog");
            dialog.innerHTML = "<div class='modal-dialog' role='document'><div class='modal-content'><div class='modal-header'>"
                + "<h5 class='modal-title'>" + title + "</h5><button type='button' class='close' data-dismiss='modal' aria-label='Close'><span aria-hidden='true'>&times;</span></button></div>"
                + "<div class='modal-body'>" + view + "</div>"
                + "<div class='modal-footer'><button type='button' class='btn btn-primary' data-dismiss='modal'>Ok</button></div>"
                + "</div></div>";

            let dialogJq = $(dialog);

            // Remove dialog on close
            dialogJq.on("hidden.bs.modal", () => {
                dialogJq.remove();
            });

            // Show
            dialogJq.modal('show');
        });
    }

    // Post
    export function Post(url: string, data?: any, callback?: (response: string) => void): void {
        BlockUi();
        let request = new XMLHttpRequest();
        request.open("POST", url, true);
        if (callback) {
            request.onload = () => {
                UnBlockUi();
                if (request.status >= 200 && request.status < 400) {
                    callback(request.response);
                }
            };
        }
        if (data) {
            let params = Object.keys(data).map(
                function (k) { return encodeURIComponent(k) + '=' + encodeURIComponent(data[k]) }
            ).join('&');

            request.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.send(params);
        } else {
            request.send();
        }
    }

    export function BlockUi(): void {
        // Test if the div container is available
        let container = document.getElementById("blockui") as HTMLDivElement;
        if (container) return;

        // Create the container
        container = document.createElement("div");

        // Add the id
        container.setAttribute("id", "blockui");
        container.innerHTML = "<div class='blockuibackground'></div><div class='blockuicontainer'><div><div>"
            + "<i class='fas fa-cog fa-spin fa-5x fa-fw'></i>"
            + "<i class='fas fa-cog fa-spin2 fa-5x fa-fw' style='margin:-37px;' ></i>"
            + "<i class='fas fa-cog fa-spin fa-5x fa-fw'></i>"
            + "</div><div>Chargement...</div></div></div>";

        // Append to the body
        let body = document.getElementsByTagName("body")[0] as HTMLBodyElement;
        if (body) body.append(container);
    }

    export function UnBlockUi(): void {
        // Test if the div container is available
        let container = document.getElementById("blockui") as HTMLDivElement;
        // Remove the container
        if (container) container.remove();        
    }
}

My.ready(() => {
    let doc = $(document);
    doc.ajaxStop(My.UnBlockUi);
    doc.ajaxStart(My.BlockUi);
});