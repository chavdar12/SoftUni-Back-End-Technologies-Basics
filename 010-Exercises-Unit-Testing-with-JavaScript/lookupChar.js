export function lookupChar(text, index) {
    if (typeof text !== 'string' || !Number.isInteger(index)) {
        return undefined;
    }
    if (index < 0 || index >= text.length) {
        return 'Incorrect index';
    }
    return text.charAt(index);
}
