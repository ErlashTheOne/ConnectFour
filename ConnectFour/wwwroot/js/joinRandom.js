let createButton = document.querySelector("#joinRandom");
createButton.addEventListener("click", function () {
    window.location.href = `/Game?lobby=random&where=joinRandom`;
});