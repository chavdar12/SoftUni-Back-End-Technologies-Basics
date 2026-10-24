// Lab: JavaScript Basics. Each task returns its output instead of printing it, so it can be unit tested.

export function multiplyByTwo(number) {
    return number * 2;
}

export function studentInformation(name, age, grade) {
    return `Name: ${name}, Age: ${age}, Grade: ${grade.toFixed(2)}`;
}

export function excellentGrade(grade) {
    return grade >= 5.5 ? 'Excellent' : 'Not excellent';
}

const MONTHS = ['January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December'];

export function monthPrinter(month) {
    return MONTHS[month - 1] ?? 'Error!';
}

export function mathOperations(a, b, operator) {
    switch (operator) {
        case '+': return a + b;
        case '-': return a - b;
        case '*': return a * b;
        case '/': return a / b;
        case '%': return a % b;
        case '**': return a ** b;
        default: throw new Error(`Unknown operator: ${operator}`);
    }
}

export function largestNumber(a, b, c) {
    return `The largest number is ${Math.max(a, b, c)}.`;
}

const TICKET_PRICES = {
    Weekday: { child: 12, adult: 18, senior: 12 },
    Weekend: { child: 15, adult: 20, senior: 15 },
    Holiday: { child: 5, adult: 12, senior: 10 },
};

export function theatrePromotions(dayType, age) {
    const prices = TICKET_PRICES[dayType];
    if (!prices || age < 0 || age > 122) {
        return 'Error!';
    }
    const group = age <= 18 ? 'child' : age <= 64 ? 'adult' : 'senior';
    return `${prices[group]}$`;
}

export function circleArea(radius) {
    if (typeof radius !== 'number') {
        return `We can not calculate the circle area, because we receive a ${typeof radius}.`;
    }
    return (Math.PI * radius ** 2).toFixed(2);
}

export function numbersOneToFive() {
    return [1, 2, 3, 4, 5];
}

export function numbersFromMToN(m, n) {
    const result = [];
    for (let i = m; i >= n; i--) {
        result.push(i);
    }
    return result;
}
