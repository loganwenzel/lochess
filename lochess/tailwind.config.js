module.exports = {
    purge: {
        enabled: true,
        content: [
            './Views/**/*.cshtml'
        ]
    },
    darkMode: false, // or 'media' or 'class'
    theme: {
        extend: {
            fontFamily: {
                'montserrat': ['Montserrat', 'sans-serif']
            },
        },
    },
    variants: {
        extend: {},
    },
    plugins: [],
}