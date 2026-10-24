export function sum(numbers) {
    let total = 0;
    for (const n of numbers) {
        total += Number(n);
    }
    return total;
}
