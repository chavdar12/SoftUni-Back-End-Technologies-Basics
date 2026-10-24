// Exercise: JavaScript Basics. Single-line output is returned as a string, multi-line output as an array of lines.

export function ages(age) {
    if (age < 0) return 'out of bounds';
    if (age <= 2) return 'baby';
    if (age <= 13) return 'child';
    if (age <= 19) return 'teenager';
    if (age <= 65) return 'adult';
    return 'elder';
}

const VACATION_PRICES = {
    Students: { Friday: 8.45, Saturday: 9.80, Sunday: 10.46 },
    Business: { Friday: 10.90, Saturday: 15.60, Sunday: 16 },
    Regular: { Friday: 15, Saturday: 20, Sunday: 22.50 },
};

export function vacation(people, groupType, day) {
    const price = VACATION_PRICES[groupType]?.[day];
    if (price === undefined) {
        throw new Error(`Unknown group type or day: ${groupType}, ${day}`);
    }
    let total = people * price;
    if (groupType === 'Students' && people >= 30) {
        total *= 0.85;
    } else if (groupType === 'Business' && people >= 100) {
        total -= 10 * price;
    } else if (groupType === 'Regular' && people >= 10 && people <= 20) {
        total *= 0.95;
    }
    return `Total price: ${total.toFixed(2)}`;
}

export function leapYear(year) {
    return (year % 4 === 0 && year % 100 !== 0) || year % 400 === 0 ? 'yes' : 'no';
}

export function printAndSum(start, end) {
    const numbers = [];
    for (let i = start; i <= end; i++) {
        numbers.push(i);
    }
    const sum = numbers.reduce((acc, n) => acc + n, 0);
    return [numbers.join(' '), `Sum: ${sum}`];
}

export function multiplicationTable(number) {
    return Array.from({ length: 10 }, (_, i) => `${number} X ${i + 1} = ${number * (i + 1)}`);
}

export function sumDigits(number) {
    return [...String(Math.abs(number))].reduce((acc, d) => acc + Number(d), 0);
}

export function charsToString(a, b, c) {
    return a + b + c;
}

export function reversedChars(a, b, c) {
    return `${c} ${b} ${a}`;
}

export function fruit(name, grams, pricePerKg) {
    const kilos = grams / 1000;
    return `I need $${(kilos * pricePerKg).toFixed(2)} to buy ${kilos.toFixed(2)} kilograms ${name}.`;
}

export function sameNumbers(number) {
    const digits = [...String(number)];
    return [digits.every(d => d === digits[0]), digits.reduce((acc, d) => acc + Number(d), 0)];
}

const SPEED_LIMITS = { motorway: 130, interstate: 90, city: 50, residential: 20 };

export function roadRadar(speed, area) {
    const limit = SPEED_LIMITS[area];
    const over = speed - limit;
    if (over <= 0) {
        return `Driving ${speed} km/h in a ${limit} zone`;
    }
    const status = over <= 20 ? 'speeding' : over <= 40 ? 'excessive speeding' : 'reckless driving';
    return `The speed is ${over} km/h faster than the allowed speed of ${limit} - ${status}`;
}

const COOKING_OPERATIONS = {
    chop: n => n / 2,
    dice: n => Math.sqrt(n),
    spice: n => n + 1,
    bake: n => n * 3,
    fillet: n => n * 0.8,
};

export function cookingByNumbers(start, ...operations) {
    let number = Number(start);
    return operations.map(op => (number = COOKING_OPERATIONS[op](number)));
}
