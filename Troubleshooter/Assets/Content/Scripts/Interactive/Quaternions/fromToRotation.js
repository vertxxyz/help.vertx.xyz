import * as THREE from '../three.module.js';
import {SVGRenderer} from '../Renderers/SVGRenderer.js';
import * as VERTX from "../behaviours.js";
import {float3, quaternion, ray, sphere} from "../spacialMaths.js";

VERTX.addCssIfRequired("./Styles/quaternions.css")

var aa_div = document.getElementById('from_to_rotation');
var scene, renderer, camera, webGlScene, webGlRenderer, webGlCamera/*, topLeftRenderer, topLeftCamera*/;
var arc, arcCone;
var axisFromLine, axisToLine,
	circle, cube;
var sphereCircle, axisLine, cubeLine;
var angle; // Angle calculated from the fromToRotation.

var fromAxis = new float3(1, 1, 1);
fromAxis.normalize();

var toAxis = new float3(1, -0.25, 0.25);
toAxis.normalize();

var directionFromColor = 0xff5500;
var directionToColor = 0xaaff00;
var directionColor = 0xffaa00;

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

	axisFromLine = getLineAndCone(directionFromColor);

	axisToLine = getLineAndCone(directionToColor);

	{ // Rotation
		let rotationColor = 0xaa00ff;
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
				const geometry = new THREE.ConeGeometry(.02, .1, 32);
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
		cube.add(getAxis(0xfd003b, float3.right()));
		cube.add(getAxis(0x00b1fd, float3.back()));

		{ // cube line
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

function createArcGeometry() {
	if(Math.abs(float3.dot(fromAxis, toAxis)) > 0.999) {
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

		vertices.push(x, 0, z);

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
	axisFromLine.quaternion.copy(rot);

	{ // axis to
		axisOther = float3.right();
		if (Math.abs(float3.dot(toAxis, axisOther)) > 0.99)
			axisOther = float3.up();
		const axisCrossTo = float3.normalize(float3.cross(toAxis, axisOther));
		const rotTo = quaternion.lookRotation(axisCrossTo, toAxis);
		axisToLine.quaternion.copy(rotTo);
	}

	const angleAbs = Math.abs(angle);
	{ // Arc details
		if (angleAbs < 0.5) {
			arc.visible = false;
			arcCone.visible = false;
			
			const circleRot = quaternion.lookRotation(fromAxis, axisCross);
			circle.quaternion.copy(circleRot);
			axisLine.quaternion.copy(circleRot);
			cubeLine.quaternion.copy(circleRot);
		} else {
			arc.visible = true;
			axisLine.visible = true;

			const arcCross = float3.normalize(float3.cross(fromAxis, toAxis));
			const arcRot = quaternion.lookRotation(fromAxis, arcCross);

			arc.quaternion.copy(arcRot);
			circle.quaternion.copy(arcRot);
			axisLine.quaternion.copy(arcRot);
			cubeLine.quaternion.copy(arcRot);

			let arcConePos = quaternion.mul(quaternion.axisAngleDeg(arcCross, Math.sign(angle) * -2), toAxis);
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
			cubeCone.position.copy(axis);
			cubeCone.quaternion.copy(quaternion.lookRotation(axis.getAnotherAxis(), axis));
			cubeLine.add(cubeCone);
		}
		return cubeLine;
	}
}

function updateFromToRotation() {
	arc.geometry = createArcGeometry();
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
	topLeftCamera.position.copy(topLeftCameraPosition);
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
	from_x.textContent = fromAxis.x.toFixed(fixedLength) + 'f';
	from_y.textContent = fromAxis.y.toFixed(fixedLength) + 'f';
	from_z.textContent = (-fromAxis.z).toFixed(fixedLength) + 'f';
	to_x.textContent = toAxis.x.toFixed(fixedLength) + 'f';
	to_y.textContent = toAxis.y.toFixed(fixedLength) + 'f';
	to_z.textContent = (-toAxis.z).toFixed(fixedLength) + 'f';
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
		handleFromAxis = float3.sqrDistance(query, fromAxis.getAbsolute('z')) < float3.sqrDistance(query, toAxis.getAbsolute('z'));
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
		fromAxis = float3.normalize(p);
	else
		toAxis = float3.normalize(p);
	updateAxisText();
	updateFromToRotation();
}