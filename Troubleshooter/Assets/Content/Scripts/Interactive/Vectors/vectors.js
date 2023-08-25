import {
    addPoints,
    circlePoints,
    clearAndRedraw,
    EasingFunctions,
    inverseLerp,
    lerp,
    remap,
    toNormalisedCanvasSpace,
    TouchHandler,
    arrow
} from "../behaviours.js";
import {noise} from "../perlin.js"

var boatImage, xImage;
var xRot;
const islandMin = 100, islandMax = 220, boatRadius = 225, islandNoiseSize = 0.75;

// Positions state
var positionsCode = {};
var toggleSpaceButton, toggleOriginButton;
var positionsState = {
    worldSpace: true,
    fromOrigin: true
};

var reload = () => {
    boatImage = document.getElementById('boat-img')?.querySelector('img');
    xImage = document.getElementById('x-img')?.querySelector('img');
    noise.seed(Math.random());
    xRot = Math.random() * Math.PI * 2;

    positionsCode.originWorldSpace = document.querySelector(".vectors-code__positions--origin-world_space");
    positionsCode.originLocalSpace = document.querySelector(".vectors-code__positions--origin-local_space");
    positionsCode.boatLocalSpace = document.querySelector(".vectors-code__positions--boat-local_space");
    positionsCode.boatWorldSpace = document.querySelector(".vectors-code__positions--boat-world_space");
    toggleSpaceButton = document.getElementById("vectors-map__positions--toggle-space-button")
    toggleOriginButton = document.getElementById("vectors-map__positions--toggle-origin-button")

    toggleSpaceButton.addEventListener("click", () => {
        positionsState.worldSpace = !positionsState.worldSpace;
        updateButtons__positions();
    });
    toggleOriginButton.addEventListener("click", () => {
        positionsState.fromOrigin = !positionsState.fromOrigin;
        updateButtons__positions();
    });

    redraw();
}

var redraw = () => {
    reloadCanvasByName('vectors-map', drawMap);
    reloadCanvasByName('vectors-map__global', drawMap__global);
    reloadCanvasByName('vectors-map__local', drawMap__local);
    reloadCanvasByName('vectors-map__local--multi', drawMap__localMulti);
    reloadCanvasByName('vectors-map__x--global', drawMap__xGlobal);
    reloadCanvasByName('vectors-map__x--local', drawMap__xLocal);
    reloadCanvasByName('vectors-map__positions', drawMap__positions);
    reloadCanvasByName('vectors-map__relative', drawMap__relative);
}

function reloadCanvasByName(name, callback) {
    const swp_canvas = document.getElementById(name);
    const swp_ctx = swp_canvas.getContext('2d');

    // Handle clicking on the canvas
    new TouchHandler(swp_canvas, touchEvent, touchEvent, null, {
        ctx: swp_ctx,
        canvas: swp_canvas,
        callback: (args, pos) => {
            clearAndRedraw(args.ctx, args.canvas, () => callback(args.ctx, pos, args.canvas))
        }
    });

    callback(swp_ctx, null, swp_canvas);
}

fireCallbackIfPageIsCurrent(reload);
reload();

function configureForIsland(ctx) {
    ctx.lineWidth = 3;
    ctx.strokeStyle = "#6c7e2a";
    ctx.fillStyle = "#4d7216";
}

function configureForGrid(ctx) {
    ctx.lineWidth = 1;
    ctx.font = '13px JetBrains Mono, monospace';
    ctx.strokeStyle = "#ffffff5f";
}


function drawMap(ctx, pos) {
    drawIsland(ctx);
    drawX(ctx);
    drawOutline(ctx);
}

function drawMap__global(ctx, pos) {
    drawIsland(ctx);
    ctx.translate(0, 500);
    drawGrid(ctx, {min: 0, max: 10});
    ctx.resetTransform();
}

function drawMap__local(ctx, pos) {
    drawIsland(ctx);
    ctx.translate(250, 250);
    drawGrid(ctx, {min: -5, max: 5});
    ctx.resetTransform();
}

