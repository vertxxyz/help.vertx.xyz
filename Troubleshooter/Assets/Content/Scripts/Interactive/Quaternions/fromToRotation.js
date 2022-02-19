import * as THREE from '../three.module.js';
import {SVGRenderer} from '../Renderers/SVGRenderer.js';
import * as VERTX from "../behaviours.js";
import {float3, quaternion, ray, sphere} from "../spacialMaths.js";
import {appendAllAxesInversed} from "../Shapes/Axes.js";
import {HoverableAxis} from "../Handles/AxisHandle.js";

VERTX.addCssIfRequired("/Styles/quaternions.css")

var scene, renderer, camera, webGlScene, webGlRenderer, webGlCamera/*, topLeftRenderer, topLeftCamera*/;
var arc, arcCone;
var axisFromLine, axisToLine, axisFromCube,
	circle, cube;
var sphereCircle, axisLine, sphereHoverCircle;
var sphereCircleOriginalMaterial;
var angle; // Angle calculated from the fromToRotation.

var fromAxis;
var toAxis;
var arcRadius;

var directionFromColor;
var directionToColor;
var directionColor;

var from_x;
var from_y;
var from_z;
var to_x;
var to_y;
var to_z;

var hovering;
var downValid;
var handleFromAxis;

var pageParameter = processPageValue(null);

