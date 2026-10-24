import { expect } from 'chai';
import { sum } from './sum.js';

describe('sum', () => {
    it('adds numbers', () => expect(sum([1, 2, 3])).to.equal(6));
    it('returns 0 for an empty array', () => expect(sum([])).to.equal(0));
    it('coerces numeric strings', () => expect(sum(['1', 2, '3.5'])).to.equal(6.5));
    it('handles negatives', () => expect(sum([-1, -2, 3])).to.equal(0));
    it('returns NaN for non-numeric strings', () => expect(sum(['a', 1])).to.be.NaN);
});
