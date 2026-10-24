export function createCalculator() {
    let value = 0;
    return {
        add: num => { value += Number(num); },
        subtract: num => { value -= Number(num); },
        get: () => value,
    };
}
