export function isSymmetric(arr) {
    if (!Array.isArray(arr)) {
        return false;
    }
    return JSON.stringify(arr) === JSON.stringify([...arr].reverse());
}
