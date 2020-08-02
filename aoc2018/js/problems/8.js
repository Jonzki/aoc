// https://adventofcode.com/2018/day/8
module.exports = class Solver {
    constructor(number) {
        this.input = require(`./input/${number}`)();
        this.currentId = 1;
        this.currentIndex = 0;
        this.nodeValues = {};
    }

    // Constructs a node hierarchy from the input data.
    getNode(input) {
        if (this.currentIndex >= input.length) return null;

        let node = {};
        // Assign id.
        node.id = this.currentId++;

        // Read header.
        // console.log(`Read header for node ${node.id} at index ${this.currentIndex}`);
        node.childCount = input[this.currentIndex++];
        node.metadataCount = input[this.currentIndex++];
        node.children = [];
        node.metadata = [];

        // Read child nodes.
        for (let i = 0; i < node.childCount; ++i) {
            const childNode = this.getNode(input);
            if (childNode) {
                node.children.push(childNode);
            }
        }

        // Read metadata.
        for (let i = 0; i < node.metadataCount; ++i) {
            node.metadata.push(input[this.currentIndex++]);
        }

        return node;
    }

    // Flattens the hierarchical node structure into an array.
    flatten(node) {
        let output = [];
        output.push({
            id: node.id,
            childCount: node.childCount,
            metadataCount: node.metadataCount,
            metadata: node.metadata,
            childIds: node.children.map(c => c.id),
            parentId: node.parentId || null
        });
        for (let i in node.children) {
            node.children[i].parentId = node.id;
            const f = this.flatten(node.children[i]);
            for (let j in f) {
                output.push(f[j]);
            }
        }
        return output;
    }

    getNodeValue(node) {
        // console.log(`Calculating value for node ${node.id}`);
        let val = this.nodeValues[node.id];
        if (typeof val !== 'undefined' && val !== null) {
            // Pre-calculated node value.
            return val;
        }

        if (node.children.length === 0) {
            this.nodeValues[node.id] = node.metadata.reduce((p, c) => p + c, 0);
            return this.nodeValues[node.id];
        } else {
            val = 0;
            for(let i in node.metadata){
                const index = node.metadata[i]-1;
                if(index >= node.children.count){
                    // Indexed outside the childen - value is 0.
                    val = 0;
                    break;
                }
                if(node.children[index]){
                    val += this.getNodeValue(node.children[index]);
                }
            }
            this.nodeValues[node.id] = val;
            return val;
        }
    }

    part1() {
        this.currentId = 1;
        this.currentIndex = 0;
        const rootNode = this.getNode(this.input);
        const flattened = this.flatten(rootNode);

        let sum = 0;
        flattened.forEach(node => {
            sum += node.metadata.reduce((p, c) => p + c, 0);
        });
        return sum;
    }

    part2() {
        this.currentId = 1;
        this.currentIndex = 0;
        const rootNode = this.getNode(this.input);

        // console.log(rootNode);
        
        const value = this.getNodeValue(rootNode);

        return value;
    }
};
