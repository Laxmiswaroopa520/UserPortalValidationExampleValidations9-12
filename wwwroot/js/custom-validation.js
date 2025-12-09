// MIN AGE
$.validator.addMethod("Minage", function (value, element, param) {
    if (!value) return false;                   //if input is null
    return parseInt(value) >= parseInt(param);
});
$.validator.unobtrusive.adapters.addSingleVal("Minage", "age");

// NO SEQUENTIAL DIGITS
$.validator.addMethod("nosequential", function (value) {
    if (!value) return true;
    return !(value.includes("1234") || value.includes("4567") || value.includes("7890"));
});
$.validator.unobtrusive.adapters.addBool("nosequential");

// PHONE NUMBER
$.validator.addMethod("phonenumber", function (value) {
    if (!value) return true;
    return /^[6-9]\d{9}$/.test(value);
});
$.validator.unobtrusive.adapters.addBool("phonenumber");


//PastDate Validation 
$.validator.addMethod("pastdate", function (value, element, params) {
    if (!value) return false;

    let inputDate = new Date(value);
    let today = new Date(params.today);

    return inputDate <= today;
});

$.validator.unobtrusive.adapters.add("pastdate", ["today"], function (options) {
    options.rules["pastdate"] = { today: options.params.today };
    options.messages["pastdate"] = options.message;
});

//MustBeTrue Validation
$.validator.addMethod("mustbetrue", function (value, element) {
    return element.checked === true;
});

$.validator.unobtrusive.adapters.addBool("mustbetrue");












/* 1. Register adapter

jQuery.validator.unobtrusive.adapters.add("notadminusername", [], function (options) {
    options.rules["notadminusername"] = true;
    options.messages["notadminusername"] = options.message;
});


// 2. Implement the rule
jQuery.validator.addMethod("notadminusername", function (value, element) {
    if (!value) return true;
    return value.toLowerCase() !== "admin";
});
*/
console.log("Custom Validation Loaded");

$.validator.addMethod("notadminusername", function (value, element) {
    if (!value) return true;
    return value.toLowerCase() !== "admin";
});

$.validator.unobtrusive.adapters.addBool("notadminusername");


// Email Validation
// -----------------------------
// VALID EMAIL
// -----------------------------

    // Same regex as server-side
    // var pattern = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(\.[a-zA-Z]{2,10})+$/;
    $.validator.addMethod("validemail", function (value) {
        if (!value) return true;

        var pattern = /^(?:[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*)@[A-Za-z0-9-]+(?:\.[A-Za-z]{2,10})+$/;

        return pattern.test(value);
    });

    $.validator.unobtrusive.adapters.addBool("validemail");
    


//strong password validation client side
$.validator.addMethod("strongpassword", function (value) {
    if (!value) return true;

    // Minimum 8 chars, uppercase, lowercase, digit, special char, no spaces
    if (value.length < 8) return false;
    if (/\s/.test(value)) return false;
    if (!/[A-Z]/.test(value)) return false;
    if (!/[a-z]/.test(value)) return false;
    if (!/\d/.test(value)) return false;
    if (!/[!@#$%^&*(),.?":{}|<>]/.test(value)) return false;

    // No sequential digits
    if (value.includes("1234") || value.includes("4567") || value.includes("7890")) return false;

    return true;
});

$.validator.unobtrusive.adapters.addBool("strongpassword");


