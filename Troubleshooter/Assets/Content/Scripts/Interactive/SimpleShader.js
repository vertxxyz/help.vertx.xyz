const SimpleShader = {

	uniforms: {},

	vertexShader: /* glsl */`
		uniform mat4 modelMatrix;
		uniform mat4 modelViewMatrix;
		uniform mat4 projectionMatrix;
		uniform mat4 viewMatrix;
		uniform mat3 normalMatrix;
		uniform vec3 cameraPosition;
		attribute vec3 position;
		attribute vec3 normal;
		attribute vec2 uv;
		varying highp vec3 colorOut;
	
		void main() {
			gl_Position = projectionMatrix * modelViewMatrix * vec4( position, 1.0 );
			vec3 worldNormal = normalize ( mat3( modelMatrix[0].xyz, modelMatrix[1].xyz, modelMatrix[2].xyz ) * normal );
			colorOut = vec3(pow(dot(worldNormal, normalize(vec3(-0.2, 0.1, -1.0))), 0.5));
		}`,

	fragmentShader: /* glsl */`
		varying highp vec3 colorOut;
		void main() {
			gl_FragColor = vec4( pow (colorOut, vec3(1.3) ), 1.0 );
		}`

};

export {SimpleShader};