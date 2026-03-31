import Konva from 'https://esm.sh/konva@9';

let _stage = null;
let _layer = null;
let _ref = null;

let _scaleStep = 0;
const _scalingConstant = 0.347; // e^0.347 ≈ 1.415, ~41% per tick

let _isPanning = false;
let _lastMiddleClickTime = 0;

export function init(containerId, dotnetRef, lines, texts) {
    _ref = dotnetRef;

    const container = document.getElementById(containerId);
    _stage = new Konva.Stage({
        container: containerId,
        width: container.offsetWidth,
        height: container.offsetHeight,
    });

    _layer = new Konva.Layer();
    _stage.add(_layer);

    renderShapes(lines, texts);

    const el = _stage.container();
    el.addEventListener('wheel', handleWheel, { passive: false });
    el.addEventListener('mousedown', handleMouseDown, { passive: false });
    el.addEventListener('mousemove', handleMouseMove);
    el.addEventListener('mouseup', handleMouseUp);
    el.addEventListener('mouseleave', handleMouseUp);

    zoomToFit();
}

export function dispose() {
    if (!_stage) return;
    const el = _stage.container();
    el.removeEventListener('wheel', handleWheel);
    el.removeEventListener('mousedown', handleMouseDown);
    el.removeEventListener('mousemove', handleMouseMove);
    el.removeEventListener('mouseup', handleMouseUp);
    el.removeEventListener('mouseleave', handleMouseUp);
    _stage.destroy();
    _stage = null;
    _layer = null;
    _ref = null;
}

export function updateShapes(lines, texts) {
    if (!_layer) return;
    _layer.destroyChildren();
    renderShapes(lines, texts);
}

export function zoomToFit() {
    if (!_layer || _layer.children.length === 0) return;

    const box = _layer.getClientRect({ skipTransform: true });
    if (box.width === 0 || box.height === 0) return;

    const pad = 0.05;
    const paddedX = box.x - box.width * pad;
    const paddedY = box.y - box.height * pad;
    const paddedW = box.width * (1 + pad * 2);
    const paddedH = box.height * (1 + pad * 2);

    const newScale = Math.min(_stage.width() / paddedW, _stage.height() / paddedH);

    // Snap scaleStep to the nearest integer for the new scale so subsequent
    // scroll zooming continues smoothly from the fitted level.
    _scaleStep = Math.round(Math.log(newScale) / _scalingConstant);

    _stage.scale({ x: newScale, y: newScale });
    _stage.x(_stage.width()  / 2 - (paddedX + paddedW / 2) * newScale);
    _stage.y(_stage.height() / 2 - (paddedY + paddedH / 2) * newScale);
}

function renderShapes(lines, texts) {
    for (const l of lines) {
        _layer.add(new Konva.Line({
            points: [l.x1, l.y1, l.x2, l.y2],
            stroke: l.stroke,
            strokeWidth: l.strokeWidth,
            lineCap: 'round',
        }));
    }
    for (const t of texts) {
        _layer.add(new Konva.Text({
            x: t.x,
            y: t.y,
            text: t.content,
            fill: t.fill,
            fontSize: t.fontSize,
            fontFamily: 'monospace',
        }));
    }
}

function handleWheel(e) {
    e.preventDefault();

    const pointer = _stage.getPointerPosition();
    const oldScale = _stage.scaleX();

    // Point in stage (content) coordinate space — must stay fixed under cursor
    const stageX = (pointer.x - _stage.x()) / oldScale;
    const stageY = (pointer.y - _stage.y()) / oldScale;

    _scaleStep += e.deltaY < 0 ? 1 : -1;
    const newScale = Math.exp(_scalingConstant * _scaleStep);

    _stage.scale({ x: newScale, y: newScale });
    _stage.x(pointer.x - stageX * newScale);
    _stage.y(pointer.y - stageY * newScale);
}

function handleMouseDown(e) {
    if (e.button !== 1) return;
    e.preventDefault(); // suppress browser auto-scroll cursor

    const now = Date.now();
    if (now - _lastMiddleClickTime < 300) {
        _lastMiddleClickTime = 0;
        zoomToFit();
        return;
    }
    _lastMiddleClickTime = now;

    _isPanning = true;
    _stage.container().style.cursor = 'grabbing';
}

function handleMouseMove(e) {
    if (!_isPanning) return;
    _stage.x(_stage.x() + e.movementX);
    _stage.y(_stage.y() + e.movementY);
}

function handleMouseUp(e) {
    if (!_isPanning) return;
    if (e.type === 'mouseup' && e.button !== 1) return;
    _isPanning = false;
    _stage.container().style.cursor = '';
}
