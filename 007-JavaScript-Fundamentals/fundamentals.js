// Lab: JavaScript Fundamentals. Single-line output is returned as a string, multi-line output as an array of lines.

export function sumFirstAndLast(numbers) {
    return numbers[0] + numbers[numbers.length - 1];
}

export function reverseArray(n, numbers) {
    return numbers.slice(0, n).reverse().join(' ');
}

export function evenOddSubtraction(numbers) {
    return numbers.reduce((acc, n) => (n % 2 === 0 ? acc + n : acc - n), 0);
}

export function substring(text, start, count) {
    return text.substring(start, start + count);
}

export function censoredWords(text, word) {
    return text.replaceAll(word, '*'.repeat(word.length));
}

export function countOccurrences(text, word) {
    return text.split(' ').filter(w => w === word).length;
}

export function formatGrade(grade) {
    if (grade < 3) return 'Fail (2)';
    const label = grade < 3.5 ? 'Poor' : grade < 4.5 ? 'Good' : grade < 5.5 ? 'Very good' : 'Excellent';
    return `${label} (${grade.toFixed(2)})`;
}

export function mathPower(base, power) {
    return base ** power;
}

export function repeatString(text, count) {
    return text.repeat(count);
}

const ORDER_PRICES = { coffee: 1.5, water: 1.0, coke: 1.4, snacks: 2.0 };

export function orders(product, quantity) {
    return (ORDER_PRICES[product] * quantity).toFixed(2);
}

export function personInfo(firstName, lastName, age) {
    return { firstName, lastName, age };
}

export function city(cityObject) {
    return Object.entries(cityObject).map(([key, value]) => `${key} -> ${value}`);
}

export function phoneBook(entries) {
    const book = new Map();
    for (const entry of entries) {
        const [name, phone] = entry.split(' ');
        book.set(name, phone);
    }
    return [...book].map(([name, phone]) => `${name} -> ${phone}`);
}
