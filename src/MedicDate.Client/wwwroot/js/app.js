function isSmWindow() {
  const w = window.outerHeight;
  if (w < 700) {
    return true;
  }
  return false;
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
