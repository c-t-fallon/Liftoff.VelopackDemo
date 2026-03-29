let _el = null;
let _ref = null;

let _vbX, _vbY, _vbW, _vbH;
let _scaleStep = 0;
let _refVbW, _refVbH;
let _isPanning = false;
let _lastX = 0, _lastY = 0;
let _lastMiddleClickTime = 0;

const _scalingConstant = 0.347; // e^0.347 ≈ 1.415, ~41% per tick (matches WPF feel)

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

    _refVbW = _vbW;
    _refVbH = _vbH;
    _scaleStep = 0;

    applyViewBox();

    _el.addEventListener('wheel', handleWheel, { passive: false });
    _el.addEventListener('mousedown', handleMouseDown, { passive: false });
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
    _refVbW = vbW; _refVbH = vbH;
    _scaleStep = 0;
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
    if (e.button !== 1) return;
    e.preventDefault(); // suppress browser auto-scroll on middle click

    const now = Date.now();
    const doubleClickThreshold = 300;
    if (now - _lastMiddleClickTime < doubleClickThreshold) {
        _lastMiddleClickTime = 0;
        _ref.invokeMethodAsync('ZoomToFit');
        return;
    }
    _lastMiddleClickTime = now;

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
    if (!_isPanning || e.button !== 1) return;
    _isPanning = false;
    _el.style.cursor = '';
    _ref.invokeMethodAsync('SyncViewBox', _vbX, _vbY, _vbW, _vbH);
}

function handleWheel(e) {
    e.preventDefault();
    const rect = _el.getBoundingClientRect();
    const offsetX = e.clientX - rect.left;
    const offsetY = e.clientY - rect.top;

    // Cursor position in SVG space before zoom
    const cx = _vbX + offsetX / rect.width * _vbW;
    const cy = _vbY + offsetY / rect.height * _vbH;

    // Integer step counter — drift-free, fully reversible
    _scaleStep += e.deltaY < 0 ? 1 : -1;

    // Compute viewBox size from step via exp (same math as WPF sandbox)
    const zoomLevel = Math.exp(_scalingConstant * _scaleStep);
    const newVbW = _refVbW / zoomLevel;
    const newVbH = _refVbH / zoomLevel;

    // Reposition so the cursor SVG point stays fixed on screen
    _vbX = cx - offsetX / rect.width * newVbW;
    _vbY = cy - offsetY / rect.height * newVbH;
    _vbW = newVbW;
    _vbH = newVbH;

    applyViewBox();
    _ref.invokeMethodAsync('SyncViewBox', _vbX, _vbY, _vbW, _vbH);
}