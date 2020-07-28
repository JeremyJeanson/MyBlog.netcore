/// <reference path="_ajax.ts" />

function getMoreItems(e) {
    // Get the caller button
    const button = e as HTMLButtonElement;
    const action = button.getAttribute("data-action");
    const args = button.getAttribute("data-args");
    const container = document.getElementById("items");
    if (container) {
        Ajax.Post(
            `/post/${action}/${args}`,
            undefined,
            (data) => {
                const doc = document.createElement("div");
                doc.innerHTML = data;

                const items = doc.getElementsByTagName("items")[0];
                container.innerHTML += items.innerHTML;

                const pagination = doc.getElementsByTagName("paginations")[0];
                const pagniationcontainer = document.getElementById("indexednavigation");
                pagniationcontainer.innerHTML = pagination.innerHTML;
            });
    }
}