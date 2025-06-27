module.exports = (req, res, next) => {
    delete req
    next()
}