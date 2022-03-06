import {ray} from "./spacialMaths.js";

const TAU = Math.PI * 2;
const Deg2Rad = 0.01745329;
const Rad2Deg = 57.29578;

document.addEventListener("DOMContentLoaded", function () {
	if (window.bc_touch_down_state === undefined) {
		window.bc_touch_down_state = false;
		document.addEventListener("touchstart", function (e) {
			window.bc_touch_down_state = true;
		}, false);
		document.addEventListener("touchend", function (e) {
			window.bc_touch_down_state = false;
		}, false);

		document.addEventListener("touchcancel", function (e) {
			window.bc_touch_down_state = false;
		}, false);
	}
});

function TouchHandler(target, begin, move, end) {
	target.onmousedown = mouse_down;
	target.ontouchstart = genericTouchHandler(mouse_down);

	const move_handler = genericTouchHandler(mouse_move);

	function mouse_down(e) {
		let res = begin ? begin(e) : true;

		if (res && e.preventDefault)
			e.preventDefault();
		
		if(res === false)
			return false;

		window.addEventListener("mousemove", mouse_move, false);
		window.addEventListener("mouseup", mouse_up, false);

		window.addEventListener("touchmove", move_handler, false);
		window.addEventListener("touchend", mouse_up, false);
		window.addEventListener("touchcancel", mouse_up, false);
		
		return true;
	}

	function mouse_move(e) {
		return move ? move(e) : true;
	}

	function mouse_up(e) {
		window.removeEventListener("mousemove", mouse_move, false);
		window.removeEventListener("mouseup", mouse_up, false);

		window.removeEventListener("touchmove", move_handler, false);
		window.removeEventListener("touchend", mouse_up, false);
		window.removeEventListener("touchcancel", mouse_up, false);

		return end ? end(e) : true;
	}
}

function Dragger(target, callback) {

	target.onmousedown = mouse_down;
	target.ontouchstart = genericTouchHandler(mouse_down);

	const move_handler = genericTouchHandler(mouse_move);
	let prev_mouse_x, prev_mouse_y;

	function mouse_down(e) {

		prev_mouse_x = e.clientX;
		prev_mouse_y = e.clientY;


		window.addEventListener("mousemove", mouse_move, false);
		window.addEventListener("mouseup", mouse_up, false);

		window.addEventListener("touchmove", move_handler, false);
		window.addEventListener("touchend", mouse_up, false);
		window.addEventListener("touchcancel", mouse_up, false);

		if (e.preventDefault)
			e.preventDefault();

		return true;
	}

	function mouse_move(e) {
		callback(e.clientX - prev_mouse_x, e.clientY - prev_mouse_y);

		prev_mouse_x = e.clientX;
		prev_mouse_y = e.clientY;

		return true;
	}

	function mouse_up(e) {
		window.removeEventListener("mousemove", mouse_move, false);
		window.removeEventListener("mouseup", mouse_up, false);

		window.removeEventListener("touchmove", move_handler, false);
		window.removeEventListener("touchend", mouse_up, false);
		window.removeEventListener("touchcancel", mouse_up, false);
	}
}

function genericTouchHandler(f) {
	return function (e) {
		if (e.touches.length === 1) {
			if (f(e.touches[0])) {
				e.preventDefault();
				return false;
			}
		}
	}
}

function Slider(container_div, callback, style_prefix, default_value, disable_click) {
	const container = container_div.querySelector('.slider_container');
	const left_gutter = container.querySelector('.slider_left_gutter');
	const right_gutter = container.querySelector('.slider_right_gutter');

	if (!disable_click) {
		left_gutter.onclick = mouse_click;
		right_gutter.onclick = mouse_click;
	}

	const knob_container = container.querySelector('.slider_knob_container');
	const knob = knob_container.querySelector('.slider_knob');

	knob.onmousedown = mouse_down;
	knob.ontouchstart = genericTouchHandler(mouse_down);

	window.addEventListener("resize", layout, true);

	let percentage = default_value === undefined ? 0.5 : default_value;

	layout();
	callback(percentage);

	function layout() {
		const width = container.getBoundingClientRect().width;

		left_gutter.style.width = width * percentage + "px";
		left_gutter.style.left = "0";

		right_gutter.style.width = (width * (1.0 - percentage)) + "px";
		right_gutter.style.left = width * percentage + "px";

		knob_container.style.left = (width * percentage) + "px"
	}

	let selection_offset;

	const move_handler = genericTouchHandler(mouse_move);

	function mouse_down(e) {

		if (window.bc_touch_down_state)
			return false;

		const knob_rect = knob_container.getBoundingClientRect();
		selection_offset = e.clientX - knob_rect.left - knob_rect.width / 2;

		window.addEventListener("mousemove", mouse_move, false);
		window.addEventListener("mouseup", mouse_up, false);

		window.addEventListener("touchmove", move_handler, false);
		window.addEventListener("touchend", mouse_up, false);
		window.addEventListener("touchcancel", mouse_up, false);


		if (e.preventDefault)
			e.preventDefault();
		return true;
	}

	function mouse_move(e) {
		const container_rect = container.getBoundingClientRect();
		const x = e.clientX - selection_offset - container_rect.left;

		const p = Math.max(0, Math.min(1.0, x / container_rect.width));

		if (percentage !== p) {
			percentage = p;
			layout();
			callback(p);
		}

		return true;
	}

	function mouse_up(e) {
		window.removeEventListener("mousemove", mouse_move, false);
		window.removeEventListener("mouseup", mouse_up, false);

		window.removeEventListener("touchmove", move_handler, false);
		window.removeEventListener("touchend", mouse_up, false);
		window.removeEventListener("touchcancel", mouse_up, false);
	}

	function mouse_click(e) {
		const container_rect = container.getBoundingClientRect();
		const x = e.clientX - container_rect.left;

		const p = Math.max(0, Math.min(1.0, x / container_rect.width));

		if (percentage !== p) {
			percentage = p;
			layout();
			callback(p);
		}

		return true;
	}
}

