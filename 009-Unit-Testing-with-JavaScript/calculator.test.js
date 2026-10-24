import { expect } from 'chai';
import { createCalculator } from './calculator.js';

describe('createCalculator', () => {
    let calc;
    beforeEach(() => { calc = createCalculator(); });

    it('starts at 0', () => expect(calc.get()).to.equal(0));
    it('adds and subtracts', () => {
        calc.add(10);
        calc.subtract(3);
        calc.add(-2);
        expect(calc.get()).to.equal(5);
    });
    it('parses numeric strings', () => {
        calc.add('7');
        calc.subtract('2.5');
        expect(calc.get()).to.equal(4.5);
    });
    it('returns NaN for non-numeric input', () => {
        calc.add('hello');
        expect(calc.get()).to.be.NaN;
    });
    it('keeps state per instance', () => {
        const other = createCalculator();
        calc.add(1);
        expect(other.get()).to.equal(0);
    });
});