function drawMap__localMulti(ctx, pos, canvas) {
    drawIsland(ctx);
    const boat = drawBoat(ctx);

    ctx.setTransform(boat.transform);
    // ctx.translate(0, 500);
    drawGrid(ctx, {min: -10, max: 12});
    ctx.resetTransform();

    window.requestAnimationFrame(() => {
        clearAndRedraw(ctx, canvas, () => {
            drawMap__localMulti(ctx, pos, canvas);
        })
    });
}

function drawMap__xGlobal(ctx, pos) {
    drawIsland(ctx);
    const xPos = drawX(ctx);

    ctx.translate(0, 500);
    const transform = ctx.getTransform();
    const range = {min: 0, max: 10};
    drawGrid(ctx, range);

    const transformedPoint = transform.transformPoint(new DOMPoint(xPos.x, -xPos.y));
    const x = remap(transformedPoint.x, 0, 500, 0, 10);
    const y = remap(transformedPoint.y, 0, 500, 0, 10);

    ctx.resetTransform();
    drawTextPosition2D(ctx, xPos, x, y);
}

function drawMap__xLocal(ctx, pos, canvas) {
    drawIsland(ctx);
    const xPos = drawX(ctx);
    const boat = drawBoat(ctx);

    const range = {min: -10, max: 12};
    ctx.setTransform(boat.transform);
    drawGrid(ctx, range);

    const invertedMatrix = boat.transform;
    invertedMatrix.invertSelf();
    const transformedPoint = invertedMatrix.transformPoint(new DOMPoint(xPos.x, xPos.y));
    const x = remap(transformedPoint.x, 0, 500, 0, 10);
    const y = remap(transformedPoint.y, 0, -500, 0, 10);

    ctx.resetTransform();
    drawTextPosition2D(ctx, xPos, x, y);

    window.requestAnimationFrame(() => {
        clearAndRedraw(ctx, canvas, () => {
            drawMap__xLocal(ctx, pos, canvas);
        })
    });
}

function updateButtons__positions() {
    toggleOriginButton.innerText = positionsState.fromOrigin ? "World relative" : "Boat relative";
    toggleSpaceButton.innerText = positionsState.worldSpace ? "World space" : "Local space";

    if (positionsState.fromOrigin) {
        if (positionsState.worldSpace) {
            positionsCode.originLocalSpace.classList.add("hidden");
            positionsCode.boatLocalSpace.classList.add("hidden");
            positionsCode.boatWorldSpace.classList.add("hidden");
            positionsCode.originWorldSpace.classList.remove("hidden");
        } else {
            positionsCode.originWorldSpace.classList.add("hidden");
            positionsCode.boatLocalSpace.classList.add("hidden");
            positionsCode.boatWorldSpace.classList.add("hidden");
            positionsCode.originLocalSpace.classList.remove("hidden");
        }
    } else {
        if (positionsState.worldSpace) {
            positionsCode.originWorldSpace.classList.add("hidden");
            positionsCode.originLocalSpace.classList.add("hidden");
            positionsCode.boatLocalSpace.classList.add("hidden");
            positionsCode.boatWorldSpace.classList.remove("hidden");
        } else {
            positionsCode.originWorldSpace.classList.add("hidden");
            positionsCode.originLocalSpace.classList.add("hidden");
            positionsCode.boatWorldSpace.classList.add("hidden");
            positionsCode.boatLocalSpace.classList.remove("hidden");
        }
    }
}

