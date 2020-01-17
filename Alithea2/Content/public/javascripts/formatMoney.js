function formatNumber(num) {
    return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
}

console.info(formatNumber(2665)) // 2,665
console.info(formatNumber(102665)) // 102,665
console.info(formatNumber(111102665)) // 111,102,665
