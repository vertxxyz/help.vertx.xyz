import {Deg2Rad, Rad2Deg} from "./behaviours.js";

class float3 {
	constructor(x, y, z) {
		if (x === undefined) {
			this.x = 0;
			this.y = 0;
			this.z = 0;
		} else if (y === undefined) {
			this.x = x;
			// noinspection JSSuspiciousNameCombination
			this.y = x;
			this.z = x;
		} else if (z === undefined) {
			this.x = x;
			this.y = y;
			this.z = 0;
		} else {
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}
	
	static copy(v) {
		return new float3(v.x, v.y, v.z);
	}
	
	getAnotherAxis() {
		let otherAxis = float3.up();
		if (this.x === 0 && this.y === 1 && this.z === 0)
			otherAxis = float3.right();
		return otherAxis;
	}

	static forward() {
		return new float3(0, 0, 1);
	}

	static back() {
		return new float3(0, 0, -1);
	}

	static right() {
		return new float3(1, 0, 0);
	}

	static left() {
		return new float3(-1, 0, 0);
	}

	static up() {
		return new float3(0, 1, 0);
	}

	static mul(a, b) {
		if (b.x === undefined) {
			// scalar
			return new float3(a.x * b, a.y * b, a.z * b);
		}
		// float3
		return new float3(a.x * b.x, a.y * b.y, a.z * b.z);
	}

	static plus(a, b) {
		return new float3(a.x + b.x, a.y + b.y, a.z + b.z);
	}

	static sub(a, b) {
		return new float3(a.x - b.x, a.y - b.y, a.z - b.z);
	}

	static dot(a, b) {
		return a.x * b.x + a.y * b.y + a.z * b.z;
	}

	static cross(x, y) {
		let yScaled = float3.mul(new float3(y.y, y.z, y.x), x);
		let xScaled = float3.mul(new float3(x.y, x.z, x.x), y);
		let s = float3.sub(yScaled, xScaled);
		return new float3(s.y, s.z, s.x);
	}

	normalize() {
		let n = float3.normalize(this);
		this.x = n.x;
		this.y = n.y;
		this.z = n.z;
	}

	static normalize(a) {
		return float3.mul(a, 1.0 / Math.sqrt(float3.dot(a, a)));
	}

	sqrMagnitude() {
		return float3.sqrMagnitude(this);
	}

	static sqrMagnitude(a) {
		return a.x * a.x + a.y * a.y + a.z * a.z;
	}

	magnitude() {
		return float3.magnitude(this);
	}

	static magnitude(a) {
		return Math.sqrt(float3.sqrMagnitude(a));
	}

	static sqrDistance(a, b) {
		return float3.sub(a, b).sqrMagnitude();
	}

	static distance(a, b) {
		return float3.sub(a, b).magnitude();
	}

	abs() {
		this.x = Math.abs(this.x);
		this.y = Math.abs(this.y);
		this.z = Math.abs(this.z);
	}

	getAbsolute(query) {
		return new float3(
			query.indexOf('x') >= 0 ? Math.abs(this.x) : this.x,
			query.indexOf('y') >= 0 ? Math.abs(this.y) : this.y,
			query.indexOf('z') >= 0 ? Math.abs(this.z) : this.z
		);
	}

	projectOnPlane(planeNormal) {
		const sqrMag = float3.dot(planeNormal, planeNormal);
		if (sqrMag < Number.EPSILON)
			return this;
		else {
			const dot = float3.dot(this, planeNormal);
			return new float3(
				this.x - planeNormal.x * dot / sqrMag,
				this.y - planeNormal.y * dot / sqrMag,
				this.z - planeNormal.z * dot / sqrMag
			);
		}
	}
}

class quaternion {
	constructor(x, y, z, w) {
		if (x === undefined) {
			this.z = this.y = this.x = 0.0;
			this.w = 1.0;
		} else if (y === undefined) {
			this.w = this.z = this.y = this.x = x;
		} else {
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}
	}

	static mul(a, b) {
		if (b.x === undefined) {
			// scalar
			return new quaternion(a.x * b, a.y * b, a.z * b, a.w * b);
		}
		if (b.w === undefined) {
			// float3
			const x = a.x * 2.0;
			const y = a.y * 2.0;
			const z = a.z * 2.0;
			const xx = a.x * x;
			const yy = a.y * y;
			const zz = a.z * z;
			const xy = a.x * y;
			const xz = a.x * z;
			const yz = a.y * z;
			const wx = a.w * x;
			const wy = a.w * y;
			const wz = a.w * z;

			return new float3(
				(1.0 - (yy + zz)) * b.x + (xy - wz) * b.y + (xz + wy) * b.z,
				(xy + wz) * b.x + (1.0 - (xx + zz)) * b.y + (yz - wx) * b.z,
				(xz - wy) * b.x + (yz + wx) * b.y + (1.0 - (xx + yy)) * b.z
			);
		}
		// quaternion
		return new quaternion(
			a.w * b.x + a.x * b.w + a.y * b.z - a.z * b.y,
			a.w * b.y + a.y * b.w + a.z * b.x - a.x * b.z,
			a.w * b.z + a.z * b.w + a.x * b.y - a.y * b.x,
			a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z);
	}

