My.ready(function(){var e=document.getElementById("CurrentUserSubscibed");e&&e.addEventListener("change",function(){var e=document.getElementById("CurrentUserSubscibedLabel").innerHTML,t=document.getElementById("Post_Id").value,n=document.getElementById("CurrentUserSubscibed").checked;My.PostAndDisplay(e,"/Post/SubscribToCommentNotification",{id:t,subscription:n})})});