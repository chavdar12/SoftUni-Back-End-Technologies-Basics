import { expect } from 'chai';
import { lookupChar } from './lookupChar.js';

describe('lookupChar', () => {
    it('returns the character at a valid index', () => {
        expect(lookupChar('hello', 0)).to.equal('h');
        expect(lookupChar('hello', 4)).to.equal('o');
    });
    it('returns undefined for invalid parameter types', () => {
        expect(lookupChar(1, 0)).to.be.undefined;
        expect(lookupChar('hello', '1')).to.be.undefined;
        expect(lookupChar('hello', 1.5)).to.be.undefined;
    });
    it('returns Incorrect index when out of range', () => {
        expect(lookupChar('hello', 5)).to.equal('Incorrect index');
        expect(lookupChar('hello', -1)).to.equal('Incorrect index');
        expect(lookupChar('', 0)).to.equal('Incorrect index');
    });
});
