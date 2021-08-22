let transitionClass = "sidebar--transition";

document.addEventListener("DOMContentLoaded", () => {
	const sidebar = document.querySelector(".sidebar");
	const overlay = document.querySelector(".nav_overlay");
	const button = document.querySelector("#button-sidebar");

	button.addEventListener("click", () => open(sidebar, overlay));
	overlay.addEventListener("click", () => close(sidebar, overlay));

	$(".container").swipe({
		swipe: function (event, direction, distance, duration, fingerCount, fingerData) {
			let width = $(document).width();
			if (width > 700) return;
			if (direction === "left") {
				open(sidebar, overlay);
			} else if (direction === "right") {
				close(sidebar, overlay);
			}
		}
	});
});

function open(sidebar, overlay) {
	if(sidebar.classList.contains("sidebar--open"))
		return;
	sidebar.classList.add(transitionClass);
	setTimeout(function () {
		sidebar.classList.remove(transitionClass);
	}, 300);
	sidebar.classList.add("sidebar--open");
	overlay.classList.add("nav_overlay--open");
}

function close(sidebar, overlay) {
	if(!sidebar.classList.contains("sidebar--open"))
		return;
	sidebar.classList.add(transitionClass);
	setTimeout(function () {
		sidebar.classList.remove(transitionClass);
	}, 300);
	sidebar.classList.remove("sidebar--open");
	overlay.classList.remove("nav_overlay--open");
}