	static scale(a, b) {
		return new quaternion(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
	}

	normalize() {
		let n = quaternion.normalize(this);
		this.x = n.x;
		this.y = n.y;
		this.z = n.z;
		this.w = n.w;
	}

	static normalize(q) {
		return quaternion.mul(q, 1.0 / Math.sqrt(quaternion.dot(q, q)));
	}

	static dot(a, b) {
		return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
	}

	inverse() {
		let q = quaternion.inverse(this);
		this.x = q.x;
		this.y = q.y;
		this.z = q.z;
		this.w = q.w;
	}

	static inverse(q) {
		return quaternion.scale(quaternion.mul(q, (1.0 / quaternion.dot(q, q))), new quaternion(-1.0, -1.0, -1.0, 1.0));
	}

	static isEqualUsingDot(dot) {
		return dot > 1.0 - 0.000001;
	}

	angle() {
		return quaternion.angle(new quaternion(), this);
	}

	static angle(a, b) {
		const dot = quaternion.dot(a, b);
		return quaternion.isEqualUsingDot(dot) ? 0.0 : Math.acos(Math.min(Math.abs(dot), 1.0)) * 2.0 * Rad2Deg;
	}

	static axisAngleDeg(axis, angle) {
		const x = 0.5 * angle * Deg2Rad;
		const a1 = float3.mul(axis, Math.sin(x));
		return new quaternion(a1.x, a1.y, a1.z, Math.cos(x));
	}

	static axisAngle(axis, angle) {
		let x = 0.5 * angle;
		const a1 = float3.mul(axis, Math.sin(x));
		return new quaternion(a1.x, a1.y, a1.z, Math.cos(x));
	}

	static euler(xyz) {
		let x = 0.5 * xyz;
		let s = Math.sin(x);
		let c = Math.cos(x);
		return new quaternion(
			s.x * c.y * c.z + s.y * s.z * c.x,
			s.y * c.x * c.z - s.x * s.z * c.y,
			s.z * c.x * c.y - s.x * s.y * c.z,
			c.x * c.y * c.z + s.y * s.z * s.x
		);
	}

	static lookRotation(forward, up) {
		if (up === undefined)
			up = new float3(0, 1, 0);
		forward = float3.normalize(forward);
		let vector = float3.normalize(forward);
		let vector2 = float3.normalize(float3.cross(up, vector));
		let vector3 = float3.cross(vector, vector2);
		let m00 = vector2.x;
		let m01 = vector2.y;
		let m02 = vector2.z;
		let m10 = vector3.x;
		let m11 = vector3.y;
		let m12 = vector3.z;
		let m20 = vector.x;
		let m21 = vector.y;
		let m22 = vector.z;


		let num8 = (m00 + m11) + m22;
		let q = new quaternion();
		if (num8 > 0) {
			let num = Math.sqrt(num8 + 1);
			q.w = num * 0.5;
			num = 0.5 / num;
			q.x = (m12 - m21) * num;
			q.y = (m20 - m02) * num;
			q.z = (m01 - m10) * num;
			return q;
		}
		if ((m00 >= m11) && (m00 >= m22)) {
			let num7 = Math.sqrt(((1 + m00) - m11) - m22);
			let num4 = 0.5 / num7;
			q.x = 0.5 * num7;
			q.y = (m01 + m10) * num4;
			q.z = (m02 + m20) * num4;
			q.w = (m12 - m21) * num4;
			return q;
		}
		if (m11 > m22) {
			let num6 = Math.sqrt(((1 + m11) - m00) - m22);
			let num3 = 0.5 / num6;
			q.x = (m10 + m01) * num3;
			q.y = 0.5 * num6;
			q.z = (m21 + m12) * num3;
			q.w = (m20 - m02) * num3;
			return q;
		}
		let num5 = Math.sqrt(((1 + m22) - m00) - m11);
		let num2 = 0.5 / num5;
		q.x = (m20 + m02) * num2;
		q.y = (m21 + m12) * num2;
		q.z = 0.5 * num5;
		q.w = (m01 - m10) * num2;
		return q;
	}

	static fromToRotation(from, to) {
		let axis = float3.cross(from, to);
		if (float3.sqrMagnitude(axis) < 0.001) {
			return quaternion.axisAngle(new float3(0, 1, 0), 180);
		}
		return quaternion.axisAngle(
			float3.normalize(axis),
			Math.acos(Math.min(Math.max(float3.dot(float3.normalize(from), float3.normalize(to)), -1.0), 1.0))
		);
	}

	toEuler() {
		// ZXY
		const epsilon = 1e-6;

		//prepare the data
		const d1 = quaternion.mul(quaternion.mul(this, new quaternion(this.w)), new quaternion(2.0)); //xw, yw, zw, ww
		const d2 = quaternion.mul(quaternion.mul(this, new quaternion(this.y, this.z, this.x, this.w)), new quaternion(2.0)); //xy, yz, zx, ww
		const d3 = quaternion.mul(this, this);
		let euler;

		const CUTOFF = (1.0 - 2.0 * epsilon) * (1.0 - 2.0 * epsilon);

		let y1 = d2.y - d1.x;
		if (y1 * y1 < CUTOFF) {
			const x1 = d2.x + d1.z;
			const x2 = d3.y + d3.w - d3.x - d3.z;
			const z1 = d2.z + d1.y;
			const z2 = d3.z + d3.w - d3.x - d3.y;
			euler = new float3(Math.atan2(x1, x2), -Math.asin(y1), Math.atan2(z1, z2));
		} else //zxz
		{
			y1 = Math.min(Math.max(y1, -1.0), 1.0);
			const abcd = new quaternion(d2.z, d1.y, d2.y, d1.x);
			const x1 = 2.0 * (abcd.x * abcd.w + abcd.y * abcd.z); //2(ad+bc)
			const q = quaternion.mul(quaternion.mul(abcd, abcd, new quaternion(-1.0, 1.0, -1.0, 1.0)));
			const x2 = q.x + q.y + q.z + q.w;
			euler = new float3(Math.atan2(x1, x2), -Math.asin(y1), 0.0);
		}

		return new float3(euler.y, euler.z, euler.x);
	}

	setFrom(rotation) {
		this.x = rotation.x;
		this.y = rotation.y;
		this.z = rotation.z;
		this.w = rotation.w;
	}
}

class ray {
	constructor(position, normal) {
		this.position = position;
		this.normal = normal;
	}

