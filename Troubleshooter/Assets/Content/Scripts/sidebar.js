let transitionClass = "sidebar--transition";

whenReady(() => {
	const sidebar = document.querySelector(".sidebar");
	const overlay = document.querySelector(".nav_overlay");
	const button = document.querySelector("#button-sidebar");

	button.addEventListener("click", () => toggle(sidebar, overlay, button));
	overlay.addEventListener("click", () => close(sidebar, overlay, button));
});

function toggle(sidebar, overlay, button) {
	if (sidebar.classList.contains("sidebar--open"))
		close(sidebar, overlay, button);
	else
		open(sidebar, overlay, button);
}

function open(sidebar, overlay, button) {
	if (sidebar.classList.contains("sidebar--open"))
		return;
	sidebar.classList.add(transitionClass);
	setTimeout(function () {
		sidebar.classList.remove(transitionClass);
	}, 300);
	sidebar.classList.add("sidebar--open");
	overlay.classList.add("nav_overlay--open");
	button.classList.add("sidebar__button--open");
}

function close(sidebar, overlay, button) {
	if (!sidebar.classList.contains("sidebar--open"))
		return;
	sidebar.classList.add(transitionClass);
	setTimeout(function () {
		sidebar.classList.remove(transitionClass);
	}, 300);
	sidebar.classList.remove("sidebar--open");
	overlay.classList.remove("nav_overlay--open");
	button.classList.remove("sidebar__button--open");
}