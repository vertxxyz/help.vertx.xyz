import * as THREE from "../three.module.js";
import {TAU} from "../behaviours.js";
import {quaternion} from "../spacialMaths.js";

class Debugging {
	static line;
	
	static getDebugLine(position, direction, scene) {
		if(Debugging.line == null) {
			const parent = new THREE.Object3D();
			const material = new THREE.MeshBasicMaterial();
			{
				const geometry = new THREE.CylinderGeometry(.005, .005, 1, 12, 1, false);
				const mesh = new THREE.Mesh(geometry, material);
				parent.add(mesh);
				mesh.rotateX(TAU * 0.25);
				mesh.translateY(0.5);
			}
			{
				const geometry = new THREE.ConeGeometry(.02, .08, 32);
				const mesh = new THREE.Mesh(geometry, material);
				parent.add(mesh);
				mesh.rotateX(TAU * 0.25);
				mesh.translateY(1);
			}
			parent.position.copy(position);
			parent.quaternion.copy(quaternion.lookRotation(direction, direction.getAnotherAxis()));
			scene.add(parent);
			Debugging.line = parent;
			return parent;
		}
		Debugging.line.position.copy(position);
		Debugging.line.quaternion.copy(quaternion.lookRotation(direction, direction.getAnotherAxis()));
		return Debugging.line;
	}
}

export {Debugging};