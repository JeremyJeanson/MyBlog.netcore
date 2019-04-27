/// <reference path="_my.ts" />

My.ready(() => {
    let checkbox = document.getElementById("CurrentUserSubscibed") as HTMLInputElement;
    if (checkbox) {
        checkbox.addEventListener("change",
            () => {
                let title = document.getElementById("CurrentUserSubscibedLabel").innerHTML;
                let id = (document.getElementById("Post_Id") as HTMLInputElement).value;
                let subscription = (document.getElementById("CurrentUserSubscibed") as HTMLInputElement).checked;
                My.PostAndDisplay(
                    title,
                    "/Post/SubscribToCommentNotification",
                    { id: id, subscription: subscription });
            });
    }
});