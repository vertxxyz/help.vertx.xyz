import * as THREE from '../three.module.js';
import {SVGRenderer} from '../Renderers/SVGRenderer.js';
import * as VERTX from '../behaviours.js';
import {float3, quaternion, ray, sphere} from "../spacialMaths.js";
import {HoverableAxis, PlainAxis} from "../Handles/AxisHandle.js";

VERTX.addCssIfRequired("./Styles/quaternions.css")

var aa_div = document.getElementById('look_rotation');
var scene, renderer, camera, webGlScene, webGlRenderer, webGlCamera/*, topLeftRenderer, topLeftCamera*/;
var axisForwardLine, axisUpLine,
	circle, cube;
var sphereCircle, axisLine, sphereHoverCircle;
var sphereCircleOriginalMaterial;
var angle; // Angle calculated from the fromToRotation.

var forwardAxis = new float3(1, 1, 1);
forwardAxis.normalize();

var upAxis = new float3(0, 1, 0);
upAxis.normalize();

var directionForwardColor = 0x00b1fd;
var directionUpColor = 0xa9fd00;
var directionRightColor = 0xfd003b;

var element = document.createElement("div");
element.id = "quaternion-renderer-parent";
aa_div.appendChild(element);

var from_x = document.getElementById("from_to-from_x");
var from_y = document.getElementById("from_to-from_y");
var from_z = document.getElementById("from_to-from_z");
var to_x = document.getElementById("from_to-to_x");
var to_y = document.getElementById("from_to-to_y");
var to_z = document.getElementById("from_to-to_z");

drawFromToRotation(element);
drawFromToRotationCube(element);
// drawFromToRotationLeft(element);
updateFromToRotation();
updateAxisText();

function drawFromToRotation(canvas) {
	renderer = new SVGRenderer();
	renderer.setSize(500, 500);
	canvas.appendChild(renderer.domElement);

	scene = new THREE.Scene();
	scene.background = new THREE.Color(0x262626);

	const camSize = 1.5;
	camera = new THREE.OrthographicCamera(-camSize, camSize, camSize, -camSize, .1, 5);
	camera.position.set(0, 0, 3);
	camera.lookAt(0, 0, 0);

	const circle_geometry = getCircleGeometryXZ();

	{ // Sphere outline
		const material = new THREE.LineBasicMaterial({
			color: 0xffffff,
			linewidth: 2
		});
		sphereCircleOriginalMaterial = material;
		sphereCircle = new THREE.Line(circle_geometry, material);
		sphereCircle.rotateX(90 * VERTX.Deg2Rad);
		scene.add(sphereCircle);
	}

	{ // Sphere hover
		const material = new THREE.LineBasicMaterial({
			color: 0xffffff,
			linewidth: 1
		});
		sphereHoverCircle = new THREE.Line(circle_geometry, material);
		sphereHoverCircle.scale.setScalar(0.05);
		scene.add(sphereHoverCircle);
		sphereHoverCircle.visible = false;
	}

	axisForwardLine = new HoverableAxis(directionForwardColor, new float3(0, 1 - .15 / 2, 0), scene);

	axisUpLine = new HoverableAxis(directionUpColor, new float3(0, 1 - .15 / 2, 0), scene);

	{ // Rotation
		let rotationColor = 0x666666;
		{ // circle
			const material = new THREE.LineDashedMaterial({
				color: rotationColor,
				linewidth: 2,
				dashSize: 5,
				gapSize: 6
			});
			circle = new THREE.Line(circle_geometry, material);
			scene.add(circle);
		}
	}

	axisLine = new HoverableAxis(directionRightColor, new float3(0, 1 - .15 / 2, 0), scene).root;

	renderer.render(scene, camera);
}

