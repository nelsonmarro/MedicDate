function isSmWindow() {
	const w = window.outerHeight;
	console.log(w);

	if (w < 700) {
		return true;
	}

	return false;
};