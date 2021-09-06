const canvas = document.getElementById('screen_to_world_point');
const ctx = canvas.getContext('2d');

ctx.lineWidth = 3;
ctx.font = '20px JetBrains Mono, monospace';

let distance = 400;
let rayHeightNormalised = 0.3;

function drawScreenToWorldPointDiagram(ctx, distance, rayHeightNormalised) {
	let nearClipPlane = 50;
	let frustumHeightStart = 250 * (nearClipPlane / 400.0); // Similar triangles
	let nearClipPos = 100 + nearClipPlane;

	{ // Frustum
		{ // Frustum - Left
			ctx.strokeStyle = "#00ff815f";

			ctx.beginPath();
			ctx.moveTo(nearClipPos, 250 - frustumHeightStart);
			ctx.lineTo(100, 250);
			ctx.lineTo(nearClipPos, 250 + frustumHeightStart);
			ctx.closePath();
			ctx.stroke();
		}

		{ // Frustum - Right Area
			ctx.strokeStyle = "#00ff81";
			ctx.fillStyle = "#0fc86d0f";

			ctx.beginPath();
			ctx.moveTo(nearClipPos, 250 - frustumHeightStart);
			ctx.lineTo(510, -5);
			ctx.lineTo(510, 505);
			ctx.lineTo(nearClipPos, 250 + frustumHeightStart);
			ctx.closePath();
			ctx.stroke();
			ctx.fill();
		}
	}

	{ // Camera
		const image = document.getElementById('camera-img').querySelector('img');

		if (!image.complete) {
			image.addEventListener('load', e => {
				ctx.drawImage(image, 30, 250 - 40, 80, 80);
			});
		} else {
			ctx.drawImage(image, 30, 250 - 40, 80, 80);
		}
	}


	{ // Distance Indicators

		let indicatorHeight = 475;
		let indicatorLineHeight = indicatorHeight - 15;
		let indicatorTextHeight = indicatorHeight + 5;
		ctx.strokeStyle = "#fff";
		ctx.fillStyle = "#fff";
		ctx.lineWidth = 1;
		ctx.beginPath();

		ctx.moveTo(100, 0);
		ctx.lineTo(100, indicatorHeight);

		let zeroText = "0";
		let textMetrics = ctx.measureText(zeroText);
		let zeroTextWidth = textMetrics.width / 2.0;
		let textHeight = indicatorTextHeight + textMetrics['actualBoundingBoxAscent'];
		ctx.fillText(zeroText, 100 - zeroTextWidth, textHeight);

		ctx.moveTo(nearClipPos, 0);
		ctx.lineTo(nearClipPos, indicatorHeight);

		let nearClipPlaneText = "nearClipPlane";
		textMetrics = ctx.measureText(nearClipPlaneText);
		let nearClipPlaneTextPos = nearClipPos - zeroTextWidth + textMetrics.width;
		ctx.fillText(nearClipPlaneText, nearClipPos - zeroTextWidth, textHeight);

		ctx.moveTo(distance, 0);
		ctx.lineTo(distance, indicatorHeight);

		const distanceLeftSide = nearClipPlaneTextPos + 5;
		const distanceRightSideFadeEnd = distanceLeftSide + 20;
		if (distance > distanceLeftSide) {
			let distanceText = "distance";
			textMetrics = ctx.measureText(distanceText);
			let distanceTextWidth = textMetrics.width;

			ctx.fillStyle = "rgba(255, 255, 255, " + inverseLerp(distanceLeftSide, distanceRightSideFadeEnd, distance) + ")";
			ctx.fillText(distanceText,
				remap(
					distance,
					distanceLeftSide,
					500,
					distance,
					distance - distanceTextWidth,
					EasingFunctions.easeInOutSine
				),
				textHeight
			);
			ctx.fillStyle = "#fff";
		}

		ctx.setLineDash([2, 2]);
		ctx.stroke();

		ctx.beginPath();
		ctx.moveTo(0, indicatorLineHeight);
		ctx.lineTo(500, indicatorLineHeight);
		ctx.setLineDash([]);
		ctx.stroke();
	}

	{ // Ray
		let heightAtDistance = 250 * ((distance - 100) / 400.0); // Similar triangles

		let distanceHeight = lerp(-heightAtDistance, heightAtDistance, rayHeightNormalised);

		ctx.lineWidth = 2;
		ctx.beginPath();
		ctx.moveTo(100, 250);
		ctx.lineTo(distance, 250 + distanceHeight);
		ctx.stroke();

		addPoints(ctx, circlePoints(30, distance, 250 + distanceHeight, 5));
		if (distance < nearClipPos) {
			ctx.fillStyle = "#ff0000";
		}
		ctx.fill();
	}
}

function redrawScreenToWorldPointDiagram() {
	clearAndRedraw(ctx, () => drawScreenToWorldPointDiagram(ctx, distance, rayHeightNormalised))
}

new Slider(document.getElementById("screen_to_world_point_slider"), function (x) {
	distance = 100 + x * 400;
	redrawScreenToWorldPointDiagram();
}, undefined, 0.75);

new TouchHandler(canvas, TouchEvent, TouchEvent);

function TouchEvent(e) {
	const pos = toCanvasSpace(e);
	let x = inverseLerp(100, 500, pos[0]);
	if (x < 0.1)
		return;
	let y = x * 250;
	let rayHeightNormalisedPrev = rayHeightNormalised;
	rayHeightNormalised = inverseLerp(250 - y, 250 + y, pos[1]);
	if (rayHeightNormalisedPrev === rayHeightNormalised)
		return;
	redrawScreenToWorldPointDiagram();
}