	getPoint(distance) {
		return float3.plus(this.position, float3.mul(this.normal, distance));
	}
}

class sphere {
	constructor(position, radius) {
		this.position = position;
		this.radius = radius;
	}

	// This function will return the value of t
	// if it returns negative, no collision!
	raycast(ray) {
		const p0 = ray.position;
		const d = ray.normal;
		const c = this.position;
		const r = this.radius;

		const e = float3.sub(c, p0);
		// Using Length here would cause floating point error to creep in
		const Esq = float3.sqrMagnitude(e);
		const a = float3.dot(e, d);
		const b = Math.sqrt(Esq - (a * a));
		const f = Math.sqrt((r * r) - (b * b));

		// No collision
		if (r * r - Esq + a * a < 0) {
			return -1; // -1 is invalid.
		}
		// Ray is inside
		else if (Esq < r * r) {
			return a + f; // Just reverse direction
		}
		// else Normal intersection
		return a - f;
	}
}

class plane {
	constructor() {
		this.normal = float3.up();
		this.distance = 0;
	}

	setFromDistance(normal, distance) {
		this.normal = float3.normalize(normal);
		this.distance = distance;
	}

	setFromPoint(normal, point) {
		this.setFromDistance(normal, -float3.dot(normal, point));
	}

	setFromThreePoints(a, b, c) {
		this.normal = float3.normalize(float3.cross(float3.sub(b, a), float3.sub(c, a)))
		this.distance = -float3.dot(this.normal, a);
	}

	getClosestPointOnPlane(point) {
		const pointToPlaneDistance = float3.dot(this.normal, point) + this.distance;
		return float3.sub(point, float3.mul(this.normal, pointToPlaneDistance));
	}

	getDistanceToPoint(point) {
		return float3.dot(this.normal, point) + this.distance;
	}

	getSide(point) {
		return float3.dot(this.normal, point) + this.distance > 0.0;
	}

	isSameSide(a, b) {
		const d0 = this.getDistanceToPoint(a);
		const d1 = this.getDistanceToPoint(b);
		return (d0 > 0.0 && d1 > 0.0) || (d0 <= 0.0 && d1 <= 0.0);
	}

	raycast(ray) {
		const vdot = float3.dot(ray.normal, this.normal);
		const ndot = -float3.dot(ray.position, this.normal) - this.distance;
		if (Math.abs(vdot) < 0.0001)
			return 0.0;
		return ndot / vdot;
	}
}

export {float3, quaternion, ray, sphere, plane}