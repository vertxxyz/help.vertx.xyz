import * as THREE from '../three.module.js';
import {float3, plane, quaternion, sphere} from "../spacialMaths.js";

class RotationHandle {
	static nameToColor = {
		"axis_xz": new THREE.Color(0xa9fd00),
		"axis_yz": new THREE.Color(0xfd003b),
		"axis_xy": new THREE.Color(0x00b1fd)
	}
	static downColor = new THREE.Color(0xffff00);

	constructor(virtualParent, scene, element, radius, repaintCallback, globalSpace) {
		this.obj = new THREE.Object3D();
		this.commonVector = new THREE.Vector3();
		this.commonQuaterion = new THREE.Quaternion();
		this.parent = virtualParent;
		this.scene = scene;
		scene.add(this.obj);
		this.element = element;
		this.reposition();
		this.radius = radius;
		this.repaintCallback = repaintCallback;
		this.currentHover = null;
		this.currentDown = null;
		this.appendAllRotationAxes(this.obj, radius)
		this.isGlobal = globalSpace === undefined ? false : globalSpace;
		this.moveFunc = (this.move).bind(this);
		document.addEventListener('pointerlockchange', () => RotationHandle.lockChangeAlert(this), false);
		document.addEventListener('mozpointerlockchange', () => RotationHandle.lockChangeAlert(this), false);
	}

	reset() {
		this.reposition();
	}

	setIsGlobal(isGlobal) {
		this.isGlobal = isGlobal;
		this.reset();
	}

	static lockChangeAlert(h) {
		if (document.pointerLockElement != null || document.mozPointerLockElement != null)
			return;
		if (h.currentDown != null)
			h.up();
	}

	reposition() {
		this.parent.updateWorldMatrix(true, false);
		if (this.isGlobal) {
			this.obj.quaternion.set(0, 0, 0, 1);
		} else {
			this.parent.getWorldQuaternion(this.commonQuaterion);
			this.obj.quaternion.copy(this.commonQuaterion);
		}
		this.commonVector.set(0, 0, 0);
		this.obj.position.copy(this.parent.localToWorld(this.commonVector));
	}

	appendAllRotationAxes(parent, radius) {
		const geometry = new THREE.TorusGeometry(radius, radius * 0.03, 16, 50);
		const geometryThick = new THREE.TorusGeometry(radius, radius * 0.06, 16, 50);
		const xz = this.getRotationAxis(float3.up(), geometry, geometryThick, "axis_xz");
		parent.add(xz);
		const yz = this.getRotationAxis(float3.right(), geometry, geometryThick, "axis_yz");
		parent.add(yz);
		const xy = this.getRotationAxis(float3.forward(), geometry, geometryThick, "axis_xy");
		parent.add(xy);
	}

	getRotationAxis(axis, geometry, geometryThick, name) {
		const color = RotationHandle.nameToColor[name];
		const material = new THREE.MeshBasicMaterial({color: color});
		const torus = new THREE.Mesh(geometry, material);
		torus.name = name;
		let otherAxis = axis.getAnotherAxis();
		const rot = quaternion.lookRotation(axis, otherAxis);
		torus.quaternion.copy(rot);

		const torusThick = new THREE.Mesh(geometryThick, material);
		torus.add(torusThick);
		torusThick.position.set(0, 0, 0);
		torusThick.quaternion.set(0, 0, 0, 1);
		torusThick.visible = false;

		return torus;
	}

	hover(r) {
		if (this.currentDown != null) {
			this.performHoverReset();
		} else {
			let hoverResult = this.raycast(r);
			if (this.performHover(hoverResult))
				this.repaintCallback();
		}
	}

