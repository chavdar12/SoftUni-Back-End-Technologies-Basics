const isChannel = value => Number.isInteger(value) && value >= 0 && value <= 255;

export function rgbToHexColor(red, green, blue) {
    if (![red, green, blue].every(isChannel)) {
        return undefined;
    }
    return '#' + [red, green, blue].map(c => c.toString(16).toUpperCase().padStart(2, '0')).join('');
}
