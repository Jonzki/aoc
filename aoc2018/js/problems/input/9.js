module.exports = size => {
    if (size === 'small') {
        // 9 players, last marble = 25.
        return [9, 25];
    }
    if (size === 'small2') {
        return [10, 1618];
    }
    if (size === 'small3') {
        return [13, 7999];
    }
    if (size === 'small4') {
        return [17, 1104];
    }
    if (size === 'small5') {
        return [21, 6111];
    }
    if(size === 'small6'){
        return [30, 5807];
    }

    // Actual input.
    return [438, 71626];
};
