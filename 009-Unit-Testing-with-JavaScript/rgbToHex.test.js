import { expect } from 'chai';
import { rgbToHexColor } from './rgbToHex.js';

describe('rgbToHexColor', () => {
    it('converts valid colors with zero padding', () => {
        expect(rgbToHexColor(255, 158, 170)).to.equal('#FF9EAA');
        expect(rgbToHexColor(12, 13, 14)).to.equal('#0C0D0E');
        expect(rgbToHexColor(0, 0, 0)).to.equal('#000000');
        expect(rgbToHexColor(255, 255, 255)).to.equal('#FFFFFF');
    });
    it('returns undefined for out-of-range values', () => {
        expect(rgbToHexColor(-1, 0, 0)).to.be.undefined;
        expect(rgbToHexColor(0, 256, 0)).to.be.undefined;
        expect(rgbToHexColor(0, 0, 300)).to.be.undefined;
    });
    it('returns undefined for non-integers and missing values', () => {
        expect(rgbToHexColor(3.14, 0, 0)).to.be.undefined;
        expect(rgbToHexColor('5', [3], { 8: 9 })).to.be.undefined;
        expect(rgbToHexColor()).to.be.undefined;
    });
});
