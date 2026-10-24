// Exam prep 2, JS task: process shop commands and return the printed lines.
export function shop(input) {
    const count = Number(input[0]);
    const products = input.slice(1, count + 1);
    const output = [];

    for (const line of input.slice(count + 1)) {
        const [command, ...args] = line.split(' ');
        if (command === 'End') {
            break;
        }
        if (command === 'Sell') {
            if (products.length) output.push(`${products.shift()} product sold!`);
        } else if (command === 'Add') {
            // The reference solution kept only the first word, so "Add Peach Juice" added "Peach".
            const product = args.join(' ');
            if (product) products.push(product);
        } else if (command === 'Swap') {
            const [i, j] = args.map(Number);
            if (Number.isInteger(i) && Number.isInteger(j) && i >= 0 && j >= 0 && i < products.length && j < products.length) {
                [products[i], products[j]] = [products[j], products[i]];
                output.push('Swapped!');
            }
        }
    }

    // Also print the summary when input has no End command, which the reference solution skipped.
    output.push(products.length ? `Products left: ${products.join(', ')}` : 'The shop is empty');
    return output;
}
