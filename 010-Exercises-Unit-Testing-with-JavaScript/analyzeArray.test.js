import { expect } from 'chai';
import { analyzeArray } from './analyzeArray.js';

describe('analyzeArray', () => {
    it('returns min, max and length', () => {
        expect(analyzeArray([3, -1, 10, 4.5])).to.deep.equal({ min: -1, max: 10, length: 4 });
    });
    it('handles a single element', () => {
        expect(analyzeArray([7])).to.deep.equal({ min: 7, max: 7, length: 1 });
    });
    it('returns undefined for empty or non-array input', () => {
        expect(analyzeArray([])).to.be.undefined;
        expect(analyzeArray('1,2')).to.be.undefined;
        expect(analyzeArray(undefined)).to.be.undefined;
    });
    it('returns undefined when an element is not a number', () => {
        expect(analyzeArray([1, '2', 3])).to.be.undefined;
    });
});
