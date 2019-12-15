My.ready(function () {
    var checkbox = document.getElementById("CurrentUserSubscibed");
    if (checkbox) {
        checkbox.addEventListener("change", function () {
            var title = document.getElementById("CurrentUserSubscibedLabel").innerHTML;
            var id = document.getElementById("Post_Id").value;
            var subscription = document.getElementById("CurrentUserSubscibed").checked;
            Dialog.Post(title, "/Post/SubscribToCommentNotification", { id: id, subscription: subscription });
        });
    }
});
