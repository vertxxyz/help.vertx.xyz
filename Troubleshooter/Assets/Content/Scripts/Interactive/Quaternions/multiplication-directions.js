import * as THREE from '../three.module.js';
import * as VERTX from '../behaviours.js';
import {SimpleShader as NormalShader} from "../SimpleShader.js";
import {RotationHandle} from "../Handles/RotationHandle.js";
import {float3, quaternion} from "../spacialMaths.js";
import {appendAllAxesUnity} from "../Shapes/Axes.js";
import {AxisHandle} from "../Handles/AxisHandle.js";

VERTX.addCssIfRequired("/Styles/quaternions.css")

var renderer, scene, camera;
var raycaster;
var mouse;
var cubeA;
var resultHandle;
var rotationHandleA, rotationHandleB;
var camSize;
var cubeYPos;
var secondaryCubeYPos, secondaryCubeSize, secondaryAxisSize;
var global;

var axisText_x, axisText_y, axisText_z;

var usingBHandle;

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
	document.getElementById('multiplication-directions').prepend(element);

	element.appendChild(getOverlayText('A', "text-a"));
	element.appendChild(getOverlayText('B', "text-b"));
	element.appendChild(getOverlayText('*', "text-multiply"));
	element.appendChild(getOverlayText('=', "text-equals"));

	axisText_x = document.getElementById("multiply-axis_x");
	axisText_y = document.getElementById("multiply-axis_y");
	axisText_z = document.getElementById("multiply-axis_z");
	
	var resetButton = document.getElementById('multiplication-directions-reset-button');
	resetButton.addEventListener("click", () => {
		cubeA.quaternion.set(0, 0, 0, 1);
		rotationHandleA.reset();
		rotationHandleB.reset();
		rotationHandleB.setAxisText(axisText_x, axisText_y, axisText_z);
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

	new VERTX.TouchHandler(renderer.domElement, begin, move, end);
}

window.addEventListener("loadedFromState", reload);
reload();

window.addEventListener('keydown', function (event) {

	switch (event.code) {
		case "KeyX":
			global = !global;
			rotationHandleA.setIsGlobal(global);
			updateMultiplicationScene();
			break
	}
});

function begin(e) {
	e.preventDefault();
	const r = VERTX.getCameraRay(renderer.domElement, e, mouse, raycaster, camera);
	let update = rotationHandleA.down(r);
	usingBHandle = rotationHandleB.down(r);
	if(usingBHandle)
		rotationHandleB.setAxisText(axisText_x, axisText_y, axisText_z);
	return update | usingBHandle;
}

function move(e) {
	if(!usingBHandle) return;
	e.preventDefault();
	const r = VERTX.getCameraRay(renderer.domElement, e, mouse, raycaster, camera);
	rotationHandleB.move(r);
	rotationHandleB.setAxisText(axisText_x, axisText_y, axisText_z);
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
		resultHandle = new AxisHandle(new float3(0, cubeYPos, 0), 0.8, 1.5,undefined,0xffaa00, scene, camera,true, updateMultiplicationScene);
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
		rotationHandleB = new AxisHandle(new float3(-0.38, secondaryCubeYPos, 0), handleRadius, 1.5,undefined,0xffaa00, scene, camera,true, updateMultiplicationScene);
	}

	renderer.render(scene, camera);
}

function updateMultiplicationScene() {
	const qA = cubeA.quaternion;
	const v = quaternion.mul(qA, rotationHandleB.axis);
	resultHandle.setAxis(v);
	renderer.render(scene, camera);
}