// Add an icon inside links to external websites
let anchors = document.querySelectorAll("article div.content a") as NodeListOf<HTMLAnchorElement>;
if (anchors) {
	anchors.forEach(anchor => {
		if (anchor.hostname && anchor.hostname !== location.hostname) {
			anchor.innerHTML += " <i class='fa-solid fa-arrow-up-right-from-square fa-2xs'></i>";
		}
	})
}