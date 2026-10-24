import { expect } from 'chai';
import {
    multiplyByTwo, studentInformation, excellentGrade, monthPrinter, mathOperations,
    largestNumber, theatrePromotions, circleArea, numbersOneToFive, numbersFromMToN,
} from './basics.js';

describe('005 JavaScript Basics', () => {
    it('multiplyByTwo doubles the number', () => {
        expect(multiplyByTwo(2)).to.equal(4);
        expect(multiplyByTwo(-1.5)).to.equal(-3);
    });

    it('studentInformation formats the grade to two decimals', () => {
        expect(studentInformation('John', 15, 5.54678)).to.equal('Name: John, Age: 15, Grade: 5.55');
    });

    it('excellentGrade uses 5.50 as the boundary', () => {
        expect(excellentGrade(5.5)).to.equal('Excellent');
        expect(excellentGrade(5.49)).to.equal('Not excellent');
    });

    it('monthPrinter returns the month name or Error!', () => {
        expect(monthPrinter(1)).to.equal('January');
        expect(monthPrinter(12)).to.equal('December');
        expect(monthPrinter(0)).to.equal('Error!');
        expect(monthPrinter(13)).to.equal('Error!');
    });

    it('mathOperations supports all six operators', () => {
        expect(mathOperations(5, 6, '+')).to.equal(11);
        expect(mathOperations(3, 5.5, '-')).to.equal(-2.5);
        expect(mathOperations(3, 4, '*')).to.equal(12);
        expect(mathOperations(9, 2, '/')).to.equal(4.5);
        expect(mathOperations(9, 2, '%')).to.equal(1);
        expect(mathOperations(2, 10, '**')).to.equal(1024);
        expect(() => mathOperations(1, 2, '^')).to.throw('Unknown operator');
    });

    it('largestNumber picks the max', () => {
        expect(largestNumber(5, -3, 16)).to.equal('The largest number is 16.');
        expect(largestNumber(-3, -5, -22.5)).to.equal('The largest number is -3.');
    });

    it('theatrePromotions prices each age group', () => {
        expect(theatrePromotions('Weekday', 42)).to.equal('18$');
        expect(theatrePromotions('Holiday', -12)).to.equal('Error!');
        expect(theatrePromotions('Holiday', 15)).to.equal('5$');
        expect(theatrePromotions('Weekend', 18)).to.equal('15$');
        expect(theatrePromotions('Weekend', 19)).to.equal('20$');
        expect(theatrePromotions('Holiday', 65)).to.equal('10$');
        expect(theatrePromotions('Weekday', 123)).to.equal('Error!');
        expect(theatrePromotions('Monday', 30)).to.equal('Error!');
    });

    it('circleArea computes area or reports the wrong type', () => {
        expect(circleArea(5)).to.equal('78.54');
        expect(circleArea('name')).to.equal('We can not calculate the circle area, because we receive a string.');
    });

    it('numbersOneToFive returns 1..5', () => {
        expect(numbersOneToFive()).to.deep.equal([1, 2, 3, 4, 5]);
    });

    it('numbersFromMToN counts down inclusive', () => {
        expect(numbersFromMToN(6, 2)).to.deep.equal([6, 5, 4, 3, 2]);
        expect(numbersFromMToN(4, 4)).to.deep.equal([4]);
        expect(numbersFromMToN(1, 3)).to.deep.equal([]);
    });
});
