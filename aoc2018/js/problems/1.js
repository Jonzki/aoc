// https://adventofcode.com/2018/day/1
module.exports = class Solver {
    constructor(number) {
        this.input = require(`./input/${number}`)();
    }

    part1() {
        return this.input.reduce((sum, value) => sum + value, 0);
    }

    part2() {
        let visits = {};

        let result = 0;
        let run = true;
        while (run) {
            for (let i = 0; run && i < this.input.length; ++i) {
                const x = this.input[i];
                result += x;
                visits[result] = (visits[result] || 0) + 1;
                if (visits[result] >= 2) {
                    run = false;
                }
            }
        }
        return result;
    }
};
