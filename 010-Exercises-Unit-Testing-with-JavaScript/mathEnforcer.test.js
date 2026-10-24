import { expect } from 'chai';
import { mathEnforcer } from './mathEnforcer.js';

describe('mathEnforcer', () => {
    describe('addFive', () => {
        it('adds five', () => {
            expect(mathEnforcer.addFive(5)).to.equal(10);
            expect(mathEnforcer.addFive(-5)).to.equal(0);
            expect(mathEnforcer.addFive(0.1)).to.be.closeTo(5.1, 0.01);
        });
        it('returns undefined for non-numbers', () => expect(mathEnforcer.addFive('5')).to.be.undefined);
    });
    describe('subtractTen', () => {
        it('subtracts ten', () => {
            expect(mathEnforcer.subtractTen(10)).to.equal(0);
            expect(mathEnforcer.subtractTen(-10)).to.equal(-20);
            expect(mathEnforcer.subtractTen(10.5)).to.be.closeTo(0.5, 0.01);
        });
        it('returns undefined for non-numbers', () => expect(mathEnforcer.subtractTen(null)).to.be.undefined);
    });
    describe('sum', () => {
        it('adds two numbers', () => {
            expect(mathEnforcer.sum(2, 3)).to.equal(5);
            expect(mathEnforcer.sum(-2, -3)).to.equal(-5);
            expect(mathEnforcer.sum(1.1, 2.2)).to.be.closeTo(3.3, 0.01);
        });
        it('returns undefined when either argument is not a number', () => {
            expect(mathEnforcer.sum('2', 3)).to.be.undefined;
            expect(mathEnforcer.sum(2, '3')).to.be.undefined;
        });
    });
});
