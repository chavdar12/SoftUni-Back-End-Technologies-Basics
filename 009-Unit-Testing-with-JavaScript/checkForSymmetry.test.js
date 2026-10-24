import { expect } from 'chai';
import { isSymmetric } from './checkForSymmetry.js';

describe('isSymmetric', () => {
    it('returns true for symmetric arrays', () => {
        expect(isSymmetric([1, 2, 3, 2, 1])).to.be.true;
        expect(isSymmetric([5, 'hi', { a: 5 }, new Date(1), { a: 5 }, 'hi', 5])).to.be.true;
    });
    it('returns true for empty and single-element arrays', () => {
        expect(isSymmetric([])).to.be.true;
        expect(isSymmetric([1])).to.be.true;
    });
    it('returns false for non-symmetric arrays', () => {
        expect(isSymmetric([1, 2])).to.be.false;
        expect(isSymmetric([1, 2, '1'])).to.be.false;
    });
    it('returns false for non-arrays', () => {
        expect(isSymmetric('aba')).to.be.false;
        expect(isSymmetric(null)).to.be.false;
        expect(isSymmetric({ 0: 1, length: 1 })).to.be.false;
    });
    it('does not mutate the input', () => {
        const arr = [1, 2, 3];
        isSymmetric(arr);
        expect(arr).to.deep.equal([1, 2, 3]);
    });
});
