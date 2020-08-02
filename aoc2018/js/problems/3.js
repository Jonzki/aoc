// https://adventofcode.com/2018/day/3
module.exports = class Solver {
    constructor(number) {
        const rawInput = require(`./input/${number}`)();
        this.input = [];
        const regex = /#(\d+) @ (\d+),(\d+): (\d+)x(\d+)/;
        rawInput.forEach(i => {
            const m = i.match(regex);
            this.input.push({
                id: parseInt(m[1]),
                x: parseInt(m[2]),
                y: parseInt(m[3]),
                w: parseInt(m[4]),
                h: parseInt(m[5])
            });
        });
    }

    solve() {
        // this.part1();
        this.part2();
    }

    getClaimCounts(input) {
        const counts = {};
        input.forEach(i => {
            for (let x = 0; x < i.w; ++x) {
                for (let y = 0; y < i.h; ++y) {
                    let pos = `${i.x + x}x${i.y + y}`;
                    if(!counts[pos]){
                        counts[pos] = [];
                    }
                    counts[pos].push(i.id);
                }
            }
        });
        return counts;
    }

    part1() {
        const counts = this.getClaimCounts(this.input);
        const result = Object.values(counts).filter(x => x.length > 1).length;
        console.log('Part 1 result: ' + result);
    }

    part2() {
        // Get claim counts.
        const counts = this.getClaimCounts(this.input);
        // Collect all ids into an object.
        let ids = {};
        this.input.forEach(i => ids[i.id] = true);

        const collisions = Object.values(counts).filter(x => x.length > 1);
        collisions.forEach(collisionIds => {
            collisionIds.forEach(cid => {
                delete(ids[cid]);
            });
        });
        console.log('Part 2 result: ' + Object.keys(ids));
    }
};
