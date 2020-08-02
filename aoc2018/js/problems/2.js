// https://adventofcode.com/2018/day/2
module.exports = class Solver {
    constructor(number) {
        this.input = require(`./input/${number}`)();
    }

    part1() {
        let twos = 0,
            threes = 0;

        this.input.forEach(input => {
            let counts = {};
            for (let i = 0; i < input.length; ++i) {
                const c = input[i];
                counts[c] = (counts[c] || 0) + 1;
            }

            let a2 = false,
                a3 = false;
            for (let i in counts) {
                if (!a2 && counts[i] === 2) {
                    a2 = true;
                    twos++;
                }
                if (!a3 && counts[i] === 3) {
                    a3 = true;
                    threes++;
                }
            }
        });

        const result = twos * threes;
        return result;
    }

    part2() {
        const diff = (a, b) => {
            // Count each letter of each input.
            let d = 0;
            for (let i = 0; i < a.length; ++i) {
                if (a[i] !== b[i]) ++d;
            }
            return d;
        };

        for(let i = 0; i < this.input.length; ++i){
            for(let j = 0; j < i; ++j){
                const d = diff(this.input[i], this.input[j]);
                if(d === 1){
                    console.log('Correct ids:');
                    console.log(this.input[i]);                    
                    console.log(this.input[j]);
                    return;
                }                
            }
        }
    }
};
