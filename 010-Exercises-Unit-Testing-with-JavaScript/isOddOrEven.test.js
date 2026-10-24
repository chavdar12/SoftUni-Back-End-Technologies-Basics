import { expect } from 'chai';
import { isOddOrEven } from './isOddOrEven.js';

describe('isOddOrEven', () => {
    it('returns undefined for non-strings', () => {
        expect(isOddOrEven(13)).to.be.undefined;
        expect(isOddOrEven({})).to.be.undefined;
    });
    it('returns even for even-length strings', () => {
        expect(isOddOrEven('ab')).to.equal('even');
        expect(isOddOrEven('')).to.equal('even');
    });
    it('returns odd for odd-length strings', () => {
        expect(isOddOrEven('abc')).to.equal('odd');
    });
});
