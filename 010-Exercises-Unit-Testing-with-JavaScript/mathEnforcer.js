const isNumber = value => typeof value === 'number';

export const mathEnforcer = {
    addFive: num => (isNumber(num) ? num + 5 : undefined),
    subtractTen: num => (isNumber(num) ? num - 10 : undefined),
    sum: (a, b) => (isNumber(a) && isNumber(b) ? a + b : undefined),
};
