
const passwordInput = document.getElementById("passwordInput");
const emoji = document.getElementById("passwordEmoji");

// Initial emoji (hidden mode)
emoji.textContent = "🙈";

emoji.addEventListener("click", () => {
    if (passwordInput.type === "password") {
        passwordInput.type = "text";   // show password
        emoji.textContent = "👀";      // update emoji
    } else {
        passwordInput.type = "password"; // hide password
        emoji.textContent = "🙈";       // update emoji
    }
});
/*document.addEventListener("DOMContentLoaded", () => {
    const passwordInput = document.getElementById("passwordInput");
    const passwordEmoji = document.getElementById("passwordEmoji");

    if (!passwordInput) return;

    passwordInput.addEventListener("input", function () {
        const value = passwordInput.value;

        if (!value) {
            passwordEmoji.textContent = "";
            return;
        }

        // Strong password pattern (same as your server-side validation)
        const strongPattern = /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*(),.?":{}|<>])(?!.*\s).{8,}$/;
        // Check sequential digits
        const sequential = ["1234", "4567", "7890"];
        let hasSequential = sequential.some(seq => value.includes(seq));

        if (strongPattern.test(value) && !hasSequential) {
            passwordEmoji.textContent = "💪"; // strong password
            passwordEmoji.style.color = "green";
        } else {
            passwordEmoji.textContent = "⚠️"; // weak password
            passwordEmoji.style.color = "orange";
        }
    });
});*/