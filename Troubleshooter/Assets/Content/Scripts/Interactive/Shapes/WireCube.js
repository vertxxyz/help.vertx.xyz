import * as VERTX from "../behaviours.js";
import * as THREE from "../three.module.js";

function getWireCube(colorCorners, size, wireRadius) {
	const p = size * 0.5; // half size
	const n = -p; // negative half size
	const a = VERTX.TAU * 0.25;
	const root = new THREE.Object3D();
	{
		const material = new THREE.MeshDepthMaterial();
		const geometry = new THREE.CylinderGeometry(wireRadius, wireRadius, size, 12, 1, true);
		const back = getSide(material, geometry, p, n);
		root.add(back);
		back.translateZ(n);
		const front = getSide(material, geometry, p, n);
		root.add(front);
		front.translateZ(p);
		const top = getSide(material, geometry, p, n);
		root.add(top);
		top.rotateX(a);
		top.translateZ(n);
		top.rotateZ(a);
		const bottom = getSide(material, geometry, p, n);
		root.add(bottom);
		bottom.rotateX(a);
		bottom.rotateZ(a);
		bottom.translateZ(p);
		const r1 = getSide(material, geometry, p, n);
		root.add(r1);
		r1.rotateX(a);
		r1.translateZ(n);
		const r2 = getSide(material, geometry, p, n);
		root.add(r2);
		r2.rotateX(a);
		r2.translateZ(p);
	}

	{
		const material = new THREE.MeshBasicMaterial({color: colorCorners});
		const geometry = new THREE.IcosahedronGeometry(wireRadius * 4, 2);
		const tlf = new THREE.Mesh(geometry, material);
		const tlb = new THREE.Mesh(geometry, material);
		const trf = new THREE.Mesh(geometry, material);
		const trb = new THREE.Mesh(geometry, material);
		const blf = new THREE.Mesh(geometry, material);
		const blb = new THREE.Mesh(geometry, material);
		const brf = new THREE.Mesh(geometry, material);
		const brb = new THREE.Mesh(geometry, material);
		root.add(tlf);
		root.add(tlb);
		root.add(trf);
		root.add(trb);
		root.add(blf);
		root.add(blb);
		root.add(brf);
		root.add(brb);
		// Y
		tlf.translateY(p);
		tlb.translateY(p);
		trf.translateY(p);
		trb.translateY(p);
		blf.translateY(n);
		blb.translateY(n);
		brf.translateY(n);
		brb.translateY(n);
		// X
		tlf.translateX(n);
		tlb.translateX(n);
		trf.translateX(p);
		trb.translateX(p);
		blf.translateX(n);
		blb.translateX(n);
		brf.translateX(p);
		brb.translateX(p);
		// Z
		tlf.translateZ(p);
		tlb.translateZ(n);
		trf.translateZ(p);
		trb.translateZ(n);
		blf.translateZ(p);
		blb.translateZ(n);
		brf.translateZ(p);
		brb.translateZ(n);
	}

	return root;
}

function getSide(material, geometry, p, n) {
	const root = new THREE.Object3D();
	const bl = new THREE.Mesh(geometry, material/*.clone()*/);
	const br = new THREE.Mesh(geometry, material/*.clone()*/);
	bl.name = "edge";
	br.name = "edge";
	root.add(bl);
	root.add(br);
	bl.position.set(n, 0, 0);
	br.position.set(p, 0, 0);
	return root;
}

export {getWireCube};