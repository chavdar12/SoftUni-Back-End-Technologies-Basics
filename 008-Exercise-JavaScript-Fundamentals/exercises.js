// Exercise: JavaScript Fundamentals. Single-line output is returned as a string, multi-line output as an array of lines.

export function arrayRotation(numbers, rotations) {
    const shift = numbers.length ? rotations % numbers.length : 0;
    return [...numbers.slice(shift), ...numbers.slice(0, shift)].join(' ');
}

export function everyNthElement(items, step) {
    return items.filter((_, i) => i % step === 0);
}

export function listOfNames(names) {
    return [...names]
        .sort((a, b) => a.localeCompare(b))
        .map((name, i) => `${i + 1}.${name}`);
}

export function sortingNumbers(numbers) {
    const sorted = [...numbers].sort((a, b) => a - b);
    const result = [];
    while (sorted.length) {
        result.push(sorted.shift());
        if (sorted.length) result.push(sorted.pop());
    }
    return result;
}

export function revealWords(words, template) {
    const candidates = words.split(', ');
    return template
        .split(' ')
        .map(token => (/^\*+$/.test(token) ? candidates.find(w => w.length === token.length) ?? token : token))
        .join(' ');
}

export function stringSubstring(word, text) {
    const found = text.toLowerCase().split(' ').includes(word.toLowerCase());
    return found ? word : `${word} not found!`;
}

export function smallestOfThree(a, b, c) {
    return Math.min(a, b, c);
}

export function addAndSubtract(a, b, c) {
    const sum = (x, y) => x + y;
    const subtract = (x, y) => x - y;
    return subtract(sum(a, b), c);
}

export function oddAndEvenSum(number) {
    let odd = 0;
    let even = 0;
    for (const digit of String(number)) {
        if (Number(digit) % 2 === 0) even += Number(digit);
        else odd += Number(digit);
    }
    return `Odd sum = ${odd}, Even sum = ${even}`;
}

export function passwordValidator(password) {
    const errors = [];
    if (password.length < 6 || password.length > 10) {
        errors.push('Password must be between 6 and 10 characters');
    }
    if (!/^[A-Za-z0-9]*$/.test(password)) {
        errors.push('Password must consist only of letters and digits');
    }
    if ((password.match(/\d/g) ?? []).length < 2) {
        errors.push('Password must have at least 2 digits');
    }
    return errors.length ? errors : ['Password is valid'];
}

export function employees(names) {
    return names.map(name => `Name: ${name} -- Personal Number: ${name.length}`);
}

export function towns(rows) {
    return rows.map(row => {
        const [town, latitude, longitude] = row.split(' | ');
        return { town, latitude: Number(latitude).toFixed(2), longitude: Number(longitude).toFixed(2) };
    });
}

export function storeProvision(stock, ordered) {
    const store = new Map();
    for (const list of [stock, ordered]) {
        for (let i = 0; i < list.length; i += 2) {
            store.set(list[i], (store.get(list[i]) ?? 0) + Number(list[i + 1]));
        }
    }
    return [...store].map(([product, quantity]) => `${product} -> ${quantity}`);
}

export function movies(commands) {
    const list = [];
    const find = name => list.find(m => m.name === name);
    for (const command of commands) {
        if (command.startsWith('addMovie ')) {
            list.push({ name: command.slice('addMovie '.length) });
        } else if (command.includes(' directedBy ')) {
            const [name, director] = command.split(' directedBy ');
            const movie = find(name);
            if (movie) movie.director = director;
        } else if (command.includes(' onDate ')) {
            const [name, date] = command.split(' onDate ');
            const movie = find(name);
            if (movie) movie.date = date;
        }
    }
    return list.filter(m => m.director && m.date).map(m => JSON.stringify(m));
}

export function inventory(rows) {
    return rows
        .map(row => {
            const [name, level, items] = row.split(' / ');
            return { name, level: Number(level), items };
        })
        .sort((a, b) => a.level - b.level)
        .flatMap(hero => [`Hero: ${hero.name}`, `level => ${hero.level}`, `items => ${hero.items}`]);
}

export function oddOccurrences(text) {
    // A Map keeps first-appearance order; a plain object would move numeric keys like "3" to the front.
    const counts = new Map();
    for (const word of text.toLowerCase().split(' ')) {
        counts.set(word, (counts.get(word) ?? 0) + 1);
    }
    return [...counts].filter(([, count]) => count % 2 !== 0).map(([word]) => word).join(' ');
}

export function piccolo(entries) {
    const parking = new Set();
    for (const entry of entries) {
        const [direction, car] = entry.split(', ');
        if (direction === 'IN') parking.add(car);
        else if (direction === 'OUT') parking.delete(car);
    }
    return parking.size ? [...parking].sort((a, b) => a.localeCompare(b)) : ['Parking Lot is Empty'];
}
