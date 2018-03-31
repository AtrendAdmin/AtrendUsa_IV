module.exports = {
    bsName: 'vuvuzela',
    path: {
        build: './build',
        src: './src',
        bower: './bower_components'
    },
    browserSync: {
        logPrefix: 'VUVUZELA',
        ui: false,
        logLevel: 'info',
        server: {
          baseDir: './build',
          directory: true
        }
    },
    backtop: {
        template: {
            "viewports": [
                {
                    "name": "macbook pro 13inch",
                    "width": 1152,
                    "height": 720
                }
            ],
            "paths": {
                "bitmaps_reference": "../../backstop_data/bitmaps_reference",
                "bitmaps_test": "../../backstop_data/bitmaps_test",
                "compare_data": "../../backstop_data/bitmaps_test/compare.json",
                "casper_scripts": "../../backstop_data/casper_scripts"
            },
            "engine": "phantomjs",
            "report": ["CLI", "browser"],
            "cliExitOnFail": false,
            "casperFlags": [],
            "debug": false,
            "port": 3001
        }
    }
}