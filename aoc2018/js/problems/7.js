// https://adventofcode.com/2018/day/7
module.exports = class Solver {
    constructor(number) {
        const rawInput = require(`./input/${number}`)();

        let regex = /Step ([A-Z]) must be finished before step ([A-Z]) can begin./;
        this.input = rawInput.map(i => {
            const m = i.match(regex);
            return [m[1], m[2]];
        });
    }

    buildGraph(input) {
        // Construct a graph.
        let graph = {};
        input.forEach(x => {
            // Ensure nodes exist.
            if (!graph[x[0]]) {
                graph[x[0]] = {
                    done: false,
                    prev: [],
                    next: []
                };
            }
            if (!graph[x[1]]) {
                graph[x[1]] = {
                    done: false,
                    prev: [],
                    next: []
                };
            }
            // Fill in the graph.
            graph[x[0]].next.push(x[1]);
            graph[x[1]].prev.push(x[0]);
        });
        return graph;
    }

    getWorkableNodes(graph, condition) {
        // Get ids to process.
        let ids = Object.keys(graph)
            .filter(condition)
            .sort();
        return ids;
    }

    part1() {
        // return;

        let graph = this.buildGraph(this.input);

        // console.log(graph);

        let order = [];
        while (true) {
            // Get ids to process.
            let ids = this.getWorkableNodes(graph, id => !graph[id].done);
            if (ids.length === 0) {
                // console.log('All nodes done.');
                break;
            }

            const potential = [];
            ids.forEach(id => {
                if (graph[id].prev.length === 0) {
                    // Starting node.
                    potential.push(id);
                } else if (graph[id].prev.filter(x => !graph[x].done).length === 0) {
                    // All prerequisites done.
                    potential.push(id);
                }
            });

            if (potential.length > 0) {
                order.push(potential[0]);
                graph[potential[0]].done = true;
            } else {
                // console.warn('No more nodes to complete.');
                break;
            }
        }
        return order.join('');
    }

    part2() {
        // Node work cost: base + charCode (A = 1)
        const baseCost = 60;
        // Amount of workers.
        const workerCount = 5;

        // Start with the same input.
        let graph = this.buildGraph(this.input);
        // Set up work amounts & worker allocations.
        
        Object.keys(graph).forEach(id => {
            graph[id].work = baseCost + id.charCodeAt(0) - 65 + 1;
            delete graph[id].done;
            graph[id].worker = null;
        });

        // Set up workers.
        let workers = [];
        for(let i = 1; i <= workerCount; ++i){
            workers.push({
                id: i,
                node: null
            });
        };

        // console.log(graph);
        // console.log(workers);

        let second = 0;
        while(true){
            // Get nodes with work remaining.
            let ids = this.getWorkableNodes(graph, id => graph[id].work > 0 && !graph.worker);
            if (ids.length === 0) {
                // console.log('All nodes done.');
                break;
            }

            const potential = [];
            // Check for prerequisities.
            ids.forEach(id => {
                let canWork = false;
                if (graph[id].prev.length === 0) {
                    // Starting node.
                    canWork = true;
                } else if (graph[id].prev.filter(x => graph[x].work > 0).length === 0) {
                    // All prerequisites done.
                    canWork = true;
                }

                if(canWork){
                    // Assign a worker for the node (if it's still available).
                    for(let i in workers){
                        if(!workers[i].node && !graph[id].worker){
                            // console.log(`Worker ${workers[i].id} takes node ${id} at ${second}.`);
                            workers[i].node = id;
                            graph[id].worker = workers[i].id;
                        }
                    }
                }
            });

            // Update work amounts & allocations.
            workers.forEach(worker => {
                if(worker.node){
                    // Reduce remaining work by 1.
                    graph[worker.node].work--;
                    // If the node got finished, free up the worker.
                    if(graph[worker.node].work === 0){
                        // console.log(`Worker ${worker.id} finished node ${worker.node} at ${second}.`);
                        worker.node = null;
                    }
                }
            });

            ++second;
        }

        // console.log(`Work completed on second ${second}.`);
        return second;
    }
};
