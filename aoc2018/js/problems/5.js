// https://adventofcode.com/2018/day/5
module.exports = class Solver {
    constructor(number) {
        this.input = require(`./input/${number}`)();
    }

    react(input) {
        let output = input.slice();
        while (true) {
            let i = 0;
            let ri = -1;
            for (; i < output.length - 1; ++i) {
                if (output[i] !== output[i + 1] && output[i].toLowerCase() === output[i + 1].toLowerCase()) {
                    // console.log(`Found unit ${output[i]}${output[i + 1]}`);
                    ri = i;
                    break;
                }
            }
            if (ri !== -1) {
                output.splice(ri, 2);
            } else {
                break;
            }
        }
        return output;
    }

    part1() {
        // return; // Already solved.
        const output = this.react(this.input);
        return output.length;
    }

    part2() {

        // Generate an alphabet.
        let alphabet = 'abcdefghijklmnopqrstuvwxyz';
        alphabet = [...alphabet];

        let shortest = 99999999999999;

        alphabet.forEach(char => {
            const i = this.input.filter(x => x.toLowerCase() !== char);
            const output = this.react(i);
            // console.log(`Removed ${char}${char.toUpperCase()} - Output ${output.length}`);
            if(output.length < shortest){
                shortest = output.length;
            }
        })
        return shortest;
    }
};
