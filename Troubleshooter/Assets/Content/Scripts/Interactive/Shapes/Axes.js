import {float3, quaternion} from "../spacialMaths.js";
import * as THREE from "../three.module.js";

function appendAllAxes(axisHeight, axisScale, parent, ignoreCylinder) {
	const up = getAxis(0xa9fd00, float3.up(), true, axisHeight, axisScale, ignoreCylinder);
	parent.add(up);
	const left = getAxis(0xfd003b, float3.left(), true, axisHeight, axisScale, ignoreCylinder);
	parent.add(left);
	const forward = getAxis(0x00b1fd, float3.forward(), true, axisHeight, axisScale, ignoreCylinder);
	parent.add(forward);
}

function getAxis(color, axis, capped, height, axisScale, ignoreCylinder) {
	if (height === undefined)
		height = 1;
	const root = new THREE.Object3D();
	let scaleMultiplier;
	if (axisScale === undefined)
		scaleMultiplier = height;
	else
		scaleMultiplier = height * axisScale;

	root.quaternion.copy(quaternion.lookRotation(axis.getAnotherAxis(), axis));

	const material = new THREE.MeshBasicMaterial({color: color});
	if (!ignoreCylinder) {
		const geometry = new THREE.CylinderGeometry(scaleMultiplier * 0.015, scaleMultiplier * 0.015, height, 12, 1, !capped);
		const cylinder = new THREE.Mesh(geometry, material);
		root.add(cylinder);
		cylinder.position.set(0, height / 2, 0);
	}

	{ // cube cone
		const geometry = new THREE.ConeGeometry(scaleMultiplier * .05, scaleMultiplier * .12, 32);
		const cubeCone = new THREE.Mesh(geometry, material);
		cubeCone.position.set(0, height, 0);
		root.add(cubeCone);
	}
	return root;
}

export {appendAllAxes};