let transitionClass = "sidebar--transition";

whenReady(() => {
	const sidebar = document.querySelector(".sidebar");
	const overlay = document.querySelector(".nav_overlay");
	const button = document.querySelector("#button-sidebar");

	button.addEventListener("click", () => open(sidebar, overlay));
	overlay.addEventListener("click", () => close(sidebar, overlay));
});

function open(sidebar, overlay) {
	if (sidebar.classList.contains("sidebar--open"))
		return;
	sidebar.classList.add(transitionClass);
	setTimeout(function () {
		sidebar.classList.remove(transitionClass);
	}, 300);
	sidebar.classList.add("sidebar--open");
	overlay.classList.add("nav_overlay--open");
}

function close(sidebar, overlay) {
	if (!sidebar.classList.contains("sidebar--open"))
		return;
	sidebar.classList.add(transitionClass);
	setTimeout(function () {
		sidebar.classList.remove(transitionClass);
	}, 300);
	sidebar.classList.remove("sidebar--open");
	overlay.classList.remove("nav_overlay--open");
}