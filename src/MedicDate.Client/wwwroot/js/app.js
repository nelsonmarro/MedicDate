function isSmWindow() {
  const w = window.outerHeight;
  if (w < 700) {
    return true;
  }
  return false;
}
