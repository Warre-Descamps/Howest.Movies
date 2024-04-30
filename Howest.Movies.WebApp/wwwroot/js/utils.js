window.isAtBottom = function (offset = 100) {
    return window.innerHeight + window.scrollY >= document.body.offsetHeight - offset;
}