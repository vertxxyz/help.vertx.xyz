import {
    addPoints,
    circlePoints, clamp, clamp01,
    clearAndRedraw, inverseLerp,
    lerp, lerpClamped, remap,
    Slider
} from "../behaviours.js";

var swp_canvas, swp_ctx;
var a, b, t;
var timeText, valueText;
var aInput, bInput;

var pageParameter = processPageValue(null);

var reload = (event) => {
    if (event !== undefined && event.detail !== pageParameter)
        return;

    aInput = document.getElementById("lerp_a");
    bInput = document.getElementById("lerp_b");
    timeText = document.getElementById("lerp_t");
    valueText = document.getElementById("lerp_value");
    
    aInput.addEventListener('input', e => {
        a = clamp(aInput.min, aInput.max, e.target.value);
        redrawDiagram();
        if(a !== e.target.value)
            aInput.value = a;
    });
    bInput.addEventListener('input', e => {
        b = clamp(bInput.min, bInput.max, e.target.value);
        redrawDiagram();
        if(b !== e.target.value)
            bInput.value = b;
    });
	
    swp_canvas = document.getElementById('lerp');
    swp_ctx = swp_canvas.getContext('2d');

    swp_ctx.lineWidth = 3;
    swp_ctx.font = '20px JetBrains Mono, monospace';
    a = 50.0;
    b = 100.0;
    t = 0.5;

    aInput.value = a;
    bInput.value = b;

    // Handle changing distance via slider
    new Slider(document.getElementById("lerp_time_slider"), function (x) {
        t = remap(x, 0, 1, -0.2, 1.2);
        redrawDiagram();
    }, undefined, t);
}

window.addEventListener("loadedFromState", reload);
reload();

function drawDiagram(ctx, a, b, time) {
    const timeClamped = clamp01(time);
	const textPadding = 15;
    const border = 90;
    const graphMaxPos = 500 - border;
    const graphMinPos = border;
	const graphMaxPosY = 500 - graphMinPos;

	ctx.fillStyle = "#ffffff";

	const fixedLength = 2;
    let timeString = time.toFixed(fixedLength).toString();
	timeText.textContent = timeString + 'f';
    let val = lerp(a, b, timeClamped);
    let valueString = (Math.round(val * 100) / 100).toString();
    valueText.textContent = valueString;

    let zeroText = "0"
    let oneText = "1";
    let textMetrics = ctx.measureText(zeroText);
    let indicatorTextHeight = graphMaxPosY + 5;
    let textHeight = indicatorTextHeight + textMetrics['actualBoundingBoxAscent'] + textPadding;

    let aPos, bPos;
	let aText = a.toString();
	let aTextMetrics = ctx.measureText(aText);
	let aXPos = graphMinPos - aTextMetrics.width - textPadding;
    let same = a === b;
    if (same) {
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
			aTextPosY = graphMaxPosY - 5;
			bTextPosY = topTextPos + 5;
		} else {
			aPos = graphMinPos;
			bPos = graphMaxPosY;
			aTextPosY = topTextPos + 5;
			bTextPosY = graphMaxPosY - 5;
		}
		ctx.fillText(aText, aXPos, aTextPosY);
		ctx.fillText(bText, bXPos, bTextPosY);

        // t Text
        ctx.fillStyle = "rgba(102, 195, 204, " + fadeEdges(time) + ")";
        valueString = val.toFixed(2);
        let vTextMetrics = ctx.measureText(valueString);
        ctx.fillText(valueString, graphMinPos - vTextMetrics.width - textPadding, lerp(aTextPosY, bTextPosY, timeClamped));
        ctx.fillStyle = "#ffffff";
	}

    { // Graph
        ctx.fillText(zeroText, graphMinPos + 5, textHeight);
        ctx.fillText(oneText, graphMaxPos - 5 - textMetrics.width, textHeight);

		ctx.strokeStyle = "#ffffff";
		ctx.beginPath();
		ctx.moveTo(graphMinPos, graphMinPos);
		ctx.lineTo(graphMinPos, graphMaxPosY);
		ctx.lineTo(graphMaxPos, graphMaxPosY);
		ctx.stroke();
    }

    { // Result
        let startY, endY;
        if(same) {
            startY = lerp(graphMinPos, graphMaxPosY, 0.5);
            endY = startY;
        }else if(a > b) {
            startY = graphMinPos;
            endY = graphMaxPosY;
        }else{
            endY = graphMinPos;
            startY = graphMaxPosY;
        }
        
        
        ctx.strokeStyle = "#FF9259";        
		ctx.beginPath();
		ctx.moveTo(graphMinPos, startY);
		ctx.lineTo(graphMaxPos, endY);
		ctx.stroke();
        ctx.strokeStyle = "#FF92595f";
        ctx.moveTo(0, startY);
        ctx.lineTo(graphMinPos, startY);
        ctx.moveTo(graphMaxPos, endY);
        ctx.lineTo(500, endY);
        ctx.stroke();
        
        let x = lerp(graphMinPos, graphMaxPos, time);
        let y;
		if (same)
			y = aPos;
		else
			y = lerp(aPos, bPos, timeClamped);
        
        ctx.strokeStyle = "#666666";
        
        if(time >= 0 && time <= 1) {
            ctx.beginPath();
            ctx.setLineDash([2, 2]);
            if(same) {
                
                ctx.moveTo(x, y);
            }else {
                ctx.moveTo(graphMinPos, y);
                ctx.lineTo(x, y);
            }
            ctx.lineTo(x, graphMaxPosY);
            ctx.stroke();
            ctx.setLineDash([]);
        }else{
            ctx.beginPath();
            ctx.setLineDash([2, 2]);
            ctx.moveTo(x, graphMaxPosY);
            ctx.lineTo(x, y);
            ctx.stroke();
            if(!same && y !== graphMaxPosY) {
                ctx.moveTo(graphMinPos, y);
                ctx.lineTo(graphMaxPos, y);
                ctx.stroke();
            }
            ctx.setLineDash([]);
        }

        addPoints(ctx, circlePoints(30, x, y, 5));
        ctx.fillStyle = "#ffffff";
        ctx.fill();

        // t Text
        ctx.fillStyle = "rgba(237, 148, 192, " + fadeEdges(time) + ")";
        timeString = "t:" + timeString;
        let valueTextMetrics = ctx.measureText(timeString);
        ctx.fillText(timeString, x - valueTextMetrics.width * 0.5, textHeight)
    }
}

function fadeEdges(normalisedValue) {
    let v = Math.abs(normalisedValue - 0.5);
    return inverseLerp(0.36, 0.25, v);
}

function redrawDiagram() {
    clearAndRedraw(swp_ctx, swp_canvas, () => drawDiagram(swp_ctx, a, b, t));
}