import {
    clearAndRedraw,
    remap,
    arrow,
    approximately,
    toFixedWithDots
} from "../behaviours.js?v=1.0.0";

let keyOptions = [
    {x: 0, y: 0},
    {x: 1, y: 0},
    {x: 0, y: 1},
    {x: 1, y: 1},
    {x: -1, y: 0},
    {x: 0, y: -1},
    {x: -1, y: -1},
    {x: 1, y: -1},
    {x: -1, y: 1},
];

let currentIndex = 0;
let normalized = false;
var toggleNormalizedButton;

var reload = () => {
    toggleNormalizedButton = document.getElementById("normalise--toggle-normalized-button")

    toggleNormalizedButton.addEventListener("click", () => {
        normalized = !normalized;
        toggleNormalizedButton.innerText = normalized ? "Normalized" : "Unscaled";
        reloadCanvasByName('normalise', drawNormalisation, true);
    });

    reloadCanvasByName('normalise', drawNormalisation);
}

function reloadCanvasByName(name, callback, reloaded = false) {
    const swp_canvas = document.getElementById(name);
    const swp_ctx = swp_canvas.getContext('2d');

    if (reloaded) {
        clearAndRedraw(swp_ctx, swp_canvas, () => {
            callback(swp_ctx, true, swp_canvas);
        })
        return;
    }

    callback(swp_ctx, null, swp_canvas);


    setInterval(
        () => {
            window.requestAnimationFrame(() => {
                clearAndRedraw(swp_ctx, swp_canvas, () => {
                    callback(swp_ctx, null, swp_canvas);
                })
            });
        },
        1500
    );
}

fireCallbackIfPageIsCurrent(reload);
reload();

function configureForGrid(ctx) {
    ctx.lineWidth = 1;
    ctx.font = '13px JetBrains Mono, monospace';
    ctx.strokeStyle = "#ffffff5f";
    ctx.textBaseline = "alphabetic";
}

function drawNormalisation(ctx, reloaded = false, canvas) {
    const range = {min: -2, max: 2};
    ctx.translate(-25, -80);
    ctx.scale(1.1, 1.1);

    if (normalized) {
        ctx.strokeStyle = "#fff";
        ctx.beginPath();
        ctx.arc(250, 250, 125, 0, 2 * Math.PI);
        ctx.stroke();
    }

    drawGrid(ctx, range);

    if (!reloaded) {
        const prevIndex = currentIndex;
        do {
            currentIndex = Math.floor((Math.random() * keyOptions.length));
        } while (currentIndex === prevIndex);
    }
    const option = keyOptions[currentIndex];

    let x = option.x;
    let y = option.y;
    let scalar = Math.sqrt(x * x + y * y);
    let one = 1;
    if (scalar === 0)
        scalar = 1;
    if (normalized) {
        x /= scalar;
        y /= scalar;
        one = 1 / Math.sqrt(2);
    }

    ctx.font = '12px JetBrains Mono, monospace';
    drawPositionText(ctx, -one, -one, range);
    drawPositionText(ctx, 0, -1, range);
    drawPositionText(ctx, 0, 1, range);
    drawPositionText(ctx, -1, 0, range);
    drawPositionText(ctx, 1, 0, range);
    drawPositionText(ctx, one, -one, range);
    drawPositionText(ctx, one, one, range);
    drawPositionText(ctx, -one, one, range);

    drawArrow(ctx, x, y, range);

    ctx.resetTransform();
    drawKey(ctx, "↑", 250, 390, option.y === 1);
    drawKey(ctx, "↓", 250, 445, option.y === -1);
    drawKey(ctx, "←", 195, 445, option.x === -1);
    drawKey(ctx, "→", 305, 445, option.x === 1);
}

function drawArrow(ctx, x, y, range) {
    ctx.fillStyle = "#fda900";
    const xx = remap(x, range.min, range.max, 1, 500 - 1);
    const yy = remap(y, range.min, range.max, 500 - 1, 1);
    arrow(ctx, 250, 250, xx, yy, 5, 20, 20);
    ctx.fill();

    const value = Math.sqrt(x * x + y * y);
    ctx.font = '15px JetBrains Mono, monospace';
    const txt = approximately(value, 1) ? "1" : approximately(value, 0) ? "0" : `${(Math.sqrt(x * x + y * y)).toFixed(4)}...`;
    drawTextSimple(ctx, txt, x * 0.5, y * 0.5, range);
}

