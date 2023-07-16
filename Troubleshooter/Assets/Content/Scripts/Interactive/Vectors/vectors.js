import {
    addPoints,
    circlePoints,
    clearAndRedraw,
    EasingFunctions,
    inverseLerp,
    lerp,
    remap,
    toNormalisedCanvasSpace,
    TouchHandler
} from "../behaviours.js";
import {noise} from "../perlin.js"

var reload = () => {
    noise.seed(Math.random());
    redraw();
}

var redraw = () => {
    reloadCanvasByName('vectors-map', drawMap);
    reloadCanvasByName('vectors-map__global', drawMap__global);
    reloadCanvasByName('vectors-map__local', drawMap__local);
    reloadCanvasByName('vectors-map__local--multi', drawMap__localMulti);
}

function reloadCanvasByName(name, callback) {
    const swp_canvas = document.getElementById(name);
    const swp_ctx = swp_canvas.getContext('2d');

    // Handle clicking on the canvas
    new TouchHandler(swp_canvas, touchEvent, touchEvent, null, {
        ctx: swp_ctx,
        canvas: swp_canvas,
        callback: (args, pos) => {
            clearAndRedraw(args.ctx, args.canvas, () => callback(args.ctx, pos))
        }
    });

    callback(swp_ctx, null, swp_canvas);
}

fireCallbackIfPageIsCurrent(reload);
reload();

function configureForIsland(ctx) {
    ctx.lineWidth = 3;
    ctx.strokeStyle = "#af2";
    ctx.fillStyle = "#4d7216";
}

function configureForGrid(ctx) {
    ctx.lineWidth = 1;
    ctx.font = '13px JetBrains Mono, monospace';
    ctx.strokeStyle = "#ffffff5f";
}


function drawMap(ctx, pos) {
    drawIsland(ctx);
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

    const time = new Date();
    const rot = ((2 * Math.PI) / 60) * time.getSeconds() +
        ((2 * Math.PI) / 60000) * time.getMilliseconds();

    const cx = Math.cos(rot) * 225;
    const sy = Math.sin(rot) * 225;

    ctx.translate(250 + cx, 250 + sy);
    ctx.rotate(rot + Math.PI);
    // ctx.translate(0, 500);
    drawGrid(ctx, {min: -10, max: 12});
    ctx.resetTransform();

    window.requestAnimationFrame(() => {
        clearAndRedraw(ctx, canvas, () => {
            drawMap__localMulti(ctx, pos, canvas);
        })
    });
}

function drawIsland(ctx) {
    configureForIsland(ctx);
    ctx.beginPath();
    let start = false;
    const noiseSize = 0.75;
    for (let i = 0; i < 1024; i++) {
        const v = i / 1023.0;
        const a = Math.PI * 2 * v;
        const cx = Math.cos(a);
        const sy = Math.sin(a);
        const amplitude = lerp(100, 220, noise.perlin2(cx * noiseSize, sy * noiseSize));
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
            ctx.strokeStyle = "#00ff005f";
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
            ctx.strokeStyle = "#ff00005f";
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
        ctx.fillText(t, x - 4 + offset,  -5);
        ctx.fillText(t, 5, y + 5 + offset);
    }
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