	down(r) {
		let hoverResult = this.raycast(r);
		if (this.performColor(hoverResult)) {
			this.performHoverReset();

			const q = this.obj.getWorldQuaternion(this.commonQuaterion).clone();
			
			// local space axis
			const localDir = quaternion.mul(hoverResult.quaternion, float3.forward());
			// world space axis
			const dir = quaternion.mul(q, localDir);
						
			const alignment = float3.cross(quaternion.mul(q, this.lastLocalSphereRaycast), dir);
			this.mouseAxis = quaternion.mul(quaternion.inverse(quaternion.lookRotation(r.normal)), float3.normalize(alignment.projectOnPlane(r.normal)));
			this.rotAxis = localDir;
			this.element.requestPointerLock();
			this.element.addEventListener('mousemove', this.moveFunc, false);
			this.element.addEventListener('touchmove', this.moveFunc, false);
			this.repaintCallback();
		}
	}

	move(e) {
		const movement = new float3(e.movementX, e.movementY);
		const p = new plane();
		p.setFromDistance(this.mouseAxis, 0);
		const d = p.getDistanceToPoint(movement);
		if (Math.abs(d) < 0.001)
			return;
		const angle = d * 0.01;
		this.obj.rotateOnAxis(this.rotAxis, angle);
		if (this.isGlobal) {
			this.parent.rotateOnWorldAxis(this.rotAxis, angle);
		} else {
			this.parent.rotateOnAxis(this.rotAxis, angle);
		}
		this.repaintCallback();
	}

	up() {
		document.exitPointerLock();
		this.element.removeEventListener('mousemove', this.moveFunc, false);
		this.element.removeEventListener('touchmove', this.moveFunc, false);
		let update = false;
		if (this.isGlobal) {
			const o = this.obj.quaternion;
			if (o.x !== 0 || o.y !== 0 || o.z !== 0 || o.z !== 1) {
				this.obj.quaternion.set(0, 0, 0, 1);
				update = true;
			}
		}
		update |= this.performColorReset();
		if (update)
			this.repaintCallback();
	}

	raycast(ray) {
		this.obj.updateWorldMatrix(true, false);
		this.commonVector.set(0, 0, 0);
		// World space obj position
		const bp = this.obj.localToWorld(this.commonVector);
		const sA = new sphere(bp, this.radius);
		const d = sA.raycast(ray);
		const p = ray.getPoint(d);
		this.commonVector.copy(p);
		let a = float3.copy(this.obj.worldToLocal(this.commonVector));
		const c = float3.copy(a);
		a.abs();
		let closestAxis;
		if (a.x < a.y) {
			if (a.x < a.z) {
				// X Smallest
				closestAxis = "axis_yz";
				this.lastLocalSphereRaycast = float3.normalize(float3.mul(c, new float3(0, 1, 1)));
			} else {
				// Z Smallest
				closestAxis = "axis_xy";
				this.lastLocalSphereRaycast = float3.normalize(float3.mul(c, new float3(1, 1, 0)));
			}
		} else if (a.y < a.z) {
			// Y Smallest
			closestAxis = "axis_xz";
			this.lastLocalSphereRaycast = float3.normalize(float3.mul(c, new float3(1, 0, 1)));
		} else {
			// Z Smallest
			closestAxis = "axis_xy";
			this.lastLocalSphereRaycast = float3.normalize(float3.mul(c, new float3(1, 1, 0)));
		}
		
		if (d >= 0) {
			let result = null;
			this.obj.traverse(function (child) {
				if (child.name === closestAxis) {
					result = child;
				}
			});
			return result;
		}
		return null;
	}

	performHover(result) {
		const changed = this.performHoverReset();
		if (result == null)
			return changed;
		this.currentHover = result.children[0];
		this.currentHover.visible = true;
		return true;
	}

	performHoverReset() {
		if (this.currentHover == null)
			return false;
		this.currentHover.visible = false;
		this.currentHover = null;
		return true;
	}

	performColor(result) {
		const changed = this.performColorReset();
		if (result == null)
			return changed;
		this.currentDown = result;
		this.currentDown.material.color = RotationHandle.downColor;
		return true;
	}

	performColorReset() {
		if (this.currentDown == null)
			return false;
		this.currentDown.material.color = RotationHandle.nameToColor[this.currentDown.name];
		this.currentDown = null;
		return true;
	}
}

export {RotationHandle};