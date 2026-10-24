export function analyzeArray(arr) {
    if (!Array.isArray(arr) || arr.length === 0 || !arr.every(n => typeof n === 'number')) {
        return undefined;
    }
    return { min: Math.min(...arr), max: Math.max(...arr), length: arr.length };
}
