import { expect } from 'chai';
import {
    arrayRotation, everyNthElement, listOfNames, sortingNumbers, revealWords, stringSubstring,
    smallestOfThree, addAndSubtract, oddAndEvenSum, passwordValidator, employees, towns,
    storeProvision, movies, inventory, oddOccurrences, piccolo,
} from './exercises.js';

describe('008 Exercise: JavaScript Fundamentals', () => {
    it('arrayRotation moves elements from front to back', () => {
        expect(arrayRotation([51, 47, 32, 61, 21], 2)).to.equal('32 61 21 51 47');
        expect(arrayRotation([32, 21, 61, 1], 4)).to.equal('32 21 61 1');
        expect(arrayRotation([2, 4, 15, 31], 5)).to.equal('4 15 31 2');
        expect(arrayRotation([], 3)).to.equal('');
    });

    it('everyNthElement takes every step-th item', () => {
        expect(everyNthElement(['5', '20', '31', '4', '20'], 2)).to.deep.equal(['5', '31', '20']);
        expect(everyNthElement(['1', '2', '3', '4', '5'], 6)).to.deep.equal(['1']);
    });

    it('listOfNames sorts and numbers the names without mutating input', () => {
        const names = ['John', 'Bob', 'Christina', 'Ema'];
        expect(listOfNames(names)).to.deep.equal(['1.Bob', '2.Christina', '3.Ema', '4.John']);
        expect(names[0]).to.equal('John');
    });

    it('sortingNumbers alternates smallest and largest', () => {
        expect(sortingNumbers([1, 65, 3, 52, 48, 63, 31, -3, 18, 56]))
            .to.deep.equal([-3, 65, 1, 63, 3, 56, 18, 52, 31, 48]);
        expect(sortingNumbers([2, 1, 3])).to.deep.equal([1, 3, 2]);
    });

    it('revealWords fills in the star placeholders', () => {
        expect(revealWords('great, learning', 'softuni is ***** place for ******** new programming languages'))
            .to.equal('softuni is great place for learning new programming languages');
    });

    it('stringSubstring searches case-insensitively', () => {
        expect(stringSubstring('javascript', 'JavaScript is the best programming language')).to.equal('javascript');
        expect(stringSubstring('Python', 'python is fine')).to.equal('Python');
        expect(stringSubstring('python', 'JavaScript is the best programming language')).to.equal('python not found!');
    });

    it('smallestOfThree and addAndSubtract', () => {
        expect(smallestOfThree(600, 342, 123)).to.equal(123);
        expect(addAndSubtract(23, 6, 10)).to.equal(19);
        expect(addAndSubtract(1, 17, 30)).to.equal(-12);
    });

    it('oddAndEvenSum splits digits by parity', () => {
        expect(oddAndEvenSum(1000435)).to.equal('Odd sum = 9, Even sum = 4');
        expect(oddAndEvenSum(3495892137259234)).to.equal('Odd sum = 54, Even sum = 22');
    });

    it('passwordValidator reports every broken rule', () => {
        expect(passwordValidator('MyPass123')).to.deep.equal(['Password is valid']);
        expect(passwordValidator('logIn')).to.deep.equal([
            'Password must be between 6 and 10 characters',
            'Password must have at least 2 digits',
        ]);
        expect(passwordValidator('Pa$s$s')).to.deep.equal([
            'Password must consist only of letters and digits',
            'Password must have at least 2 digits',
        ]);
    });

    it('employees uses name length as personal number', () => {
        expect(employees(['Silas Butler'])).to.deep.equal(['Name: Silas Butler -- Personal Number: 12']);
    });

    it('towns parses and rounds coordinates', () => {
        expect(towns(['Sofia | 42.696552 | 23.32601'])).to.deep.equal([{ town: 'Sofia', latitude: '42.70', longitude: '23.33' }]);
    });

    it('storeProvision merges stock and orders', () => {
        expect(storeProvision(['Chips', '5', 'Pasta', '4'], ['Flour', '44', 'Pasta', '7']))
            .to.deep.equal(['Chips -> 5', 'Pasta -> 11', 'Flour -> 44']);
    });

    it('movies prints only movies with name, director and date', () => {
        expect(movies([
            'addMovie Fast and Furious',
            'addMovie Godfather',
            'Inception directedBy Christopher Nolan',
            'Godfather directedBy Francis Ford Coppola',
            'Godfather onDate 29.07.2018',
            'Fast and Furious onDate 30.07.2018',
        ])).to.deep.equal(['{"name":"Godfather","director":"Francis Ford Coppola","date":"29.07.2018"}']);
    });

    it('inventory sorts heroes by level', () => {
        expect(inventory(['Isacc / 25 / Apple, GravityGun', 'Hes / 1 / Desolator, Sentinel, Antara'])).to.deep.equal([
            'Hero: Hes', 'level => 1', 'items => Desolator, Sentinel, Antara',
            'Hero: Isacc', 'level => 25', 'items => Apple, GravityGun',
        ]);
    });

    it('oddOccurrences keeps first-appearance order, including numeric words', () => {
        expect(oddOccurrences('Java C# Php PHP Java PhP 3 C#3 1 5 C#')).to.equal('php 3 c#3 1 5');
        expect(oddOccurrences('Cake IS SWEET is Soft CAKE sweet Food')).to.equal('soft food');
    });

    it('piccolo lists parked cars or reports empty', () => {
        expect(piccolo(['IN, CA2844AA', 'IN, CA1234TA', 'OUT, CA2844AA', 'IN, CA9999TT'])).to.deep.equal(['CA1234TA', 'CA9999TT']);
        expect(piccolo(['IN, CA2844AA', 'OUT, CA2844AA'])).to.deep.equal(['Parking Lot is Empty']);
    });
});
