import * as THREE from '../three.module.js';
import {SVGRenderer} from '../Renderers/SVGRenderer.js';
import * as VERTX from '../behaviours.js';
import {float3, quaternion, ray, sphere} from "../spacialMaths.js";

VERTX.addCssIfRequired("./Styles/quaternions.css")

var aa_div = document.getElementById('look_rotation');
var scene, renderer, camera, webGlScene, webGlRenderer, webGlCamera/*, topLeftRenderer, topLeftCamera*/;
var axisForwardLine, axisUpLine,
	circle, cube;
var sphereCircle, axisLine;
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
	
	const circle_geometry = getCircleGeometry();

	{ // Sphere outline
		const material = new THREE.LineBasicMaterial({
			color: 0xffffff,
			linewidth: 2
		});
		sphereCircle = new THREE.Line(circle_geometry, material);
		sphereCircle.rotateX(90 * VERTX.Deg2Rad);
		scene.add(sphereCircle);
	}

	axisForwardLine = getLineAndCone(directionForwardColor);

	axisUpLine = getLineAndCone(directionUpColor);

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

	{ // Direction arrow
		{ // Line
			const points = [];
			points.push(new THREE.Vector3(0, 0, 0));
			points.push(new THREE.Vector3(0, 1 - .15 / 2, 0));
			const geometry = new THREE.BufferGeometry().setFromPoints(points);
			const material = new THREE.LineBasicMaterial({
				color: directionRightColor,
				linewidth: 2
			});
			axisLine = new THREE.Line(geometry, material);
			scene.add(axisLine);
		}

		{ // Cone
			const geometry = new THREE.ConeGeometry(.05, .15, 32);
			const material = new THREE.MeshBasicMaterial({color: directionRightColor});
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

	webGlRenderer = new THREE.WebGLRenderer( { antialias: true } );
	webGlRenderer.setSize(115, 115);

	element.appendChild(webGlRenderer.domElement);

	webGlScene = new THREE.Scene();
	webGlScene.background = new THREE.Color(0x262626);

	const camSize = 1.5;
	webGlCamera = new THREE.OrthographicCamera(-camSize, camSize, camSize, -camSize, .1, 5);
	webGlCamera.position.set(0, 0, 3);
	webGlCamera.lookAt(0, 0, 0);

	{ // cube
		const geometry = new THREE.BoxGeometry( 1, 1, 1 );
		const material = new THREE.MeshNormalMaterial ( );
		cube = new THREE.Mesh( geometry, material );
		webGlScene.add( cube );

		cube.add(getAxis(0xa9fd00, float3.up()));
		cube.add(getAxis(0xfd003b, float3.left()));
		cube.add(getAxis(0x00b1fd, float3.forward()));
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

function getCircleGeometry () {
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

function getLineAndCone(color) {
	const point = new THREE.Vector3(0, 1 - .15 / 2, 0);
	// Line
	const points = [];
	points.push(new THREE.Vector3(0, 0, 0));
	points.push(point);
	const geometry = new THREE.BufferGeometry().setFromPoints(points);
	const material = new THREE.LineBasicMaterial({
		color: color,
		linewidth: 2
	});
	const line = new THREE.Line(geometry, material);
	scene.add(line);

	{ // Cone
		const geo = new THREE.ConeGeometry(.05, .15, 32);
		const mat = new THREE.MeshBasicMaterial({color: color});
		const cone = new THREE.Mesh(geo, mat);
		line.add(cone);
		cone.position.copy(point)
	}
	return line;
}

function updateAxis() {
	let axisOther = float3.right();
	if (Math.abs(float3.dot(forwardAxis, axisOther)) > 0.99)
		axisOther = float3.up();
	const axisCross = float3.normalize(float3.cross(forwardAxis, axisOther));

	const rot = quaternion.lookRotation(axisCross, forwardAxis);
	axisForwardLine.quaternion.copy(rot);

	{ // axis to
		axisOther = float3.right();
		if (Math.abs(float3.dot(upAxis, axisOther)) > 0.99)
			axisOther = float3.up();
		const axisCrossTo = float3.normalize(float3.cross(upAxis, axisOther));
		const rotTo = quaternion.lookRotation(axisCrossTo, upAxis);
		axisUpLine.quaternion.set(rotTo.x, rotTo.y, rotTo.z, rotTo.w);
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

function getAxis(color, axis) {
	{ // cube axis line
		const points = [];
		points.push(new THREE.Vector3(0, 0, 0));
		points.push(new THREE.Vector3(axis.x, axis.y, axis.z));
		const geometry = new THREE.BufferGeometry().setFromPoints(points);
		const material = new THREE.LineBasicMaterial({
			color: color,
			linewidth: 2
		});
		const cubeLine = new THREE.Line(geometry, material);

		{ // cube cone
			const geometry = new THREE.ConeGeometry(.075, .15, 32);
			const material = new THREE.MeshBasicMaterial({color: color});
			const cubeCone = new THREE.Mesh(geometry, material);
			cubeCone.position.set(axis.x, axis.y, axis.z);
			let otherAxis = float3.up();
			if(axis.x === 0 && axis.y === 1 && axis.z === 0)
				otherAxis = float3.right();
			const rot = quaternion.lookRotation(otherAxis, axis);
			cubeCone.quaternion.set(rot.x, rot.y, rot.z, rot.w);
			cubeLine.add(cubeCone);
		}
		return cubeLine;
	}
}

function updateFromToRotation() {
	updateAxis();
	renderer.render(scene, camera);
	webGlRenderer.render( webGlScene, webGlCamera);
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

function updateAxisText () {
	const fixedLength = 2;
	from_x.textContent = forwardAxis.x.toFixed(fixedLength) + 'f';
	from_y.textContent = forwardAxis.y.toFixed(fixedLength) + 'f';
	from_z.textContent = (-forwardAxis.z).toFixed(fixedLength) + 'f';
	to_x.textContent = upAxis.x.toFixed(fixedLength) + 'f';
	to_y.textContent = upAxis.y.toFixed(fixedLength) + 'f';
	to_z.textContent = (-upAxis.z).toFixed(fixedLength) + 'f';
}

var downValid;
var handleFromAxis;

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
		if(!downValid) return;
		const query = float3.normalize(r.getPoint(d));
		handleFromAxis = float3.sqrDistance(query, forwardAxis.getAbsolute('z')) < float3.sqrDistance(query, upAxis.getAbsolute('z'));
	}

	let p;
	if (d < 0) {
		const m = pos3.magnitude();
		pos3.normalize();
		const zeroCross = float3.cross(float3.forward(), pos3);
		const rotation = quaternion.axisAngleDeg(zeroCross, (m * 3) * 90);
		
		p = quaternion.mul(rotation, float3.forward());
	}else {
		p = r.getPoint(d);
	}
	if (handleFromAxis)
		forwardAxis = float3.normalize(p);
	else
		upAxis = float3.normalize(p);
	updateAxisText();
	updateFromToRotation();
}