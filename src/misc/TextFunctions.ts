export function capitalizeFirstLetter(str: string) {
	return str.charAt(0).toUpperCase() + str.slice(1);
}

export function addSpaceBeforeCapitalLetters(str: string) {
	// Use a regular expression to match capital letters
	// and insert a space before them using replace
	return str.replace(/([A-Z])/g, ' $1');
}