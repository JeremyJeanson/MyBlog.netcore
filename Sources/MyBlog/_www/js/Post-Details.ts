/// <reference path="_dialog.ts" />

My.ready(() => {
    // Allow user to subscrib/unsubscribe to comment notifications
    let checkbox = document.getElementById("CurrentUserSubscibed") as HTMLInputElement;
    if (checkbox) {
        checkbox.addEventListener("change",
            () => {
                let title = document.getElementById("CurrentUserSubscibedLabel").innerHTML;
                let id = (document.getElementById("Post_Id") as HTMLInputElement).value;
                let subscription = (document.getElementById("CurrentUserSubscibed") as HTMLInputElement).checked;
                Dialog.Post(
                    title,
                    "/Post/SubscribToCommentNotification",
                    { id: id, subscription: subscription });
            });
    }
});