import {
    addPoints,
    circlePoints,
    clearAndRedraw,
    lerp,
    Slider
} from "../behaviours.js";

var swp_canvas, swp_ctx;
var a, b, t;
var timeText, valueText;

var pageParameter = processPageValue(null);

var reload = (event) => {
    if (event !== undefined && event.detail !== pageParameter)
        return;

	timeText = document.getElementById("lerp_t");
    valueText = document.getElementById("lerp_value");
	
    swp_canvas = document.getElementById('lerp');
    swp_ctx = swp_canvas.getContext('2d');

    swp_ctx.lineWidth = 3;
    swp_ctx.font = '20px JetBrains Mono, monospace';
    a = 50.0;
    b = 100.0;
    t = 0.5;

    // Handle changing distance via slider
    new Slider(document.getElementById("lerp_time_slider"), function (x) {
        t = x;
        redrawDiagram();
    }, undefined, t);
}

window.addEventListener("loadedFromState", reload);
reload();

function drawDiagram(ctx, a, b, time) {
	const textPadding = 15;
    const border = 20;
    const graphMaxPos = 500 - border;
    const graphMinPos = 50;
	const graphMaxPosY = 500 - graphMinPos;

	ctx.fillStyle = "#ffffff";

	const fixedLength = 2;
	timeText.textContent = time.toFixed(fixedLength) + 'f';
    valueText.textContent = Math.round(lerp(a, b, time) * 100) / 100;

    let aPos, bPos;
	let aText = a.toString();
	let aTextMetrics = ctx.measureText(aText);
	let aXPos = graphMinPos - aTextMetrics.width - textPadding;
    if (a === b) {
        aPos = lerp(graphMinPos, graphMaxPosY, 0.5);
		ctx.fillText(aText, aXPos, aPos);
    } else {
		let bText = b.toString();
		let bTextMetrics = ctx.measureText(bText);
		let bXPos = graphMinPos - bTextMetrics.width - 15;
		let topTextPos = graphMinPos + bTextMetrics['actualBoundingBoxAscent'];
		let aTextPosY, bTextPosY;
		if (a < b) {
			aPos = graphMaxPosY;
			bPos = graphMinPos;
			aTextPosY = graphMaxPosY;
			bTextPosY = topTextPos;
		} else {
			aPos = graphMinPos;
			bPos = graphMaxPosY;
			aTextPosY = topTextPos;
			bTextPosY = graphMaxPosY;
		}
		ctx.fillText(aText, aXPos, aTextPosY);
		ctx.fillText(bText, bXPos, bTextPosY);
	}

    { // Graph
		
        let zeroText = "0";
        let textMetrics = ctx.measureText(zeroText);
        let indicatorTextHeight = graphMaxPosY + 5;
        let textHeight = indicatorTextHeight + textMetrics['actualBoundingBoxAscent'] + textPadding;
        ctx.fillText(zeroText, 55, textHeight)
        let oneText = "1";
        ctx.fillText(oneText, graphMaxPos - 15, textHeight);

		ctx.strokeStyle = "#ffffff";
		ctx.beginPath();
		ctx.moveTo(graphMinPos, graphMinPos);
		ctx.lineTo(graphMinPos, graphMaxPosY);
		ctx.lineTo(graphMaxPos, graphMaxPosY);
		ctx.stroke();
    }

    { // Result
		ctx.strokeStyle = "#aaaaaa";
		ctx.beginPath();
		ctx.moveTo(graphMinPos, graphMaxPosY);
		ctx.lineTo(graphMaxPos, graphMinPos);
		ctx.stroke();
		
        ctx.strokeStyle = "#666666";
        ctx.beginPath();
        let x = lerp(50, graphMaxPos, time);
        let y;
		if (a === b)
			y = aPos;
		else
			y = lerp(aPos, bPos, time);
        ctx.moveTo(x, 450);
        ctx.lineTo(x, y);
        ctx.lineTo(graphMinPos, y);

        ctx.setLineDash([2, 2]);
        ctx.stroke();
        ctx.setLineDash([]);

        addPoints(ctx, circlePoints(30, x, y, 5));
        ctx.fillStyle = "#66C3CC";
        ctx.fill();
    }
}

function redrawDiagram() {
    clearAndRedraw(swp_ctx, swp_canvas, () => drawDiagram(swp_ctx, a, b, t))
}