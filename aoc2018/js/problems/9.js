// https://adventofcode.com/2018/day/9 (template)
module.exports = class Solver {
    constructor(number) {
        this.input = require(`./input/${number}`)('small');
    }

    printTurn(player, marbles, index) {
        const p = marbles.map((m, i) => (i === index ? `(${m})` : m));
        console.log(`[${player}] ${p}`);
    }

    solve(playerCount, lastMarble) {
        console.log(`${playerCount} players; last marble is worth ${lastMarble} points.`);

        // Player id -> score
        const players = {};
        for (let i = 1; i <= playerCount; ++i) {
            players[i] = 0;
        }

        // Marble array
        let index = 0;
        let marbles = [0];
        let marble = 0;
        let player = 1;

        while (true) {
            ++marble;

            if (marble % (lastMarble / 100) === 0) {
                console.log(`${(100 * marble) / lastMarble}% ...`);
            }

            if (marble % 23 === 0) {
                // Add player points for the marble.
                players[player] += marble;
                // Add points from the -7 marble.
                index = index - 7;
                if (index < 0) {
                    index += marbles.length;
                }
                index = index % marbles.length;

                players[player] += marbles[index];
                // Remove the -7 marble.
                marbles.splice(index, 1);
            } else {
                // Place the marble.
                index = index + 2;
                if (index > marbles.length) {
                    index -= marbles.length;
                }
                marbles.splice(index, 0, marble);
            }

            // this.printTurn(player, marbles, index);

            if (marble === lastMarble) {
                // console.log('Last marble placed.');
                break;
            }

            player++;
            if (player > playerCount) player = 1;
        }

        // console.log(players);

        let winningPlayer = 0;
        let winningScore = 0;
        Object.keys(players).forEach(p => {
            if (players[p] > winningScore) {
                winningPlayer = p;
                winningScore = players[p];
            }
        });

        console.log(`Player ${winningPlayer} wins with a score of ${winningScore}.`);
        return winningScore;
    }

    solve2(playerCount, lastMarble) {
        console.log(`${playerCount} players; last marble is worth ${lastMarble} points.`);

        // Player id -> score
        const players = {};
        for (let i = 1; i <= playerCount; ++i) {
            players[i] = 0;
        }

        let index = 0;
        let marbles = [0];
        let marble = 1;
        let player = 1;

        while (true) {
            if (marble >= lastMarble) {
                // console.log('Last marble placed.');
                break;
            }

            for (;(marble + 1) % 23 != 0; ++marble) {
                console.log(`Place marble ${marble}`);
                // Place the marble.
                index = index + 2;
                if (index > marbles.length) {
                    index -= marbles.length;
                }
                marbles.splice(index, 0, marble);
                player++;
                if (player > playerCount) player = 1;
            }
            ++marble;
            console.log(`KEEP marble ${marble}`);
            // Add player points for the marble.
            players[player] += marble;
            // Add points from the -7 marble.
            index = index - 7;
            if (index < 0) {
                index += marbles.length;
            }
            index = index % marbles.length;

            players[player] += marbles[index];
            // Remove the -7 marble.
            marbles.splice(index, 1);

            ++marble;

            if (marble === lastMarble) {
                // console.log('Last marble placed.');
                break;
            }

            player++;
            if (player > playerCount) player = 1;
        }

        // console.log(players);

        let winningPlayer = 0;
        let winningScore = 0;
        Object.keys(players).forEach(p => {
            if (players[p] > winningScore) {
                winningPlayer = p;
                winningScore = players[p];
            }
        });

        console.log(`Player ${winningPlayer} wins with a score of ${winningScore}.`);
        return winningScore;
    }

    part1() {
        const playerCount = this.input[0];
        const lastMarble = this.input[1];
        return this.solve2(playerCount, lastMarble);
    }

    part2() {
        return;
        const playerCount = this.input[0];
        const lastMarble = this.input[1] * 100;
        return this.solve(playerCount, lastMarble);
    }
};
