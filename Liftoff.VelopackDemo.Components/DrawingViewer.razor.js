let _el = null;
let _ref = null;

let _vbX, _vbY, _vbW, _vbH;
let _isPanning = false;
let _lastX = 0, _lastY = 0;

export function init(el, dotnetRef, vbX, vbY, vbW, vbH) {
    _el = el;
    _ref = dotnetRef;

    const rect = _el.getBoundingClientRect();

    // Adjust initial viewBox height to match element aspect ratio so
    // preserveAspectRatio="none" never distorts, and rect dimensions map 1:1.
    _vbX = vbX;
    _vbW = vbW;
    _vbH = vbW * (rect.height / rect.width);
    _vbY = (vbY + vbH / 2) - _vbH / 2; // keep vertical centre

    applyViewBox();

    _el.addEventListener('wheel', handleWheel, { passive: false });
    _el.addEventListener('mousedown', handleMouseDown);
    _el.addEventListener('mousemove', handleMouseMove);
    _el.addEventListener('mouseup', handleMouseUp);
    _el.addEventListener('mouseleave', handleMouseUp);

    return [rect.width, rect.height, _vbX, _vbY, _vbW, _vbH];
}

export function dispose() {
    if (!_el) return;
    _el.removeEventListener('wheel', handleWheel);
    _el.removeEventListener('mousedown', handleMouseDown);
    _el.removeEventListener('mousemove', handleMouseMove);
    _el.removeEventListener('mouseup', handleMouseUp);
    _el.removeEventListener('mouseleave', handleMouseUp);
    _el = null;
    _ref = null;
}

export function updateViewBox(vbX, vbY, vbW, vbH) {
    _vbX = vbX; _vbY = vbY; _vbW = vbW; _vbH = vbH;
    applyViewBox();
}

export function getElementSize() {
    const rect = _el.getBoundingClientRect();
    return [rect.width, rect.height];
}

function applyViewBox() {
    _el.setAttribute('viewBox', `${_vbX} ${_vbY} ${_vbW} ${_vbH}`);
}

function handleMouseDown(e) {
    if (e.button !== 0) return;
    _isPanning = true;
    _lastX = e.clientX;
    _lastY = e.clientY;
    _el.style.cursor = 'grabbing';
}

function handleMouseMove(e) {
    if (!_isPanning) return;
    const rect = _el.getBoundingClientRect();
    _vbX -= (e.clientX - _lastX) / rect.width * _vbW;
    _vbY -= (e.clientY - _lastY) / rect.height * _vbH;
    _lastX = e.clientX;
    _lastY = e.clientY;
    applyViewBox();
}

function handleMouseUp(e) {
    if (!_isPanning) return;
    _isPanning = false;
    _el.style.cursor = 'grab';
    _ref.invokeMethodAsync('SyncViewBox', _vbX, _vbY, _vbW, _vbH);
}

function handleWheel(e) {
    e.preventDefault();
    const rect = _el.getBoundingClientRect();
    const offsetX = e.clientX - rect.left;
    const offsetY = e.clientY - rect.top;

    const zoomFactor = 1.12;
    const scale = e.deltaY > 0 ? zoomFactor : 1.0 / zoomFactor;

    const cx = _vbX + offsetX / rect.width * _vbW;
    const cy = _vbY + offsetY / rect.height * _vbH;

    _vbW *= scale;
    _vbH *= scale;
    _vbX = cx - offsetX / rect.width * _vbW;
    _vbY = cy - offsetY / rect.height * _vbH;

    applyViewBox();
    _ref.invokeMethodAsync('SyncViewBox', _vbX, _vbY, _vbW, _vbH);
}