function drawFromToRotationCube(canvas) {
	const element = document.createElement("div");
	element.id = "quaternion-webgl-renderer-parent-right";
	canvas.appendChild(element);

	webGlRenderer = new THREE.WebGLRenderer({antialias: true});
	webGlRenderer.setSize(115, 115);

	element.appendChild(webGlRenderer.domElement);

	webGlScene = new THREE.Scene();
	webGlScene.background = new THREE.Color(0x262626);

	const camSize = 1.5;
	webGlCamera = new THREE.OrthographicCamera(-camSize, camSize, camSize, -camSize, .1, 5);
	webGlCamera.position.set(0, 0, 3);
	webGlCamera.lookAt(0, 0, 0);

	{ // cube
		const geometry = new THREE.BoxGeometry(1, 1, 1);
		const material = new THREE.MeshNormalMaterial();
		cube = new THREE.Mesh(geometry, material);
		webGlScene.add(cube);

		new PlainAxis(0xa9fd00, float3.up(), cube);
		new PlainAxis(0xfd003b, float3.left(), cube);
		new PlainAxis(0x00b1fd, float3.forward(), cube);
	}

	webGlRenderer.render(webGlScene, webGlCamera);
}

/*function drawFromToRotationLeft(canvas) {
	const element = document.createElement("div");
	element.id = "quaternion-webgl-renderer-parent-left";
	canvas.appendChild(element);

	topLeftRenderer = new THREE.WebGLRenderer( { antialias: true } );
	topLeftRenderer.setSize(115, 115);

	const camSize = 1.5;
	topLeftCamera = new THREE.OrthographicCamera(-camSize, camSize, camSize, -camSize, .1, 5);
	element.appendChild(topLeftRenderer.domElement);
}*/

function getCircleGeometryXZ() {
	const vertices = [];
	const divisions = 100;
	for (let i = 0; i <= divisions; i++) {

		const v = (i / divisions) * VERTX.TAU;

		const x = Math.sin(v);
		const z = Math.cos(v);

		vertices.push(x, 0, z);

	}
	const circle_geometry = new THREE.BufferGeometry();
	circle_geometry.setAttribute('position', new THREE.Float32BufferAttribute(vertices, 3));
	return circle_geometry;
}

function updateAxis() {
	let axisOther = float3.right();
	if (Math.abs(float3.dot(forwardAxis, axisOther)) > 0.99)
		axisOther = float3.up();
	const axisCross = float3.normalize(float3.cross(forwardAxis, axisOther));

	const rot = quaternion.lookRotation(axisCross, forwardAxis);
	axisForwardLine.root.quaternion.copy(rot);

	{ // axis to
		axisOther = float3.right();
		if (Math.abs(float3.dot(upAxis, axisOther)) > 0.99)
			axisOther = float3.up();
		const axisCrossTo = float3.normalize(float3.cross(upAxis, axisOther));
		const rotTo = quaternion.lookRotation(axisCrossTo, upAxis);
		axisUpLine.root.quaternion.set(rotTo.x, rotTo.y, rotTo.z, rotTo.w);
	}

	const angleAbs = Math.abs(angle);

	{ // Arc details
		if (angleAbs < 0.5) {
			const circleRot = quaternion.lookRotation(forwardAxis, axisCross);
			circle.quaternion.copy(circleRot);
			axisLine.quaternion.copy(circleRot);
		} else {
			axisLine.visible = true;

			const arcCross = float3.normalize(float3.cross(forwardAxis, upAxis));
			const arcRot = quaternion.lookRotation(forwardAxis, arcCross);
			circle.quaternion.copy(arcRot);
			axisLine.quaternion.copy(arcRot);
		}
	}

	// Cube
	const fromToRot = angleAbs < 0.5 ? new quaternion() : quaternion.lookRotation(forwardAxis, upAxis);
	cube.quaternion.copy(fromToRot);

	sphereCircle.visible = Math.abs(float3.dot(float3.cross(forwardAxis, upAxis), float3.forward())) < 0.975;
}

function updateFromToRotation() {
	updateAxis();
	renderer.render(scene, camera);
	webGlRenderer.render(webGlScene, webGlCamera);
	// renderTopLeft();
}

/*function renderTopLeft() {
	const backAxis = new float3(fromAxis.x, 0, fromAxis.z);
	backAxis.normalize();
	const topLeftCameraRotation = quaternion.lookRotation(backAxis, float3.up());
	const topLeftCameraPosition = float3.mul(quaternion.mul(topLeftCameraRotation, float3.forward()), 3);
	topLeftCamera.position.set(topLeftCameraPosition.x, topLeftCameraPosition.y, topLeftCameraPosition.z);
	topLeftCamera.lookAt(0, 0, 0);
	sphereCircle.visible = false;
	axisFromLine.visible = false;
	axisToLine.visible = false;
	topLeftRenderer.render( scene, topLeftCamera);
	axisFromLine.visible = true;
	axisToLine.visible = true;
	sphereCircle.visible = true;
}*/

