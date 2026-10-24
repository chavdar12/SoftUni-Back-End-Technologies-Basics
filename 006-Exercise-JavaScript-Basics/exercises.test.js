import { expect } from 'chai';
import {
    ages, vacation, leapYear, printAndSum, multiplicationTable, sumDigits, charsToString,
    reversedChars, fruit, sameNumbers, roadRadar, cookingByNumbers,
} from './exercises.js';

describe('006 Exercise: JavaScript Basics', () => {
    it('ages classifies every boundary', () => {
        expect(ages(-1)).to.equal('out of bounds');
        expect(ages(0)).to.equal('baby');
        expect(ages(2)).to.equal('baby');
        expect(ages(3)).to.equal('child');
        expect(ages(13)).to.equal('child');
        expect(ages(14)).to.equal('teenager');
        expect(ages(19)).to.equal('teenager');
        expect(ages(20)).to.equal('adult');
        expect(ages(65)).to.equal('adult');
        expect(ages(66)).to.equal('elder');
    });

    it('vacation applies group discounts', () => {
        expect(vacation(30, 'Students', 'Sunday')).to.equal('Total price: 266.73');
        expect(vacation(40, 'Regular', 'Saturday')).to.equal('Total price: 800.00');
        expect(vacation(10, 'Regular', 'Friday')).to.equal('Total price: 142.50');
        expect(vacation(100, 'Business', 'Friday')).to.equal('Total price: 981.00');
        expect(vacation(29, 'Students', 'Friday')).to.equal('Total price: 245.05');
        expect(() => vacation(1, 'Students', 'Monday')).to.throw();
    });

    it('leapYear follows the Gregorian rules', () => {
        expect(leapYear(1984)).to.equal('yes');
        expect(leapYear(2003)).to.equal('no');
        expect(leapYear(1900)).to.equal('no');
        expect(leapYear(2000)).to.equal('yes');
    });

    it('printAndSum lists the range and its sum', () => {
        expect(printAndSum(5, 10)).to.deep.equal(['5 6 7 8 9 10', 'Sum: 45']);
        expect(printAndSum(3, 3)).to.deep.equal(['3', 'Sum: 3']);
    });

    it('multiplicationTable prints 10 rows', () => {
        const table = multiplicationTable(5);
        expect(table).to.have.length(10);
        expect(table[0]).to.equal('5 X 1 = 5');
        expect(table[9]).to.equal('5 X 10 = 50');
    });

    it('sumDigits adds all digits', () => {
        expect(sumDigits(245678)).to.equal(32);
        expect(sumDigits(97561)).to.equal(28);
        expect(sumDigits(0)).to.equal(0);
    });

    it('charsToString and reversedChars combine characters', () => {
        expect(charsToString('a', 'b', 'c')).to.equal('abc');
        expect(reversedChars('A', 'B', 'C')).to.equal('C B A');
    });

    it('fruit calculates the money needed', () => {
        expect(fruit('orange', 2500, 1.80)).to.equal('I need $4.50 to buy 2.50 kilograms orange.');
        expect(fruit('apple', 1563, 2.35)).to.equal('I need $3.67 to buy 1.56 kilograms apple.');
    });

    it('sameNumbers checks digits and sums them', () => {
        expect(sameNumbers(2222222)).to.deep.equal([true, 14]);
        expect(sameNumbers(1234)).to.deep.equal([false, 10]);
        expect(sameNumbers(7)).to.deep.equal([true, 7]);
    });

    it('roadRadar reports the speeding status', () => {
        expect(roadRadar(40, 'city')).to.equal('Driving 40 km/h in a 50 zone');
        expect(roadRadar(21, 'residential')).to.equal('The speed is 1 km/h faster than the allowed speed of 20 - speeding');
        expect(roadRadar(120, 'interstate')).to.equal('The speed is 30 km/h faster than the allowed speed of 90 - excessive speeding');
        expect(roadRadar(200, 'motorway')).to.equal('The speed is 70 km/h faster than the allowed speed of 130 - reckless driving');
    });

    it('cookingByNumbers applies each operation in order', () => {
        expect(cookingByNumbers('32', 'chop', 'chop', 'chop', 'chop', 'chop')).to.deep.equal([16, 8, 4, 2, 1]);
        const result = cookingByNumbers('9', 'dice', 'spice', 'chop', 'bake', 'fillet');
        expect(result.slice(0, 4)).to.deep.equal([3, 4, 2, 6]);
        expect(result[4]).to.be.closeTo(4.8, 1e-9);
    });
});
