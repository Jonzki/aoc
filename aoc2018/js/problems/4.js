// https://adventofcode.com/2018/day/4
module.exports = class Solver {
    constructor(number) {
        let rawInput = require(`./input/${number}`)();
        this.input = [];
        // Splits the date and the rest.
        const regex = /\[([^\]]+)\] (.+)/;

        for (let i = 0; i < rawInput.length; ++i) {
            const m = rawInput[i].match(regex);
            rawInput[i] = {
                timestamp: new Date(m[1]),
                text: m[2]
            };
        }
        rawInput = rawInput.sort((a, b) => a.timestamp.getTime() - b.timestamp.getTime());

        // Assign Guard Id, parse event type.
        const guardRegex = /^Guard #(\d+) begins shift$/;
        let id = null;
        for (let i = 0; i < rawInput.length; ++i) {
            const m = rawInput[i].text.match(guardRegex);
            if (m) {
                id = parseInt(m[1]);
            }
            rawInput[i].id = id;
            if (rawInput[i].text.endsWith('begins shift')) {
                rawInput[i].type = 'BEGIN';
            } else if (rawInput[i].text === 'falls asleep') {
                rawInput[i].type = 'SLEEP';
            } else if (rawInput[i].text === 'wakes up') {
                rawInput[i].type = 'WAKEUP';
            }
            delete rawInput[i].text;
        }
        this.input = rawInput;
    }

    solve() {
        this.part1();
        this.part2();
    }

    part1() {
        let guards = {};

        for (let i = 0; i < this.input.length; ++i) {
            const id = this.input[i].id;
            // Ensure guard exists.
            if (!guards[id]) {
                guards[id] = {
                    id: id,
                    totalMinutes: 0,
                    sleepMinutes: {}
                };
            }

            if (i === 0) continue;

            if (this.input[i - 1].type === 'SLEEP' && this.input[i].type === 'WAKEUP') {
                // Found sleep period. Place in proper date.
                const date = `${this.input[i - 1].timestamp.getMonth()}-${this.input[i - 1].timestamp.getDate()}`;
                if (!guards[id].sleepMinutes[date]) {
                    guards[id].sleepMinutes[date] = {};
                }
                guards[id].totalMinutes += this.input[i].timestamp.getMinutes() - this.input[i - 1].timestamp.getMinutes();
                for (let m = this.input[i - 1].timestamp.getMinutes(); m < this.input[i].timestamp.getMinutes(); ++m) {
                    guards[id].sleepMinutes[date][m] = true;
                }
            }
        }

        let guard = null;
        Object.values(guards).forEach(g => {
            if (!guard) {
                guard = g;
            } else {
                if (g.totalMinutes > guard.totalMinutes) {
                    guard = g;
                }
            }
        });

        // Count sleep minutes.
        let minuteCounts = {};
        Object.values(guard.sleepMinutes).forEach(sm => {
            Object.keys(sm).forEach(m => {
                minuteCounts[m] = (minuteCounts[m] || 0) + 1;
            });
        });

        // Find most common minute.
        let minute = null;
        let count = 0;

        Object.keys(minuteCounts).forEach(m => {
            if (minuteCounts[m] > count) {
                minute = m;
                count = minuteCounts[m];
            }
        });

        // console.log(`Chose guard ${guard.id} with ${guard.totalMinutes} total minutes.`);
        // console.log(`Most common minute: ${minute} with ${count} occurrences.`);

        console.log('Part1 Result:', guard.id * parseInt(minute));
    }

    part2() {
        let guards = {};

        for (let i = 0; i < this.input.length; ++i) {
            const id = this.input[i].id;
            // Ensure guard exists.
            if (!guards[id]) {
                guards[id] = {
                    id: id,
                    totalMinutes: 0,
                    sleepMinutes: {}
                };
            }

            if (i === 0) continue;

            if (this.input[i - 1].type === 'SLEEP' && this.input[i].type === 'WAKEUP') {
                // Found sleep period. Place in proper date.
                const date = `${this.input[i - 1].timestamp.getMonth()}-${this.input[i - 1].timestamp.getDate()}`;
                if (!guards[id].sleepMinutes[date]) {
                    guards[id].sleepMinutes[date] = {};
                }
                guards[id].totalMinutes += this.input[i].timestamp.getMinutes() - this.input[i - 1].timestamp.getMinutes();
                for (let m = this.input[i - 1].timestamp.getMinutes(); m < this.input[i].timestamp.getMinutes(); ++m) {
                    guards[id].sleepMinutes[date][m] = true;
                }
            }
        }

        let guardFreq = {};
        Object.values(guards).forEach(guard => {
            // Count sleep minutes.
            let minuteCounts = {};
            Object.values(guard.sleepMinutes).forEach(sm => {
                Object.keys(sm).forEach(m => {
                    minuteCounts[m] = (minuteCounts[m] || 0) + 1;
                });
            });

            // Find most common minute.
            let minute = null;
            let count = 0;

            Object.keys(minuteCounts).forEach(m => {
                if (minuteCounts[m] > count) {
                    minute = m;
                    count = minuteCounts[m];
                }
            });

            guardFreq[guard.id] = {
                id: guard.id,
                minute,
                count
            };
        });

        let guardId = null;
        let minute = null;
        let count = 0;

        Object.values(guardFreq).forEach(gf => {
            if (gf.count > count) {
                guardId = gf.id;
                count = gf.count;
                minute = parseInt(gf.minute);
            }
        });

        console.log(`Part2 Result: ${guardId} x ${minute} = ${guardId * minute}`);
    }
};

console.log(new Date().getDate());