function drawMap__positions(ctx, pos, canvas) {
    drawIsland(ctx);
    const xPos = drawX(ctx);
    const boat = drawBoat(ctx);

    const local = !positionsState.worldSpace;
    const fromOrigin = positionsState.fromOrigin;
    let range;
    if (fromOrigin) {
        ctx.resetTransform();
        ctx.translate(0, 500);
        range = {min: 0, max: 10};
    } else {
        if (local) {
            range = {min: -10, max: 12};
            ctx.setTransform(boat.transform);
        } else {
            ctx.translate(boat.x, boat.y);
            range = {min: -12, max: 12};
        }
    }
    drawGrid(ctx, range);

    let transformedPoint;
    if (fromOrigin) {
        transformedPoint = new DOMPoint(xPos.x, xPos.y - 500);
    } else {
        if (local) {
            const invertedMatrix = boat.transform;
            invertedMatrix.invertSelf();
            transformedPoint = invertedMatrix.transformPoint(new DOMPoint(xPos.x, xPos.y));
        } else {
            ctx.resetTransform();
            ctx.translate(250 + boat.x, 250 + boat.y);
            const invertedMatrix = ctx.getTransform();
            invertedMatrix.invertSelf();
            transformedPoint = invertedMatrix.transformPoint(new DOMPoint(xPos.x, xPos.y));
        }
    }
    const x = remap(transformedPoint.x, 0, 500, 0, 10);
    const y = remap(transformedPoint.y, 0, -500, 0, 10);

    ctx.resetTransform();

    ctx.fillStyle = "#fda900";
    if (fromOrigin)
        arrow(ctx, 0, 500, xPos.x, xPos.y, 10, 25, 30);
    else
        arrow(ctx, 250 + boat.x, 250 + boat.y, xPos.x, xPos.y, 10, 25, 30);
    ctx.fill();

    drawTextPosition2D(ctx, xPos, x, y);

    window.requestAnimationFrame(() => {
        clearAndRedraw(ctx, canvas, () => {
            drawMap__positions(ctx, pos, canvas);
        })
    });
}

function drawMap__relative(ctx, pos) {
    drawIsland(ctx);
    ctx.translate(0, 500);
    drawGrid(ctx, {min: 0, max: 10});
    ctx.resetTransform();
    
    // TODO draw moored boat
    // TODO draw wandering north arrow
}

function drawIsland(ctx) {
    configureForIsland(ctx);
    ctx.beginPath();
    let start = false;
    for (let i = 0; i < 1024; i++) {
        const v = i / 1023.0;
        const a = Math.PI * 2 * v;
        const cx = Math.cos(a);
        const sy = Math.sin(a);
        const amplitude = lerp(islandMin, islandMax, noise.perlin2(cx * islandNoiseSize, sy * islandNoiseSize));
        const x = cx * amplitude;
        const y = sy * amplitude;
        if (!start) {
            start = true;
            ctx.moveTo(250 + x, 250 + y);
        } else {
            ctx.lineTo(250 + x, 250 + y);
        }
    }
    ctx.closePath();
    ctx.stroke();
    ctx.fill();
}

function drawOutline(ctx) {
    configureForGrid(ctx);
    ctx.beginPath();
    ctx.moveTo(0, 0);
    ctx.lineTo(0, 500);
    ctx.lineTo(500, 500);
    ctx.lineTo(500, 0);
    ctx.closePath();
    ctx.stroke();
}

