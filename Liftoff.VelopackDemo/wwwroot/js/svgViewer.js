let _el = null;
let _ref = null;

export function init(el, dotnetRef) {
    _el = el;
    _ref = dotnetRef;
    _el.addEventListener('wheel', handleWheel, { passive: false });
    const rect = _el.getBoundingClientRect();
    return [rect.width, rect.height];
}

export function dispose() {
    if (_el) _el.removeEventListener('wheel', handleWheel);
    _el = null;
    _ref = null;
}

function handleWheel(e) {
    e.preventDefault();
    const rect = _el.getBoundingClientRect();
    _ref.invokeMethodAsync('HandleWheel',
        e.deltaY,
        e.clientX - rect.left,
        e.clientY - rect.top,
        rect.width,
        rect.height
    );
}
