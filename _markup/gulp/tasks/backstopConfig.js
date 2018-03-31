var gulp = require('gulp');
var fs = require('fs');
var config = require('../config').backtop;
var chalk = require('chalk');

gulp.task('backstopConfig', function() {
    console.log('• Backtop Config Generator •');

    function getFileList(dir) {
        console.log('> Scanning directory: ' + dir);
        
        var r = /\.(html)$/i;
        var listing = fs.readdirSync(dir);

        return listing.filter(function(file) {
            return file.charAt(0) != '_' && r.test(file);
        });
    };

    function genScenario(file, host) {
        return {
            "label": file,
            "url": host + file,
            "selectors": [
                "body"
            ],
            "hideSelectors": [
                "#__bs_notify__"
            ],
            "readyEvent": null,
            "delay": 1000,
            "misMatchThreshold" : 0.1,
            "onBeforeScript": "onBefore.js",
            "onReadyScript": "onReady.js"
        };
    };

    var fileArray = getFileList('build');
    var host = 'localhost:3000/';
    var scenarios = [];

    var config = config.template;

    if(fileArray.length) {
        for(var i = 0; i < fileArray.length; i++) {
            scenarios.push(genScenario(fileArray[i], host));
        }

        config['scenarios'] = scenarios;

        fs.writeFileSync('backtop.json', JSON.stringify(config));

        console.log(scenarios);
    } else {
        console.log('Hey! I need more HTML files!')
    }
});