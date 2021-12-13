function isSmWindow() {
    const w = window.innerWidth;
    if (w < 700) {
        return true;
    }
    return false;
}

function expandSidebarOnSmScreen() {
    const w = window.innerWidth;
    if (w < 700) {
        const header = document.getElementById('nav-bar');
        header.parentElement.classList.add('header-expanded');
    }
}


function changeBodyContainerHeight() {
    const conteIndex = document.getElementById('conte-index');
    const parentConte = conteIndex.parentElement;
    parentConte.style.height = "100%";
}

function changeBodyContainerHeightToMaxVh() {
    const conteIndex = document.getElementById('conte-index');
    const parentConte = conteIndex.parentElement;
    parentConte.style.height = "100vh";
}

function toggleHeaderExpand() {
    const header = document.getElementById('nav-bar');
    header.parentElement.classList.toggle('header-expanded');
}