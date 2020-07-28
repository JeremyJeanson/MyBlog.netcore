namespace Ajax {
    // Post
    export function Post(url: string, data?: any, callback?: (response: string) => void): void {
        BlockUi();
        const request = new XMLHttpRequest();
        request.open("POST", url, true);

        // Callback        
        request.onload = function (this: XMLHttpRequest, ev: ProgressEvent): any {
            UnBlockUi();
            if (request.status >= 200 && request.status < 400) {
                if (callback != undefined && callback != null) {
                    callback(request.response);
                }
            }
        };        

        // Error
        request.onerror = function (this: XMLHttpRequest, ev: ProgressEvent): any  {
            UnBlockUi();
        };

        // Data to send?
        if (data != undefined && data != null) {
            // Set forms params
            let params = Object.keys(data).map(
                function (k) { return encodeURIComponent(k) + '=' + encodeURIComponent(data[k]) }
            ).join('&');

            // Send with data
            request.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.send(params);
        } else {
            // Send without data
            request.send();
        }
    }

    // Block the user interface
    function BlockUi(): void {
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
            + "</div><div tabindex='-1' role='status'>Chargement...</div></div></div>";

        // Append to the body
        let body = document.getElementsByTagName("body")[0] as HTMLBodyElement;
        if (body) body.append(container);
    }

    // Unblock the user interface
    function UnBlockUi(): void {
        // Test if the div container is available
        const container = document.getElementById("blockui") as HTMLDivElement;
        // Remove the container
        if (container) container.remove();
    }
}