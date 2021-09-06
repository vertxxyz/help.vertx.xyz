let transitionClass = "sidebar--transition";

document.addEventListener("DOMContentLoaded", () => {
	const sidebar = document.querySelector(".sidebar");
	const overlay = document.querySelector(".nav_overlay");
	const button = document.querySelector("#button-sidebar");

	button.addEventListener("click", () => open(sidebar, overlay));
	overlay.addEventListener("click", () => close(sidebar, overlay));

	$(".container").swipe({
		swipeLeft: function () {
			let width = $(document).width();
			if (width > 700) return;
			open(sidebar, overlay);
		},
		swipeRight: function () {
			let width = $(document).width();
			if (width > 700) return;
			close(sidebar, overlay);
		},
		allowPageScroll:"auto",
		excludedElements:$.fn.swipe.defaults.excludedElements+", code, pre, .editor-colors, " +
			"p, h1, h2, h3, h4, h5, a, " +
			".slider, .slider_container, .slider_knob, .slider_left_gutter, .slider_right_gutter"
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