const solve = number => {
    let Solver;
    try {
        console.log('Looking for solver for problem ' + number);
        Solver = require(`./problems/${number}.js`);
    } catch (error) {
        console.error('Failed:', error);
    }

    if (Solver) {
        console.log('Solving problem ' + number);
        let s = new Solver(number);
        const p1 = s.part1();
        if (p1) {
            console.log('Part1 result:', p1);
        }
        s = new Solver(number);
        const p2 = s.part2();
        if (p2) {
            console.log('Part2 result:', p2);
        }
    }
};

solve(5);
