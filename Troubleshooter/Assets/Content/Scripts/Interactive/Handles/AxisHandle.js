import * as THREE from "../three.module.js";
import * as VERTX from "../behaviours.js";
import {float3, plane, quaternion, sphere} from "../spacialMaths.js";

class AxisHandle {
	static circle_geometry;

	static getCircleGeometry() {
		if (AxisHandle.circle_geometry == null) {
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
			AxisHandle.circle_geometry = circle_geometry;
		}
		return AxisHandle.circle_geometry;
	}

	constructor(position, radius, lineScalar, defaultDirection, directionColor, scene, camera, uses3D, repaintCallback) {
		this.defaultDirection = defaultDirection === undefined ? float3.up() : defaultDirection;
		this.axis = this.defaultDirection;
		this.radius = radius;
		this.camera = camera;
		this.commonVector = new THREE.Vector3();
		this.commonQuaterion = new THREE.Quaternion();
		this.repaintCallback = repaintCallback;
		const use3D = uses3D === undefined ? false : uses3D;
		this.parent = new THREE.Object3D();
		scene.add(this.parent);
		this.parent.position.copy(position);

		{ // Sphere outline
			if (!use3D) {
				const material = new THREE.LineBasicMaterial({
					color: 0xffffff,
					linewidth: 2
				});
				const sphereCircle = new THREE.Line(AxisHandle.getCircleGeometry(), material);
				sphereCircle.rotateX(90 * VERTX.Deg2Rad);
				sphereCircle.scale.set(radius, radius, radius);
				this.parent.add(sphereCircle);
				this.sphere = sphereCircle;
			} else {
				const material = new THREE.MeshBasicMaterial({
					color: 0xffffff
				});
				const geometry = new THREE.TorusGeometry(radius, 0.005 * lineScalar, 8, 100);
				const sphereCircle = new THREE.Mesh(geometry, material);
				camera.getWorldQuaternion(this.commonQuaterion);
				sphereCircle.quaternion.copy(this.commonQuaterion);
				this.parent.add(sphereCircle);
				this.sphere = sphereCircle;
			}
		}

		{ // Direction arrow
			{ // Line
				if (!use3D) {
					const points = [];
					points.push(new THREE.Vector3(0, 0, 0));
					points.push(new THREE.Vector3(0, (1 - .15 / 2) * radius, 0));
					const geometry = new THREE.BufferGeometry().setFromPoints(points);
					const material = new THREE.LineBasicMaterial({
						color: directionColor,
						linewidth: 2
					});
					const axisLine = new THREE.Line(geometry, material);
					this.axisLine = axisLine;
					this.parent.add(axisLine);
				} else {
					this.axisLine = new THREE.Object3D();
					this.parent.add(this.axisLine);
					{
						const geometry = new THREE.CylinderBufferGeometry(0.005 * lineScalar, 0.005 * lineScalar, radius);
						const material = new THREE.MeshBasicMaterial({
							color: directionColor,
						});
						const axisLine = new THREE.Mesh(geometry, material);
						this.axisLine.add(axisLine);
						axisLine.translateY(radius * 0.5);
					}

					{
						const material = new THREE.LineDashedMaterial({
							color: 0x666666,
							linewidth: 1,
							scale: 1,
							dashSize: 0.08,
							gapSize: 0.04,
						});
						const sphereCircle = new THREE.Line(AxisHandle.getCircleGeometry(), material);
						sphereCircle.computeLineDistances();
						sphereCircle.scale.set(radius, radius, radius);
						this.axisLine.add(sphereCircle);
					}
				}
			}

			{ // Cone
				const geometry = new THREE.ConeGeometry(.05 * lineScalar * radius, .15 * lineScalar * radius, 32);
				const material = new THREE.MeshBasicMaterial({color: directionColor});
				const cone = new THREE.Mesh(geometry, material);
				this.axisLine.add(cone);
				cone.position.set(0, radius - .15 * lineScalar * radius * 0.5, 0);
			}
		}
	}

	setAxis(axis) {
		let axisOther = float3.right();
		if (Math.abs(float3.dot(axis, axisOther)) > 0.99)
			axisOther = float3.up();
		const axisCross = float3.normalize(float3.cross(axis, axisOther));

		const rot = quaternion.lookRotation(axisCross, axis);
		this.setAxisQuaternion(rot);
	}

	setAxisQuaternion(q) {
		this.axisLine.quaternion.copy(q);
		this.axis = quaternion.mul(q, float3.up());
		this.camera.getWorldDirection(this.commonVector);
		this.sphere.visible = Math.abs(float3.dot(this.axis, this.commonVector)) < 0.975;
	}

	down(r) {
		if (!this.raycast(r))
			return false;
		this.setAxis(this.lastLocalSphereRaycast);
		this.repaintCallback();
		return true;
	}

	move(r) {
		if (this.raycast(r)) {
			this.setAxis(this.lastLocalSphereRaycast);
		} else {
			const pl = new plane();
			this.camera.getWorldDirection(this.commonVector);
			const n = this.commonVector.clone();
			this.parent.getWorldPosition(this.commonVector);
			const p = this.commonVector.clone();
			pl.setFromPoint(n, p);
			const d = pl.raycast(r);
			const pos3 = float3.sub(r.getPoint(d), p);
			const m = pos3.magnitude();
			pos3.normalize();
			const zeroCross = float3.cross(n, pos3);
			const rotation = quaternion.axisAngleDeg(zeroCross, 180 + (-m * 3) * 90);

			this.setAxis(float3.normalize(quaternion.mul(rotation, n)));
		}

		this.repaintCallback();
	}

	reset() {
		this.setAxis(this.defaultDirection);
	}

	raycast(ray) {
		this.parent.updateWorldMatrix(true, false);
		this.commonVector.set(0, 0, 0);
		// World space obj position
		const bp = this.parent.localToWorld(this.commonVector);
		const sA = new sphere(bp, this.radius);
		const d = sA.raycast(ray);
		if (d < 0)
			return false;
		const p = ray.getPoint(d);
		this.commonVector.copy(p);
		float3.copy(this.parent.worldToLocal(this.commonVector));
		this.lastLocalSphereRaycast = float3.normalize(this.commonVector);
		return true;
	}
}

export {AxisHandle};