/// <reference path="_my.ts" />

My.ready(() => {
    let button = document.getElementById("btn-getmore");
    if (button) {
        button.onclick = () => {
            let action = button.getAttribute("data-action");
            let args = button.getAttribute("data-args");
            let container = document.getElementById("items");
            if (container) {
                My.Post(
                    "/Post/" + action + "GetMore/" + args,
                    undefined,
                    (data) => {
                        container.innerHTML+=data;
                    });
            }
        };
    }
});