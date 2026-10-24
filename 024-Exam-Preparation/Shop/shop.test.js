import { expect } from 'chai';
import { shop } from './shop.js';

describe('024 Exam prep: shop', () => {
    it('sells, adds and swaps', () => {
        expect(shop(['5', 'Apple', 'Banana', 'Orange', 'Pineapple', 'Mango', 'Sell', 'Add Peach', 'Swap 0 2', 'End'])).to.deep.equal([
            'Apple product sold!', 'Swapped!', 'Products left: Pineapple, Orange, Banana, Mango, Peach',
        ]);
    });

    it('keeps multi-word product names', () => {
        expect(shop(['0', 'Add Peach Juice', 'End'])).to.deep.equal(['Products left: Peach Juice']);
    });

    it('reports an empty shop and ignores selling from it', () => {
        expect(shop(['1', 'Milk', 'Sell', 'Sell', 'End'])).to.deep.equal(['Milk product sold!', 'The shop is empty']);
    });

    it('ignores invalid swaps', () => {
        expect(shop(['2', 'A', 'B', 'Swap 0 2', 'Swap 1', 'Swap a b', 'End'])).to.deep.equal(['Products left: A, B']);
    });

    it('prints the summary even without End', () => {
        expect(shop(['1', 'A'])).to.deep.equal(['Products left: A']);
    });
});
