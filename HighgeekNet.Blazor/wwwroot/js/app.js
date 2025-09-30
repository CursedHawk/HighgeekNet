function blazorGetTimezoneOffset() {
    return new Date().getTimezoneOffset();
}

function scrollToBottom(name) {
    document.getElementsByClassName(name)[0].scrollTop = document.getElementsByClassName(name)[0].scrollHeight;
}

function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}
