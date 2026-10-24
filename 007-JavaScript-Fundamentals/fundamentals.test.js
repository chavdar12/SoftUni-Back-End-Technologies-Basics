import { expect } from 'chai';
import {
    sumFirstAndLast, reverseArray, evenOddSubtraction, substring, censoredWords, countOccurrences,
    formatGrade, mathPower, repeatString, orders, personInfo, city, phoneBook,
} from './fundamentals.js';

describe('007 JavaScript Fundamentals', () => {
    it('sumFirstAndLast adds the edges', () => {
        expect(sumFirstAndLast([20, 30, 40])).to.equal(60);
        expect(sumFirstAndLast([5])).to.equal(10);
    });

    it('reverseArray takes the first n and reverses them', () => {
        expect(reverseArray(3, [10, 20, 30, 40, 50])).to.equal('30 20 10');
        expect(reverseArray(4, [-1, 20, 99, 5])).to.equal('5 99 20 -1');
    });

    it('evenOddSubtraction returns even sum minus odd sum', () => {
        expect(evenOddSubtraction([1, 2, 3, 4, 5, 6])).to.equal(3);
        expect(evenOddSubtraction([3, 5, 7, 9])).to.equal(-24);
        expect(evenOddSubtraction([2, 4, 6, 8, 10])).to.equal(30);
    });

    it('substring extracts count characters', () => {
        expect(substring('ASentence', 1, 8)).to.equal('Sentence');
        expect(substring('SkipWord', 4, 7)).to.equal('Word');
    });

    it('censoredWords replaces every occurrence', () => {
        expect(censoredWords('A small sentence with some words', 'small')).to.equal('A ***** sentence with some words');
        expect(censoredWords('Find the hidden word, the hidden one', 'hidden')).to.equal('Find the ****** word, the ****** one');
    });

    it('countOccurrences counts whole words only', () => {
        expect(countOccurrences('This is a word and it also is a sentence', 'is')).to.equal(2);
        expect(countOccurrences('softuni is great place for learning new programming languages', 'softuni')).to.equal(1);
    });

    it('formatGrade labels each range', () => {
        expect(formatGrade(2.99)).to.equal('Fail (2)');
        expect(formatGrade(3.33)).to.equal('Poor (3.33)');
        expect(formatGrade(3.5)).to.equal('Good (3.50)');
        expect(formatGrade(4.5)).to.equal('Very good (4.50)');
        expect(formatGrade(5.5)).to.equal('Excellent (5.50)');
    });

    it('mathPower raises to the power', () => {
        expect(mathPower(2, 8)).to.equal(256);
        expect(mathPower(3, 4)).to.equal(81);
        expect(mathPower(5, 0)).to.equal(1);
    });

    it('repeatString repeats the text', () => {
        expect(repeatString('abc', 3)).to.equal('abcabcabc');
        expect(repeatString('String', 0)).to.equal('');
    });

    it('orders prices each product', () => {
        expect(orders('coffee', 2)).to.equal('3.00');
        expect(orders('water', 5)).to.equal('5.00');
        expect(orders('coke', 3)).to.equal('4.20');
        expect(orders('snacks', 1)).to.equal('2.00');
    });

    it('personInfo builds an object', () => {
        expect(personInfo('Peter', 'Pan', 20)).to.deep.equal({ firstName: 'Peter', lastName: 'Pan', age: 20 });
    });

    it('city lists every property', () => {
        expect(city({ name: 'Sofia', area: 492, postCode: '1000' }))
            .to.deep.equal(['name -> Sofia', 'area -> 492', 'postCode -> 1000']);
    });

    it('phoneBook keeps the last number per name in first-seen order', () => {
        expect(phoneBook(['Tim 0834212554', 'Peter 0877547887', 'Bill 0896543112', 'Tim 0876566344']))
            .to.deep.equal(['Tim -> 0876566344', 'Peter -> 0877547887', 'Bill -> 0896543112']);
    });
});
