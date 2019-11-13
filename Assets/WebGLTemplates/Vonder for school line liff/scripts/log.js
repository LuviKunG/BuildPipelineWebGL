console.log('Cache log error is active.');
var cacheError = console.error;
console.error = function (message, ...optionalParams) {
    window.alert(message);
    optionalParams.forEach(function (optionalParam) {
        window.alert(optionalParam);
    });
    window.alert(optionalParams);
    cacheError.error(message, optionalParams);
};