function drawGrid(ctx, range) {
    configureForGrid(ctx);
    ctx.beginPath();

    const minX = remap(range.min, 0, 10, 1, 500 - 1);
    const maxX = remap(range.max, 0, 10, 1, 500 - 1);

    const minY = remap(range.min, 0, 10, -1, -500);
    const maxY = remap(range.max, 0, 10, -1, -500);

    ctx.fillStyle = "#aaa";
    for (let v = range.min; v <= range.max; v++) {
        const isCenter = v === 0;
        const x = remap(v, 0, 10, 1, 500 - 1);
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
        const y = remap(v, 0, 10, -1, -500);
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

    const offset = 10;
    for (let v = range.min; v <= range.max; v++) {
        const t = v.toString();
        const x = remap(v, 0, 10, 0, 500);
        const y = remap(v, 0, 10, 0, -500);
        ctx.fillText(t, x - 4 + offset, -5);
        ctx.fillText(t, 5, y + 5 + offset);
    }
}

function drawBoat(ctx) {
    const time = new Date();
    const rot = ((2 * Math.PI) / 60) * time.getSeconds() +
        ((2 * Math.PI) / 60000) * time.getMilliseconds();

    const cx = Math.cos(rot) * boatRadius;
    const sy = Math.sin(rot) * boatRadius;

    ctx.translate(250 + cx, 250 + sy);
    ctx.rotate(rot + Math.PI);

    const t = ctx.getTransform();

    // boat
    if (boatImage != null) {
        const xScaled = cx * 0.05;
        const yScaled = sy * 0.05;
        if (!boatImage.complete) {
            boatImage.addEventListener('load', e => {
                ctx.drawImage(boatImage, -20, -20, 40, 40);
            });
        } else
            ctx.drawImage(boatImage, -20, -20, 40, 40);

        // wake
        let grd = ctx.createLinearGradient(0, -20, 0, 40);
        grd.addColorStop(0, "#ffffff90");
        grd.addColorStop(1, "#ffffff00");
        ctx.fillStyle = grd;
        ctx.beginPath();
        ctx.moveTo(0, -20);
        ctx.lineTo(10, 0);
        ctx.lineTo(12, 20);
        ctx.lineTo(15 + noise.perlin2(xScaled, yScaled - 0.2) * 2, 40);
        ctx.lineTo(0, noise.perlin2(xScaled, yScaled) * 5);
        ctx.lineTo(-15 + noise.perlin2(xScaled, yScaled + 0.2) * 2, 40);
        ctx.lineTo(-12, 20);
        ctx.lineTo(-10, 0);
        ctx.closePath();
        ctx.fill();

        // trail
        ctx.resetTransform();
        ctx.translate(250, 250);
        grd = ctx.createRadialGradient(cx, sy, 0, cx, sy, 200);
        grd.addColorStop(0, "#ffffff30");
        grd.addColorStop(1, "#ffffff00");
        ctx.strokeStyle = grd;
        ctx.linewidth = 25;
        ctx.beginPath();
        ctx.moveTo(cx, sy);
        for (let g = 0; g < 20; g++) {
            const offset = g / 20.0;
            const cx2 = Math.cos(rot - offset);
            const sy2 = Math.sin(rot - offset);
            let r = boatRadius;
            r += noise.perlin2(cx2, sy2) * 50 * (g / 20.0);
            ctx.lineTo(cx2 * r, sy2 * r);
        }
        ctx.stroke();
        ctx.linewidth = 1;
    }
    return {x: cx, y: sy, rot: rot, transform: t};
}

function drawX(ctx) {
    if (xImage == null) return;

    const cx = Math.cos(xRot);
    const sy = Math.sin(xRot);
    const amplitude = lerp(islandMin, islandMax, noise.perlin2(cx * islandNoiseSize, sy * islandNoiseSize)) * 0.8;
    const x = cx * amplitude;
    const y = sy * amplitude;

    if (!xImage.complete) {
        xImage.addEventListener('load', e => {
            ctx.drawImage(xImage, 250 + x - 20, 250 + y - 20, 40, 40);
        });
    } else {
        ctx.drawImage(xImage, 250 + x - 20, 250 + y - 20, 40, 40);
    }
    return {x: 250 + x, y: 250 + y};
}

function drawTextPosition2D(ctx, pos, x, y, drawCircle) {
    if (pos == null) return;

    if (drawCircle) {
        ctx.beginPath();
        ctx.fillStyle = "#fff";
        addPoints(ctx, circlePoints(30, pos.x, pos.y, 5));
        ctx.fill();
    }

    ctx.fillStyle = "#fff";
    ctx.fillText(`(${x.toFixed(2)}, ${y.toFixed(2)})`, pos.x + 15, pos.y + 5);
}

function touchEvent(e, args) {
    if (e.button !== undefined && e.button !== 0)
        return false;

    e.preventDefault();
    const pos = toNormalisedCanvasSpace(args.canvas, e);
    pos[0] *= 500;
    pos[1] *= 500;
    args.callback(args, pos);
    return true;
}