var reload = (event) => {
	if(event !== undefined && event.detail !== pageParameter)
		return;

	fromAxis = new float3(-1, 0, 0);
	toAxis = new float3(0, -0.5, .5);
	toAxis.normalize();
	arcRadius = 0.75;
	directionFromColor = 0xff5500;
	directionToColor = 0xaaff00;
	directionColor = 0xffaa00;

	const element = document.createElement("div");
	element.id = "quaternion-renderer-parent";
	document.getElementById('from_to_rotation').appendChild(element);

	from_x = document.getElementById("from_to-from_x");
	from_y = document.getElementById("from_to-from_y");
	from_z = document.getElementById("from_to-from_z");
	to_x = document.getElementById("from_to-to_x");
	to_y = document.getElementById("from_to-to_y");
	to_z = document.getElementById("from_to-to_z");

	hovering = null;

	drawFromToRotation(element);
	drawFromToRotationCube(element);
	// drawFromToRotationLeft(element);
	updateFromToRotation();
	updateAxisText();

	// Handle clicking on the canvas
	new VERTX.TouchHandler(renderer.domElement, startTouchEvent, moveTouchEvent);

	renderer.domElement.onmousemove = e => {
		e.preventDefault();
		const result = new float3(0);
		if (hasRayResult(e, result)) {
			const newHovering = shouldHandleFromAxis(result) ? axisFromLine : axisToLine;
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
}

window.addEventListener("loadedFromState", reload);
reload();

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

	const circle_geometry = getCircleGeometry();

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

	axisFromLine = new HoverableAxis(directionFromColor, new float3(0, 1 - .15 / 2, 0), scene);

	axisToLine = new HoverableAxis(directionToColor, new float3(0, 1 - .15 / 2, 0), scene);

	{ // Rotation
		let rotationColor = 0x666666;
		let rotationColorSecondary = 0xee88ff;
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

		{
			{ // arc
				const material = new THREE.LineBasicMaterial({
					color: rotationColorSecondary,
					linewidth: 3
				});
				arc = new THREE.Line(createArcGeometry(), material);
				//arc.scale.setScalar( 0.99 );
				scene.add(arc);
			}

			{ // arc cone
				const geometry = new THREE.ConeGeometry(.03, .1, 32);
				const material = new THREE.MeshBasicMaterial({color: rotationColorSecondary});
				arcCone = new THREE.Mesh(geometry, material);
				scene.add(arcCone);
			}
		}
	}

	{ // Direction arrow
		{ // Line
			const points = [];
			points.push(new THREE.Vector3(0, 0, 0));
			points.push(new THREE.Vector3(0, 1 - .15 / 2, 0));
			const geometry = new THREE.BufferGeometry().setFromPoints(points);
			const material = new THREE.LineBasicMaterial({
				color: directionColor,
				linewidth: 2
			});
			axisLine = new THREE.Line(geometry, material);
			scene.add(axisLine);
		}

		{ // Cone
			const geometry = new THREE.ConeGeometry(.05, .15, 32);
			const material = new THREE.MeshBasicMaterial({color: directionColor});
			const cone = new THREE.Mesh(geometry, material);
			axisLine.add(cone);
			cone.position.set(0, 1 - .15 / 2, 0);
		}
	}

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

		appendAllAxesInversed(0.8, 2, cube, true);

		/*{ // cube line
			const points = [];
			const lineLength = 1.1;
			points.push(new THREE.Vector3(0, -lineLength, 0));
			points.push(new THREE.Vector3(0, lineLength, 0));
			const geometry = new THREE.BufferGeometry().setFromPoints(points);
			const material = new THREE.LineBasicMaterial({
				color: directionColor,
				linewidth: 2
			});
			cubeLine = new THREE.Line(geometry, material);
			webGlScene.add(cubeLine);

			{ // cube cone
				const geometry = new THREE.ConeGeometry(.075, .15, 32);
				const material = new THREE.MeshBasicMaterial({color: directionColor});
				const cubeCone = new THREE.Mesh(geometry, material);
				cubeLine.add(cubeCone);
				cubeCone.position.set(0, lineLength, 0);
			}
		}*/

		{ // cube line
			const points = [];
			const lineLength = 1.1;
			points.push(new THREE.Vector3(0, 0, 0));
			points.push(new THREE.Vector3(0, lineLength, 0));
			const geometry = new THREE.BufferGeometry().setFromPoints(points);
			const material = new THREE.LineBasicMaterial({
				color: directionFromColor,
				linewidth: 2
			});
			axisFromCube = new THREE.Line(geometry, material);
			cube.add(axisFromCube);

			{ // cube cone
				const geometry = new THREE.ConeGeometry(.075, .15, 32);
				const material = new THREE.MeshBasicMaterial({color: directionFromColor});
				const cubeCone = new THREE.Mesh(geometry, material);
				axisFromCube.add(cubeCone);
				cubeCone.position.set(0, lineLength, 0);
			}
		}
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

function getCircleGeometry() {
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

function createArcGeometry() {
	if (Math.abs(float3.dot(fromAxis, toAxis)) > 0.999) {
		angle = 0;
		const geometry = new THREE.BufferGeometry();
		geometry.setAttribute('position', new THREE.Float32BufferAttribute([], 3));
		return geometry;
	}

	const rotation = quaternion.fromToRotation(fromAxis, toAxis);
	angle = rotation.angle();

	const vertices = [];
	const divisions = 50;
	const angleDivision = angle / 360.0;
	for (let i = 0; i <= divisions; i++) {

		let v = (i / divisions) * VERTX.TAU;
		v *= angleDivision;

		const x = Math.sin(v);
		const z = Math.cos(v);

		vertices.push(x * arcRadius, 0, z * arcRadius);

	}
	const geometry = new THREE.BufferGeometry();
	geometry.setAttribute('position', new THREE.Float32BufferAttribute(vertices, 3));
	return geometry;
}

function updateAxis() {
	let axisOther = float3.right();
	if (Math.abs(float3.dot(fromAxis, axisOther)) > 0.99)
		axisOther = float3.up();
	const axisCross = float3.normalize(float3.cross(fromAxis, axisOther));

	const rot = quaternion.lookRotation(axisCross, fromAxis);
	axisFromLine.root.quaternion.copy(rot);
	axisFromCube.quaternion.copy(rot);

	{ // axis to
		axisOther = float3.right();
		if (Math.abs(float3.dot(toAxis, axisOther)) > 0.99)
			axisOther = float3.up();
		const axisCrossTo = float3.normalize(float3.cross(toAxis, axisOther));
		const rotTo = quaternion.lookRotation(axisCrossTo, toAxis);
		axisToLine.root.quaternion.copy(rotTo);
	}

	const angleAbs = Math.abs(angle);
	{ // Arc details
		if (angleAbs < 0.5) {
			arc.visible = false;
			arcCone.visible = false;

			const circleRot = quaternion.lookRotation(fromAxis, axisCross);
			circle.quaternion.copy(circleRot);
			axisLine.quaternion.copy(circleRot);
			// cubeLine.quaternion.copy(circleRot);
		} else {
			arc.visible = true;
			axisLine.visible = true;

			const arcCross = float3.normalize(float3.cross(fromAxis, toAxis));
			const arcRot = quaternion.lookRotation(fromAxis, arcCross);

			arc.quaternion.copy(arcRot);
			circle.quaternion.copy(arcRot);
			axisLine.quaternion.copy(arcRot);
			// cubeLine.quaternion.copy(arcRot);

			const arcPosLocal = float3.mul(toAxis, arcRadius);
			let arcConePos = quaternion.mul(quaternion.axisAngleDeg(arcCross, Math.sign(angle) * -3), arcPosLocal);
			let arcConeRot = quaternion.mul(quaternion.axisAngleDeg(toAxis, Math.sign(angle) * -90), arcRot);
			arcCone.position.copy(arcConePos);
			arcCone.quaternion.copy(arcConeRot);
			arcCone.visible = angleAbs > 5;
		}
	}

	// Cube
	const fromToRot = angleAbs < 0.5 ? new quaternion() : quaternion.fromToRotation(fromAxis, toAxis);
	cube.quaternion.copy(fromToRot);

	sphereCircle.visible = Math.abs(float3.dot(float3.cross(fromAxis, toAxis), float3.forward())) < 0.975;
}

function updateFromToRotation() {
	arc.geometry = createArcGeometry();
	updateAxis();
	renderer.render(scene, camera);
	webGlRenderer.render(webGlScene, webGlCamera);
	// renderTopLeft();
}

function updateAxisText() {
	const fixedLength = 2;
	from_x.textContent = fromAxis.x.toFixed(fixedLength) + 'f';
	from_y.textContent = fromAxis.y.toFixed(fixedLength) + 'f';
	from_z.textContent = (-fromAxis.z).toFixed(fixedLength) + 'f';
	to_x.textContent = toAxis.x.toFixed(fixedLength) + 'f';
	to_y.textContent = toAxis.y.toFixed(fixedLength) + 'f';
	to_z.textContent = (-toAxis.z).toFixed(fixedLength) + 'f';
}

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
		const query = float3.normalize(r.getPoint(d));
		handleFromAxis = shouldHandleFromAxis(query);
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
	if (handleFromAxis)
		fromAxis = float3.normalize(p);
	else
		toAxis = float3.normalize(p);
	updateAxisText();
	updateFromToRotation();
}


function shouldHandleFromAxis(query) {
	return float3.sqrDistance(query, fromAxis.getAbsolute('z')) < float3.sqrDistance(query, toAxis.getAbsolute('z'));
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

function orientHoverCircle(result) {
	sphereHoverCircle.position.copy(result);
	sphereHoverCircle.quaternion.copy(quaternion.mul(quaternion.lookRotation(result, result.getAnotherAxis()), quaternion.axisAngleDeg(float3.right(), 90)));
}