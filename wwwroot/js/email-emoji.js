const emailPattern =
    /^(?:[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*)@[A-Za-z0-9-]+(?:\.[A-Za-z]{2,10})+$/;

document.addEventListener("DOMContentLoaded", () => {
    const emailInput = document.getElementById("emailInput");
    const emailEmoji = document.getElementById("emailEmoji");

    if (!emailInput) return;

    emailInput.addEventListener("input", function () {
        const value = emailInput.value;

        if (!value) {
            emailEmoji.textContent = "";
            return;
        }

        if (emailPattern.test(value)) {
            emailEmoji.textContent = "😊";
            emailEmoji.style.color = "green";
        } else {
            emailEmoji.textContent = "😢";
            emailEmoji.style.color = "red";
        }
    });
});