const EasingFunctions = {
	// no easing, no acceleration
	linear: t => t,
	// accelerating from zero velocity
	easeInQuad: t => t * t,
	// decelerating to zero velocity
	easeOutQuad: t => t * (2 - t),
	// acceleration until halfway, then deceleration
	easeInOutQuad: t => t < .5 ? 2 * t * t : -1 + (4 - 2 * t) * t,
	// accelerating from zero velocity 
	easeInCubic: t => t * t * t,
	// decelerating to zero velocity 
	easeOutCubic: t => (--t) * t * t + 1,
	// acceleration until halfway, then deceleration 
	easeInOutCubic: t => t < .5 ? 4 * t * t * t : (t - 1) * (2 * t - 2) * (2 * t - 2) + 1,
	// accelerating from zero velocity 
	easeInQuart: t => t * t * t * t,
	// decelerating to zero velocity 
	easeOutQuart: t => 1 - (--t) * t * t * t,
	// acceleration until halfway, then deceleration
	easeInOutQuart: t => t < .5 ? 8 * t * t * t * t : 1 - 8 * (--t) * t * t * t,
	// accelerating from zero velocity
	easeInQuint: t => t * t * t * t * t,
	// decelerating to zero velocity
	easeOutQuint: t => 1 + (--t) * t * t * t * t,
	// acceleration until halfway, then deceleration 
	easeInOutQuint: t => t < .5 ? 16 * t * t * t * t * t : 1 + 16 * (--t) * t * t * t * t,
	easeInSine: t => 1 - Math.cos((t * Math.PI) / 2),
	easeOutSine: t => Math.sin((t * PI) / 2),
	easeInOutSine: t => -(Math.cos(Math.PI * t) - 1) / 2
}

const lerp = (a, b, t) => (1 - t) * a + t * b;
const lerpClamped = (a, b, t) => {
	if(t < 0)
		t = 0;
	else if(t > 1)
		t = 1;
	return (1 - t) * a + t * b;
};

const inverseLerp = (a, b, t) => (t - a) / (b - a);

const remap = (value, fromA, fromB, toA, toB, easing) => {
	const t = inverseLerp(fromA, fromB, value);
	if (easing === undefined)
		return lerp(toA, toB, t);
	return lerp(toA, toB, easing(t));
};

const circlePoints = (n, posX, posY, radius) => {
	let circle_ps = [];
	for (let i = 0; i < n; i++) {
		let t = TAU * i / n;
		circle_ps.push([radius * Math.cos(t) + posX, radius * Math.sin(t) + posY, 0]);
	}
	return circle_ps;
};

const addPoints = (ctx, points, close, begin) => {
	if (begin === undefined || begin)
		ctx.beginPath();

	for (let i = 0; i < points.length; i++) {
		let p = points[i];
		if (i !== 0 || begin === undefined || begin)
			ctx.lineTo(p[0], p[1]);
		else
			ctx.moveTo(p[0], p[1]);
	}
	if (close || close === undefined)
		ctx.closePath();
};

const inversedEase = ease => x => 1 - ease(1 - x);

const clearAndRedraw = (ctx, canvas, callback) => {
	ctx.clearRect(0, 0, canvas.width, canvas.height);
	if (callback != null)
		callback();
};

const toNormalisedCanvasSpace = (canvas, e) => {
	const rect = canvas.getBoundingClientRect();
	const x = e.clientX - rect.left
	const y = e.clientY - rect.top
	return [x / rect.width, y / rect.height];
};

function getCameraRay(element, e, mouse, raycaster, camera) {
	const pos = toNormalisedCanvasSpace(element, e);
	mouse.x = (pos[0] - 0.5) * 2;
	mouse.y = ((1 - pos[1]) - 0.5) * 2;
	raycaster.setFromCamera(mouse, camera);
	return new ray(raycaster.ray.origin, raycaster.ray.direction);
}

const clamp01 = v => Math.min(1, Math.max(0, v));
const clamp = (min, max, v) => Math.min(max, Math.max(min, v));

const addCssIfRequired = (path) => {
	if (document.getElementById(path) != null)
		return;
	const head = document.getElementsByTagName("head")[0];

	const cssLink = document.createElement("link");

	cssLink.href = path + "?1.0.0";
	cssLink.rel = "stylesheet";
	cssLink.id = path;

	head.appendChild(cssLink);
};

export {
	TAU,
	Rad2Deg,
	Deg2Rad,
	TouchHandler,
	Slider,
	EasingFunctions,
	lerp,
	lerpClamped,
	inverseLerp,
	remap,
	circlePoints,
	addPoints,
	inversedEase,
	clearAndRedraw,
	toNormalisedCanvasSpace,
	getCameraRay,
	clamp01,
	clamp,
	addCssIfRequired
};