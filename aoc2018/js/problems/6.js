// https://adventofcode.com/2018/day/6
module.exports = class Solver {
    constructor(number) {
        let input = require(`./input/${number}`)();

        this.input = input.map((i, index) => ({
            id: index + 1,
            x: i[0],
            y: i[1]
        }));
    }

    printMap(map, minX, maxX, minY, maxY) {
        // Draw the map from the bounding box.
        console.log('MAP:');
        for (let y = minY - 1; y <= maxY + 1; ++y) {
            let row = '';
            for (let x = minX - 1; x <= maxX + 1; ++x) {
                const key = `${x},${y}`;
                row = row + (map[key] || '#');
            }
            console.log(row);
        }
    }

    part1() {
        // Step 1: find a bounding box. Finite areas must be inside this box.
        let minX = null,
            maxX = null,
            minY = null,
            maxY = null;
        this.input.forEach(c => {
            if (minX === null || c.x < minX) minX = c.x;
            if (maxX === null || c.x > maxX) maxX = c.x;
            if (minY === null || c.y < minY) minY = c.y;
            if (maxY === null || c.y > maxY) maxY = c.y;
        });

        console.log(`Bounding box: (${minX}, ${minY}), (${maxX}, ${maxY}).`);
        let potentialLocations = this.input.filter(c => minX < c.x && c.x < maxX && minY < c.y && c.y < maxY);
        const potentialCount = potentialLocations.length;
        let temp = {};
        potentialLocations.forEach(p => {
            temp[p.id] = p;
        });
        potentialLocations = temp;
        console.log(`Potential locations (${potentialCount}):`);
        console.log(potentialLocations);

        // Build a map to fill distances in.
        let map = {};
        this.input.forEach(c => {
            map[`${c.x},${c.y}`] = c.id;
        });

        // Fill with empty.
        for (let x = minX; x <= maxX; ++x) {
            for (let y = minY; y <= maxY; ++y) {
                const key = `${x},${y}`;
                if (!map[key]) {
                    map[key] = ' ';
                }
            }
        }

        // this.printMap(map, minX, maxX, minY, maxY);

        // Flood-fill from each location while staying within the bounding box.
        let d = 0;
        while (true) {
            let changes = [];
            console.log(`Flood-filling iteration ${++d}.`);

            for (let x = minX; x <= maxX; ++x) {
                for (let y = minY; y <= maxY; ++y) {
                    const key = `${x},${y}`;
                    if (!map[key]) {
                        map[key] = ' ';
                    }

                    if (map[key] === ' ') {
                        // console.log(`Trying to fill ${key}.`);
                        const l = `${x - 1},${y}`;
                        const r = `${x + 1},${y}`;
                        const u = `${x},${y - 1}`;
                        const d = `${x},${y + 1}`;

                        // Scan for nearby items.
                        const ids = [];
                        this.input.forEach(c => {
                            if (map[l] === c.id || map[r] === c.id || map[u] === c.id || map[d] === c.id) {
                                ids.push(c.id);
                            }
                        });

                        if (ids.length === 1) {
                            // Found a single closest location.
                            // console.log('> Fill with ' + ids[0]);
                            changes.push({
                                key,
                                id: ids[0]
                            });
                        } else if (ids.length > 1) {
                            // console.log(`> Multiple matches - fill with '.'`);
                            changes.push({
                                key,
                                id: '.'
                            });
                        } else {
                            // console.log('> Could not fill.');
                        }
                    }
                }
            }
            // console.log(`Map after d${d}:`);
            // console.log(map);

            if (changes.length === 0) {
                // console.log('No changes - finished.');
                break;
            } else {
                changes.forEach(c => {
                    map[c.key] = c.id;
                });
            }
        }

        // Draw the map from the bounding box.
        // this.input.forEach(i => {
        //     map[`${i.x},${i.y}`] = i.id.toUpperCase();
        // });
        // this.printMap(map, minX, maxX, minY, maxY);

        // Resolve area sizes.
        let areas = {};
        Object.values(map).forEach(v => {
            if (potentialLocations[v]) {
                areas[v] = (areas[v] || 0) + 1;
            }
        });
        // console.log('Areas:', areas);

        const largest = Object.values(areas).sort((a, b) => b - a)[0];
        return largest;
    }

    part2() {
        // Manhattan distance calculator.
        const mdist = (a, b) => Math.abs(a.x - b.x) + Math.abs(a.y - b.y);

        // Step 1: find a bounding box. Finite areas must be inside this box.
        let minX = null,
            maxX = null,
            minY = null,
            maxY = null;
        this.input.forEach(c => {
            if (minX === null || c.x < minX) minX = c.x;
            if (maxX === null || c.x > maxX) maxX = c.x;
            if (minY === null || c.y < minY) minY = c.y;
            if (maxY === null || c.y > maxY) maxY = c.y;
        });

        let region = [];

        // Calculate items within the region.
        const distanceLimit = 10000;
        for (let x = minX; x <= maxX; ++x) {
            for (let y = minY; y <= maxY; ++y) {
                let totalDist = 0;
                const loc = { x, y };
                this.input.forEach(i => {
                    totalDist += mdist(loc, i);
                });
                if (totalDist < distanceLimit) {
                    region.push(loc);
                }
            }
        }

        console.log('Region size: ' + region.length);
    }
};
