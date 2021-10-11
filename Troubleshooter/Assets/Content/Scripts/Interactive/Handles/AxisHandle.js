import * as THREE from "../three.module.js";
import * as VERTX from "../behaviours.js";
import {float3, plane, quaternion, sphere} from "../spacialMaths.js";
import {TAU} from "../behaviours.js";

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
		this.currentHover = null;
		this.currentlyDown = false;
		this.repaintCallback = repaintCallback;
		const use3D = uses3D === undefined ? false : uses3D;
		this.root = new THREE.Object3D();
		scene.add(this.root);
		this.root.position.copy(position);

		{ // Sphere outline
			if (!use3D) {
				const material = new THREE.LineBasicMaterial({
					color: 0xffffff,
					linewidth: 2
				});
				const sphereCircle = new THREE.Line(AxisHandle.getCircleGeometry(), material);
				sphereCircle.rotateX(90 * VERTX.Deg2Rad);
				sphereCircle.scale.set(radius, radius, radius);
				this.root.add(sphereCircle);
				this.sphere = sphereCircle;

				{
					const lineMaterial = new THREE.LineBasicMaterial({
						color: 0xffffff,
						linewidth: 4
					});
					const hoverCircle = new THREE.Line(AxisHandle.getCircleGeometry(), lineMaterial);
					camera.getWorldQuaternion(this.commonQuaterion);
					hoverCircle.quaternion.copy(this.commonQuaterion);
					sphereCircle.add(hoverCircle);
					hoverCircle.visible = false;
				}
			} else {
				const material = new THREE.MeshBasicMaterial({
					color: 0xffffff
				});
				const geometry = new THREE.TorusGeometry(radius, 0.005 * lineScalar, 8, 100);
				const sphereCircle = new THREE.Mesh(geometry, material);
				camera.getWorldQuaternion(this.commonQuaterion);
				sphereCircle.quaternion.copy(this.commonQuaterion);
				this.root.add(sphereCircle);
				this.sphere = sphereCircle;

				{
					const geometry = new THREE.TorusGeometry(radius, 0.01 * lineScalar, 8, 100);
					const hoverCircle = new THREE.Mesh(geometry, material);
					camera.getWorldQuaternion(this.commonQuaterion);
					hoverCircle.quaternion.copy(this.commonQuaterion);
					sphereCircle.add(hoverCircle);
					hoverCircle.visible = false;
				}
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
					this.root.add(axisLine);
				} else {
					this.axisLine = new THREE.Object3D();
					this.root.add(this.axisLine);
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
						{ // dashed circle
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

						/*{ // multiply circle
							const material = new THREE.MeshBasicMaterial( { color: 0x555555, transroot: true, blending: THREE.MultiplyBlending } )
							const geometry = new THREE.CircleGeometry(radius, 100);
							const mesh = new THREE.Mesh(geometry, material);
							this.axisLine.add(mesh);
							mesh.rotateX(TAU * 0.25);
						}*/
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
		this.currentlyDown = true;
		this.setAxis(this.lastLocalSphereRaycast);
		this.performHoverReset();
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
			this.root.getWorldPosition(this.commonVector);
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

	up() {
		this.currentlyDown = false;
	}

	hover(r) {
		if (this.currentlyDown || !this.raycast(r)) {
			if (this.performHoverReset())
				this.repaintCallback();
		} else {
			if (this.performHover())
				this.repaintCallback();
		}
	}

	performHover() {
		const changed = this.performHoverReset();
		if (this.sphere == null)
			return changed;
		this.currentHover = this.sphere.children[0];
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

	reset() {
		this.setAxis(this.defaultDirection);
	}

	raycast(ray) {
		this.root.updateWorldMatrix(true, false);
		this.commonVector.set(0, 0, 0);
		// World space obj position
		const bp = this.root.localToWorld(this.commonVector);
		const sA = new sphere(bp, this.radius);
		const d = sA.raycast(ray);
		if (d < 0)
			return false;
		const p = ray.getPoint(d);
		this.commonVector.copy(p);
		float3.copy(this.root.worldToLocal(this.commonVector));
		this.lastLocalSphereRaycast = float3.normalize(this.commonVector);
		return true;
	}

	setAxisText(x, y, z) {
		const fixedLength = 2;
		x.textContent = this.axis.x.toFixed(fixedLength) + 'f';
		y.textContent = this.axis.y.toFixed(fixedLength) + 'f';
		z.textContent = (-this.axis.z).toFixed(fixedLength) + 'f';
	}
}

class HoverableAxis {
	constructor(color, axis, parent, uses3D) {
		const use3D = uses3D === undefined ? false : uses3D;
		const root = new THREE.Object3D();
		{ // cube axis line
			let cubeLine;
			if (!use3D) {
				const points = [];
				points.push(new THREE.Vector3(0, 0, 0));
				points.push(new THREE.Vector3(axis.x, axis.y, axis.z));
				const geometry = new THREE.BufferGeometry().setFromPoints(points);
				{ // base
					const material = new THREE.LineBasicMaterial({
						color: color,
						linewidth: 2
					});
					cubeLine = new THREE.Line(geometry, material);
					root.add(cubeLine);
				}
				{ // hover
					const material = new THREE.LineBasicMaterial({
						color: color,
						linewidth: 4
					});
					this.hoverRoot = new THREE.Line(geometry, material);
					root.add(this.hoverRoot);
					this.hideHover();
				}
			}

			{ // cube cone
				const geometry = new THREE.ConeGeometry(.05, .15, 32);
				const material = new THREE.MeshBasicMaterial({color: color});
				const cubeCone = new THREE.Mesh(geometry, material);
				cubeCone.position.set(axis.x, axis.y, axis.z);
				let otherAxis = float3.up();
				if (axis.y > axis.x && axis.y > axis.z)
					otherAxis = float3.right();
				const rot = quaternion.lookRotation(otherAxis, axis);
				cubeCone.quaternion.set(rot.x, rot.y, rot.z, rot.w);
				cubeLine.add(cubeCone);
			}
			parent.add(root);
			this.root = root;
		}
	}

	showHover() {
		this.hoverRoot.visible = true;
	}

	hideHover() {
		this.hoverRoot.visible = false;
	}
}

class PlainAxis {
	constructor(color, axis, parent) {
		{ // cube axis line
			const root = new THREE.Object3D();
			const points = [];
			points.push(new THREE.Vector3(0, 0, 0));
			points.push(new THREE.Vector3(axis.x, axis.y, axis.z));
			const geometry = new THREE.BufferGeometry().setFromPoints(points);
			const material = new THREE.LineBasicMaterial({
				color: color,
				linewidth: 2
			});
			const cubeLine = new THREE.Line(geometry, material);
			root.add(cubeLine);

			{ // cube cone
				const geometry = new THREE.ConeGeometry(.05, .15, 32);
				const material = new THREE.MeshBasicMaterial({color: color});
				const cubeCone = new THREE.Mesh(geometry, material);
				cubeCone.position.set(axis.x, axis.y, axis.z);
				let otherAxis = float3.up();
				if (axis.x === 0 && axis.y === 1 && axis.z === 0)
					otherAxis = float3.right();
				const rot = quaternion.lookRotation(otherAxis, axis);
				cubeCone.quaternion.set(rot.x, rot.y, rot.z, rot.w);
				cubeLine.add(cubeCone);
			}
			parent.add(root);
			this.root = root;
		}
	}
}

export {AxisHandle, HoverableAxis, PlainAxis};