function drawKey(ctx, value, x, y, pressed) {
    ctx.beginPath();
    const size = 50;
    ctx.roundRect(x - size * 0.5, y, size, size, 10);
    ctx.fillStyle = pressed ? "#f19020" : "#000";
    ctx.strokeStyle = pressed ? "#000" : "#ffcb4c";
    ctx.strokeWidth = "1px";
    ctx.fill();
    ctx.stroke();
    ctx.fillStyle = pressed ? "#000" : "#f19020";
    ctx.textAlign = "center";
    ctx.textBaseline = "top";
    ctx.font = 'bold 24px JetBrains Mono, monospace';
    ctx.fillText(value, x, y + 5);
}

function drawPositionText(ctx, x, y, range) {
    const value = `(${toFixedWithDots(x, 3)}, ${toFixedWithDots(y, 3)})`;
    const metrics = ctx.measureText(value);
    let actualHeight = metrics.actualBoundingBoxAscent + metrics.actualBoundingBoxDescent;

    const offset = 10;
    const xSign = Math.sign(x);
    ctx.textAlign = xSign === 0 ? "center" : xSign > 0 ? "left" : "right";
    ctx.textBaseline = "alphabetic";

    const xx = remap(x, range.min, range.max, 1, 500 - 1) + xSign * 2;
    const yy = remap(y, range.min, range.max, 500 - 1, 1) + actualHeight * 0.5 - offset * Math.sign(y) - 2;
    ctx.fillStyle = "#282828";
    ctx.beginPath();
    const halfWidth = metrics.width * 0.5;
    ctx.rect(xx + halfWidth * xSign - halfWidth, yy - actualHeight, metrics.width, actualHeight + 4)
    ctx.fill();

    ctx.fillStyle = "#ffffff";
    ctx.fillText(value, xx, yy);
}

function drawTextSimple(ctx, value, x, y, range) {
    ctx.textAlign = "center";
    ctx.textBaseline = "alphabetic";
    const metrics = ctx.measureText(value);
    let actualHeight = metrics.actualBoundingBoxAscent + metrics.actualBoundingBoxDescent;

    const xx = remap(x, range.min, range.max, 1, 500 - 1);
    const yy = remap(y, range.min, range.max, 500 - 1, 1) + actualHeight * 0.5;
    ctx.fillStyle = "#282828";
    ctx.beginPath();
    const width = Math.max(metrics.width, 20);
    const halfWidth = width * 0.5;
    ctx.rect(xx - halfWidth, yy - actualHeight - 2, width, actualHeight + 4)
    ctx.fill();

    ctx.fillStyle = "#ffffff";
    ctx.fillText(value, xx, yy);
}

function drawGrid(ctx, range) {
    configureForGrid(ctx);
    ctx.beginPath();

    const minX = 1;
    const maxX = 500 - 1;

    const minY = 500 - 1;
    const maxY = -1;

    ctx.fillStyle = "#aaa";
    for (let v = range.min; v <= range.max; v++) {
        const isCenter = v === 0;
        const x = remap(v, range.min, range.max, 1, 500 - 1);
        if (isCenter) {
            if (v !== range.min) {
                ctx.stroke();
                ctx.beginPath();
            }
            ctx.strokeStyle = "#a9fd00";
            ctx.moveTo(x, minY);
            ctx.lineTo(x, maxY);
            ctx.stroke();
            // continued in y
        } else {
            ctx.moveTo(x, minY);
            ctx.lineTo(x, maxY);
        }
        const y = remap(v, range.min, range.max, 500 - 1, 1);
        if (isCenter) {
            ctx.strokeStyle = "#fd003b";
            ctx.beginPath();
            ctx.moveTo(minX, y);
            ctx.lineTo(maxX, y);
            ctx.stroke();
            ctx.strokeStyle = "#ffffff5f";
            ctx.beginPath();
        } else {
            ctx.moveTo(minX, y);
            ctx.lineTo(maxX, y);
        }
    }
    ctx.stroke();
}