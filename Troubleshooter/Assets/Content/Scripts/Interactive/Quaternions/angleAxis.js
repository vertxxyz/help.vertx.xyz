import * as THREE from '../three.module.js';
import {SVGRenderer} from '../Renderers/SVGRenderer.js';
import * as VERTX from "../behaviours.js";
import {float3, quaternion, ray, sphere} from "../spacialMaths.js";
import {AxisHandle} from "../Handles/AxisHandle.js";
import {appendAllAxesUnity} from "../Shapes/Axes.js";

VERTX.addCssIfRequired("/Styles/quaternions.css")

var aa_div = document.getElementById('angle_axis');
var scene, renderer, camera, webGlScene, webGlRenderer, webGlCamera;
var endLine, arc, arcCone;
var circle, startLine, cube, cubeLine;
var axisHandle;
var axis = new float3(-1, 1, -1);
var raycaster = new THREE.Raycaster();
var mouse = new THREE.Vector2();
axis.normalize();

var angle = 0;
var arcRadius = 0.75;
var directionColor = 0xffaa00;

var element = document.createElement("div");
element.id = "quaternion-renderer-parent";
aa_div.appendChild(element);

var angleText = document.getElementById("angle_axis-angle");
var axis_xText = document.getElementById("angle_axis-axis_x");
var axis_yText = document.getElementById("angle_axis-axis_y");
var axis_zText = document.getElementById("angle_axis-axis_z");

drawAngleAxis(element);
drawAngleAxisCube(element);
updateAngleAxis();
updateAxisText();

function drawAngleAxis(canvas) {
	renderer = new SVGRenderer();
	renderer.setSize(500, 500);
	canvas.appendChild(renderer.domElement);

	scene = new THREE.Scene();
	scene.background = new THREE.Color(0x262626);

	const camSize = 1.5;
	camera = new THREE.OrthographicCamera(-camSize, camSize, camSize, -camSize, .1, 5);
	camera.position.set(0, 0, -3);
	camera.lookAt(0, 0, 0);

	axisHandle = new AxisHandle(new float3(0), 1, 1, axis, directionColor, scene, camera, false, updateAngleAxis);

	{ // Rotation
		let rotationColor = 0xaa00ff;
		let circleColor = 0x666666;
		let rotationColorSecondary = 0xee88ff;
		{ // circle
			const material = new THREE.LineDashedMaterial({
				color: circleColor,
				linewidth: 2,
				dashSize: 5,
				gapSize: 6
			});
			circle = new THREE.Line(AxisHandle.getCircleGeometry(), material);
			scene.add(circle);
		}

		{// lines
			const points = [];
			points.push(new THREE.Vector3(0, 0, 0));
			points.push(new THREE.Vector3(0, 1, 0));
			const geometry = new THREE.BufferGeometry().setFromPoints(points);
			const material = new THREE.LineBasicMaterial({
				color: rotationColor,
				linewidth: 2
			});

			{ // Start Line
				startLine = new THREE.Line(geometry, material);
				scene.add(startLine);
			}

			{ // End Line
				endLine = new THREE.Line(geometry, material);
				scene.add(endLine);
			}

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

	renderer.render(scene, camera);
}

function drawAngleAxisCube(canvas) {
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
	webGlCamera.position.set(0, 0, -3);
	webGlCamera.lookAt(0, 0, 0);

	{ // cube
		const geometry = new THREE.BoxGeometry( 1, 1, 1 );
		const material = new THREE.MeshNormalMaterial ( );
		cube = new THREE.Mesh( geometry, material );
		webGlScene.add( cube );

		appendAllAxesUnity(0.6, 3, cube, true);
		
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

function createArcGeometry() {
	if(Math.abs(angle) < 0.01) {
		const geometry = new THREE.BufferGeometry();
		geometry.setAttribute('position', new THREE.Float32BufferAttribute([], 3));
		return geometry;
	}
	
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
	if (Math.abs(float3.dot(axis, axisOther)) > 0.99)
		axisOther = float3.up();
	const axisCross = float3.normalize(float3.cross(axis, axisOther));
	
	const rot = quaternion.lookRotation(axisCross, axis);
	axisHandle.setAxisQuaternion(rot);
	axisHandle.setAxisQuaternion(rot);

	let lineRot = quaternion.lookRotation(axis, axisCross);
	startLine.quaternion.copy(lineRot);

	// Cube
	const angleRotation = quaternion.axisAngleDeg(axis, angle);
	angleRotation.normalize();
	cube.quaternion.copy(angleRotation);
	cubeLine.quaternion.copy(rot);

	{ // Arc details
		arc.quaternion.copy(rot);
		circle.quaternion.copy(rot);
		
		let lineRot = quaternion.lookRotation(axis, axisCross);
		lineRot = quaternion.mul(angleRotation, lineRot);
		endLine.quaternion.copy(lineRot);

		let arcConePos = quaternion.mul(lineRot, new float3(0, arcRadius, 0));

		lineRot = quaternion.mul(quaternion.axisAngleDeg(axis, Math.sign(angle) * 90), lineRot);
		arcConePos = float3.plus(arcConePos, float3.mul(quaternion.mul(lineRot, float3.up()), -0.05));
		arcCone.position.copy(arcConePos);
		arcCone.quaternion.copy(lineRot);
		arcCone.visible = Math.abs(angle) > 5;
	}
}

function updateAngleAxis() {
	arc.geometry = createArcGeometry();
	updateAxis();
	renderer.render( scene, camera );
	webGlRenderer.render( webGlScene, webGlCamera);
}

function updateAxisText () {
	const fixedLength = 2;
	angleText.textContent = angle.toFixed(fixedLength) + 'f';
	axis_xText.textContent = (-axis.x).toFixed(fixedLength) + 'f';
	axis_yText.textContent = axis.y.toFixed(fixedLength) + 'f';
	axis_zText.textContent = axis.z.toFixed(fixedLength) + 'f';
}


// Handle changing distance via slider
new VERTX.Slider(document.getElementById("angle_axis_slider"), function (x) {
	angle = VERTX.remap(x, 0, 1, -180, 180);
	updateAngleAxis();
	updateAxisText();
}, undefined, 0.5);

var downValid;

// Handle clicking on the canvas
new VERTX.TouchHandler(renderer.domElement, startTouchEvent, moveTouchEvent);

function startTouchEvent(e) {
	touchEvent(e, false);
}

function moveTouchEvent(e) {
	if(!downValid)
		return;
	touchEvent(e, true);
}

function touchEvent(e, isMove) {
	e.preventDefault();
	const pos = VERTX.toNormalisedCanvasSpace(renderer.domElement, e);
	const pos3 = new float3((1 - pos[0]) - 0.5, (1 - pos[1]) - 0.5, 0);
	const s = new sphere(new float3(), 1 / 3.0);
	const r = new ray(new float3(pos3.x, pos3.y, -3), float3.forward());
	const d = s.raycast(r);

	if (!isMove) downValid = d > 0;
	if (!downValid) return;

	let p;
	if (d < 0) {
		const m = pos3.magnitude();
		pos3.normalize();
		const zeroCross = float3.cross(float3.back(), pos3);
		const rotation = quaternion.axisAngleDeg(zeroCross, (m * 3) * 90);

		p = quaternion.mul(rotation, float3.back());
	}else {
		p = r.getPoint(d);
	}
	axis = float3.normalize(p);
	updateAxisText();

	updateAngleAxis();
}

renderer.domElement.onmousemove = e => {
	e.preventDefault();
	const r = VERTX.getCameraRay(renderer.domElement, e, mouse, raycaster, camera);
	axisHandle.hover(r);
};