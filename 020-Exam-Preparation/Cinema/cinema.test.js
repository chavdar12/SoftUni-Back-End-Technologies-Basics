import { expect } from 'chai';
import { cinema } from './cinema.js';

describe('020 Exam prep: cinema', () => {
    it('sells all tickets', () => {
        expect(cinema(['3', 'Star Wars', 'Harry Potter', 'The Hunger Games', 'Sell', 'Sell', 'Sell', 'End'])).to.deep.equal([
            'Star Wars ticket sold!', 'Harry Potter ticket sold!', 'The Hunger Games ticket sold!', 'The box office is empty',
        ]);
    });

    it('adds multi-word titles and swaps', () => {
        expect(cinema(['2', 'Up', 'Cars', 'Add The Lion King', 'Swap 0 2', 'Sell', 'End'])).to.deep.equal([
            'Swapped!', 'The Lion King ticket sold!', 'Tickets left: Cars, Up',
        ]);
    });

    it('ignores invalid swaps and empty adds', () => {
        expect(cinema(['2', 'A', 'B', 'Swap 0 5', 'Swap -1 0', 'Swap x 1', 'Add', 'End'])).to.deep.equal(['Tickets left: A, B']);
    });

    it('does not sell from an empty box office', () => {
        expect(cinema(['0', 'Sell', 'End'])).to.deep.equal(['The box office is empty']);
    });

    it('stops processing at End', () => {
        expect(cinema(['1', 'A', 'End', 'Sell'])).to.deep.equal(['Tickets left: A']);
    });
});