function updateAxisText() {
	const fixedLength = 2;
	from_x.textContent = forwardAxis.x.toFixed(fixedLength) + 'f';
	from_y.textContent = forwardAxis.y.toFixed(fixedLength) + 'f';
	from_z.textContent = (-forwardAxis.z).toFixed(fixedLength) + 'f';
	to_x.textContent = upAxis.x.toFixed(fixedLength) + 'f';
	to_y.textContent = upAxis.y.toFixed(fixedLength) + 'f';
	to_z.textContent = (-upAxis.z).toFixed(fixedLength) + 'f';
}

var downValid;
var handleForwardAxis;

// Handle clicking on the canvas
new VERTX.TouchHandler(renderer.domElement, startTouchEvent, moveTouchEvent);

function startTouchEvent(e) {
	touchEvent(e, false);
}

function moveTouchEvent(e) {
	if (!downValid)
		return;
	touchEvent(e, true);
}

function touchEvent(e, isMove) {
	e.preventDefault();
	const pos = VERTX.toNormalisedCanvasSpace(renderer.domElement, e);
	const pos3 = new float3(pos[0] - 0.5, (1 - pos[1]) - 0.5, 0);
	const s = new sphere(new float3(), 1 / 3.0);
	const r = new ray(new float3(pos3.x, pos3.y, 3), float3.back());
	const d = s.raycast(r);

	if (!isMove) {
		downValid = d >= 0;
		if (!downValid) return;
		handleForwardAxis = shouldHandleForwardAxis(float3.normalize(r.getPoint(d)));
	}

	let p;
	if (d < 0) {
		const m = pos3.magnitude();
		pos3.normalize();
		const zeroCross = float3.cross(float3.forward(), pos3);
		const rotation = quaternion.axisAngleDeg(zeroCross, (m * 3) * 90);

		p = quaternion.mul(rotation, float3.forward());
	} else {
		p = r.getPoint(d);
	}
	if (handleForwardAxis)
		forwardAxis = float3.normalize(p);
	else
		upAxis = float3.normalize(p);
	updateAxisText();
	updateFromToRotation();
}

function shouldHandleForwardAxis(query) {
	return float3.sqrDistance(query, forwardAxis.getAbsolute('z')) < float3.sqrDistance(query, upAxis.getAbsolute('z'));
}

function hasRayResult(e, resultOut) {
	const pos = VERTX.toNormalisedCanvasSpace(renderer.domElement, e);
	const pos3 = new float3(pos[0] - 0.5, (1 - pos[1]) - 0.5, 0);
	const s = new sphere(new float3(), 1 / 3.0);
	const r = new ray(new float3(pos3.x, pos3.y, 3), float3.back());
	const d = s.raycast(r);
	if (d < 0)
		return false;
	resultOut.copy(float3.normalize(r.getPoint(d)));
	return true;
}

var hovering = null;

renderer.domElement.onmousemove = e => {
	e.preventDefault();
	const result = new float3(0);
	if (hasRayResult(e, result)) {
		const newHovering = shouldHandleForwardAxis(result) ? axisForwardLine : axisUpLine;
		if (newHovering === hovering) {
			orientHoverCircle(result);
			updateFromToRotation();
			return;
		}
		if (hovering != null)
			hovering.hideHover();
		hovering = newHovering;
		hovering.showHover();
		sphereCircle.material = new THREE.LineBasicMaterial({
			color: 0xffffff,
			linewidth: 3
		});
		sphereHoverCircle.visible = true;
		orientHoverCircle(result);
		updateFromToRotation();
	} else {
		if (hovering != null) {
			hovering.hideHover();
			hovering = null;
			sphereCircle.material = sphereCircleOriginalMaterial;
			sphereHoverCircle.visible = false;
			updateFromToRotation();
		}
	}
};

function orientHoverCircle(result) {
	sphereHoverCircle.position.copy(result);
	sphereHoverCircle.quaternion.copy(quaternion.mul(quaternion.lookRotation(result, result.getAnotherAxis()), quaternion.axisAngleDeg(float3.right(), 90)));
}