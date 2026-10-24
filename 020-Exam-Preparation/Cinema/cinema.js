// Exam prep 1, JS task: process a box office's commands and return the printed lines.
export function cinema(input) {
    const count = Number(input[0]);
    const movies = input.slice(1, count + 1);
    const output = [];

    for (const line of input.slice(count + 1)) {
        const [command, ...args] = line.split(' ');
        if (command === 'End') {
            break;
        }
        if (command === 'Sell') {
            // The reference solution printed "undefined ticket sold!" when the box office was empty.
            if (movies.length) output.push(`${movies.shift()} ticket sold!`);
        } else if (command === 'Add') {
            const title = args.join(' ');
            if (title) movies.push(title);
        } else if (command === 'Swap') {
            const [i, j] = args.map(Number);
            if (Number.isInteger(i) && Number.isInteger(j) && i >= 0 && j >= 0 && i < movies.length && j < movies.length) {
                [movies[i], movies[j]] = [movies[j], movies[i]];
                output.push('Swapped!');
            }
        }
    }

    output.push(movies.length ? `Tickets left: ${movies.join(', ')}` : 'The box office is empty');
    return output;
}
