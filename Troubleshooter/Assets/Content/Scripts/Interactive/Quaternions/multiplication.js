import * as THREE from '../three.module.js';
import * as VERTX from '../behaviours.js';
import {SimpleShader as NormalShader} from "../SimpleShader.js";
import {RotationHandle} from "../Handles/RotationHandle.js";
import {quaternion} from "../spacialMaths.js";
import {getWireCube} from "../Shapes/WireCube.js";
import {appendAllAxesUnity} from "../Shapes/Axes.js";

VERTX.addCssIfRequired("/Styles/quaternions.css")

var renderer, scene, camera;
var raycaster;
var mouse;
var cube, cubeA, cubeB;
var rotationHandleA, rotationHandleB;
var camSize;
var cubeYPos;
var secondaryCubeYPos, secondaryCubeSize, secondaryAxisSize;
var global;

var pageParameter = processPageValue(null);

var reload = (event) => {
	if(event !== undefined && event.detail !== pageParameter)
		return;

	raycaster = new THREE.Raycaster();
	mouse = new THREE.Vector2();
	camSize = 1.5;
	cubeYPos = -0.58;
	secondaryCubeYPos = 0.68;
	secondaryCubeSize = 0.25;
	secondaryAxisSize = 0.375;
	global = false;

	var element = document.createElement("div");
	element.id = "quaternion-renderer-parent";
	document.getElementById('multiplication').prepend(element);

	element.appendChild(getOverlayText('A', "text-a"));
	element.appendChild(getOverlayText('B', "text-b"));
	element.appendChild(getOverlayText('*', "text-multiply"));
	element.appendChild(getOverlayText('=', "text-equals"));

	var resetButton = document.getElementById('multiplication-reset-button');
	resetButton.addEventListener("click", () => {
		cubeA.quaternion.set(0, 0, 0, 1);
		cubeB.quaternion.set(0, 0, 0, 1);
		rotationHandleA.reset();
		rotationHandleB.reset();
		updateMultiplicationScene();
	});

	drawMultiplication(element);
	updateMultiplicationScene();

	renderer.domElement.onmousemove = e => {
		e.preventDefault();
		const r = VERTX.getCameraRay(renderer.domElement, e, mouse, raycaster, camera);
		rotationHandleA.hover(r);
		rotationHandleB.hover(r);
	};

	new VERTX.TouchHandler(renderer.domElement, begin, null, end);
}

window.addEventListener("loadedFromState", reload);
reload();

window.addEventListener('keydown', function (event) {
	switch (event.code) {
		case "KeyX":
			global = !global;
			rotationHandleA.setIsGlobal(global);
			rotationHandleB.setIsGlobal(global);
			updateMultiplicationScene();
			break
	}
});

function begin(e) {
	e.preventDefault();
	const r = VERTX.getCameraRay(renderer.domElement, e, mouse, raycaster, camera);
	let update = rotationHandleA.down(r);
	update |= rotationHandleB.down(r);
	return update;
}

function end(e) {
	rotationHandleA.up();
	rotationHandleB.up();
}

function getOverlayText(value, additiveClass) {
	const textEquals = document.createElement("div");
	textEquals.classList.add("quaternion-overlay-text");
	textEquals.classList.add(additiveClass);
	textEquals.innerHTML = value;
	return textEquals;
}

function drawMultiplication(canvas) {
	renderer = new THREE.WebGLRenderer({antialias: true});
	renderer.setSize(500, 500);

	canvas.appendChild(renderer.domElement);

	scene = new THREE.Scene();
	scene.background = new THREE.Color(0x262626);

	camera = new THREE.OrthographicCamera(-camSize, camSize, camSize, -camSize, .1, 5);
	camera.position.set(-0.75, 0.2, -3);
	camera.lookAt(0, 0, 0);

	{ // Cube		
		cube = getWireCube(0xffffff, 1, 0.0075);
		scene.add(cube);
		cube.position.set(0, cubeYPos, 0);

		const axisHeight = 0.75;
		appendAllAxesUnity(axisHeight, 1, cube, false);
	}

	const material = new THREE.RawShaderMaterial({vertexShader: NormalShader.vertexShader, fragmentShader: NormalShader.fragmentShader});
	const handleRadius = 0.3;
	const axisScale = 1.2;
	{ // A
		const geometry = new THREE.BoxGeometry(secondaryCubeSize, secondaryCubeSize, secondaryCubeSize);
		cubeA = new THREE.Mesh(geometry, material);
		scene.add(cubeA);
		cubeA.position.set(0.84, secondaryCubeYPos, 0);
		appendAllAxesUnity(secondaryAxisSize, axisScale, cubeA, false);
		rotationHandleA = new RotationHandle(cubeA, scene, renderer.domElement, handleRadius, updateMultiplicationScene, global);
	}

	{ // B
		const geometry = new THREE.BoxGeometry(secondaryCubeSize, secondaryCubeSize, secondaryCubeSize);
		cubeB = new THREE.Mesh(geometry, material);
		scene.add(cubeB);
		cubeB.position.set(-0.38, secondaryCubeYPos, 0);
		appendAllAxesUnity(secondaryAxisSize, axisScale, cubeB, false);
		rotationHandleB = new RotationHandle(cubeB, scene, renderer.domElement, handleRadius, updateMultiplicationScene, global);
	}

	renderer.render(scene, camera);
}

function updateMultiplicationScene() {
	const qA = cubeA.quaternion;
	const qB = cubeB.quaternion;
	cube.quaternion.copy(quaternion.mul(qA, qB));
	renderer.render(scene, camera);
}