/// <reference path="_dialog.ts" />

My.ready(() => {
    // Allow user to subscrib/unsubscribe to comment notifications
    const checkbox = document.getElementById("CurrentUserSubscibed") as HTMLInputElement;
    if (checkbox) {
        checkbox.addEventListener("change",
            () => {
                const title = document.getElementById("CurrentUserSubscibedLabel").innerHTML;
                const id = (document.getElementById("Post_Id") as HTMLInputElement).value;
                const subscription = (document.getElementById("CurrentUserSubscibed") as HTMLInputElement).checked;
                Dialog.Post(
                    title,
                    "/Post/SubscribToCommentNotification",
                    { id: id, subscription: subscription });
            });
    }
});