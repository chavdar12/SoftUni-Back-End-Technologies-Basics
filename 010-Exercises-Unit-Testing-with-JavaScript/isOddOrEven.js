export function isOddOrEven(text) {
    if (typeof text !== 'string') {
        return undefined;
    }
    return text.length % 2 === 0 ? 'even' : 'odd';
}
